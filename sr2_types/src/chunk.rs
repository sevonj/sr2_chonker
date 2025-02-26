// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use byteorder::{LittleEndian, ReadBytesExt};
use std::{
    fs::File,
    io::{BufRead, BufReader, Read, Seek},
};
use zerocopy::{FromBytes, IntoBytes};
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::{
    Material, MaterialHeader, MaterialTextureEntry, MaterialUnknown2, MaterialUnknown3, MeshBuffer,
    MeshBufferType, VertexBuffer,
};

use super::{
    GpuMeshUnkA, ModelHeader, ModelUnknownA, ModelUnknownB, ObjectModel, Sr2TypeError, Vector,
};

/// An instance of SR2 CPU chunk (.chunk_pc) file.
#[derive(Debug, Clone)]
pub struct Chunk {
    pub header: ChunkHeader,

    pub textures: Vec<String>,

    pub model_header: ModelHeader,
    pub gpu_mesh_unk_as: Vec<GpuMeshUnkA>,
    pub obj_models: Vec<ObjectModel>,
    pub model_unk_as: Vec<ModelUnknownA>,
    pub model_unk_bs: Vec<ModelUnknownB>,

    pub world_collision_vbuf: Vec<Vector>,
    /// Unknown buffer, may or may not relate to unknown5
    pub unknown6: Vec<u8>,
    /// Type: unknown 4B
    pub unknown7: Vec<u8>,
    /// Type: unknown 12B. Not floats.
    pub unknown8: Vec<u8>,
    /// Havok collisions stuff
    pub mopp_buf: Vec<u8>,
    pub unk_bb_min: Vector,
    pub unk_bb_max: Vector,

    pub mesh_buffers: Vec<MeshBuffer>,

    pub mat_header: MaterialHeader,
    pub materials: Vec<Material>,
    pub shader_consts: Vec<f32>,
    pub material_unk_3: Vec<MaterialUnknown3>,

    pub remaining_data: Vec<u8>,
}

impl Chunk {
    pub const MAGIC: u32 = 0xBBCACA12;
    pub const VERION: u32 = 121;

    /// Open a .chunk_pc file
    pub fn open(path: &str) -> Result<Self, Sr2TypeError> {
        let mut reader = BufReader::new(File::open(path)?);
        Self::read(&mut reader)
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let header = {
            let mut buf = vec![0_u8; size_of::<ChunkHeader>()];
            reader.read_exact(&mut buf)?;
            ChunkHeader::read_from_bytes(&buf).unwrap()
        };
        if header.magic != Self::MAGIC {
            return Err(Sr2TypeError::ChunkInvalidMagic(header.magic));
        }
        if header.version != Self::VERION {
            return Err(Sr2TypeError::ChunkInvalidVersion(header.version));
        }

        let num_textures = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_textures as i64 * 4)?;

        let mut textures = vec![];
        for _ in 0..num_textures {
            let mut buf = vec![];
            reader.read_until(0x00, &mut buf)?;
            textures.push(String::from_utf8_lossy(&buf).to_string());
        }

        seek_align(reader, 16)?;

        let model_header = {
            let mut buf = vec![0_u8; size_of::<ModelHeader>()];
            reader.read_exact(&mut buf)?;
            ModelHeader::read_from_bytes(&buf).unwrap()
        };

        let mut gpu_mesh_unk_as = vec![];
        let mut obj_models = vec![];
        let mut model_unk_as = vec![];
        let mut model_unk_bs = vec![];

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_gpu_meshes {
            let mut buf = vec![0_u8; size_of::<GpuMeshUnkA>()];
            reader.read_exact(&mut buf)?;
            gpu_mesh_unk_as.push(GpuMeshUnkA::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_obj_models {
            let mut buf = vec![0_u8; size_of::<ObjectModel>()];
            reader.read_exact(&mut buf)?;
            obj_models.push(ObjectModel::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_model_unknown_a {
            let mut buf = vec![0_u8; size_of::<ModelUnknownA>()];
            reader.read_exact(&mut buf)?;
            model_unk_as.push(ModelUnknownA::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_model_unknown_b {
            let mut buf = vec![0_u8; size_of::<ModelUnknownB>()];
            reader.read_exact(&mut buf)?;
            model_unk_bs.push(ModelUnknownB::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        let num_unknown5_vertices = reader.read_u32::<LittleEndian>()?;
        let mut unknown5_vbuf = vec![];
        for _ in 0..num_unknown5_vertices {
            let mut buf = vec![0_u8; size_of::<Vector>()];
            reader.read_exact(&mut buf)?;
            unknown5_vbuf.push(Vector::read_from_bytes(&buf).unwrap());
        }

        let num_unknown6 = reader.read_u32::<LittleEndian>()?;
        let mut unknown6 = vec![0_u8; num_unknown6 as usize * 3];
        reader.read_exact(&mut unknown6)?;

        let num_unknown7 = reader.read_u32::<LittleEndian>()?;
        let mut unknown7 = vec![0_u8; num_unknown7 as usize * 4];
        reader.read_exact(&mut unknown7)?;

        let num_unknown8 = reader.read_u32::<LittleEndian>()?;
        let mut unknown8 = vec![0_u8; num_unknown8 as usize * 12];
        reader.read_exact(&mut unknown8)?;

        seek_align(reader, 16)?;

        let len_mopp = reader.read_u32::<LittleEndian>()?;
        seek_align(reader, 16)?;
        if len_mopp != 0 {
            // Using MOPP signature as a landmark.
            let mut buf = vec![0_u8; 4];
            reader.read_exact(&mut buf)?;
            if String::from_utf8_lossy(&buf).to_string().as_str() != "MOPP" {
                return Err(Sr2TypeError::ChunkLostTrack {
                    msg: "First MOPP signature didn't appear where expected.".into(),
                    pos: reader.stream_position().unwrap() as i64,
                });
            }
            reader.seek_relative(-4)?;
        }
        let mut mopp_buf = vec![0_u8; len_mopp as usize];
        reader.read_exact(&mut mopp_buf)?;

        seek_align(reader, 4)?;

        // Collision area AABB?
        let unk_bb_min = {
            let mut buf = vec![0_u8; size_of::<Vector>()];
            reader.read_exact(&mut buf)?;
            Vector::read_from_bytes(&buf).unwrap()
        };
        let unk_bb_max = {
            let mut buf = vec![0_u8; size_of::<Vector>()];
            reader.read_exact(&mut buf)?;
            Vector::read_from_bytes(&buf).unwrap()
        };

        seek_align(reader, 16)?;

        let mut mesh_buffers: Vec<MeshBuffer> = vec![];
        for _ in 0..model_header.num_meshes {
            mesh_buffers.push(MeshBuffer::read(reader)?);
        }
        for mesh_buffer in &mut mesh_buffers {
            for vertex_buffer in &mut mesh_buffer.vertex_buffers {
                *vertex_buffer = VertexBuffer::read(reader)?;
            }
        }

        for mesh_buffer in &mut mesh_buffers {
            if mesh_buffer.buffer_type != MeshBufferType::Cpu {
                continue;
            }
            seek_align(reader, 16)?;
            for vbuf in &mut mesh_buffer.vertex_buffers {
                reader.read_exact(&mut vbuf.data)?;
            }
            seek_align(reader, 16)?;
            for indy in &mut mesh_buffer.indices {
                *indy = reader.read_u16::<LittleEndian>()?;
            }
        }

        seek_align(reader, 16)?;

        let mut materials = vec![];
        let mat_header = MaterialHeader::read(reader)?;
        for _ in 0..mat_header.num_materials {
            materials.push(Material::read(reader)?);
        }
        for material in &mut materials {
            seek_align(reader, 4)?;
            let mut buf = vec![0_u8; size_of::<MaterialUnknown2>()];
            for unk2 in &mut material.unknown2 {
                reader.read_exact(&mut buf)?;
                *unk2 = MaterialUnknown2::read_from_bytes(&buf).unwrap()
            }
        }
        for material in &mut materials {
            reader.read_exact(&mut material.unknown_16b_struct)?;
        }

        seek_align(reader, 16)?;

        let mut shader_consts = vec![0_f32; mat_header.num_shader_constants as usize];
        for shad_const in &mut shader_consts {
            *shad_const = reader.read_f32::<LittleEndian>()?;
        }

        for material in &mut materials {
            let mut buf = vec![0_u8; size_of::<MaterialTextureEntry>()];
            for texture_entry in &mut material.textures {
                reader.read_exact(&mut buf)?;
                *texture_entry = MaterialTextureEntry::read_from_bytes(&buf).unwrap()
            }
            let num_unused = 16 - material.textures.len();
            reader.seek_relative((num_unused * size_of::<MaterialTextureEntry>()) as i64)?;
        }

        let mut material_unk_3 = vec![];
        for _ in 0..mat_header.num_mat_unknown3 {
            material_unk_3.push(MaterialUnknown3::read(reader)?);
        }
        for mat_unk3 in &mut material_unk_3 {
            for value in &mut mat_unk3.unk_data {
                *value = reader.read_u32::<LittleEndian>()?;
            }
        }

        let mut remaining_data = vec![];
        reader.read_to_end(&mut remaining_data)?;

        Ok(Self {
            header,
            textures,
            model_header,
            gpu_mesh_unk_as,
            obj_models,
            model_unk_as,
            model_unk_bs,
            world_collision_vbuf: unknown5_vbuf,
            unknown6,
            unknown7,
            unknown8,
            mopp_buf,
            unk_bb_min,
            unk_bb_max,
            mesh_buffers,
            remaining_data,
            mat_header,
            materials,
            shader_consts,
            material_unk_3,
        })
    }

    /// Serialize back to a .chunk_pc file
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut buf = vec![];

        buf.extend_from_slice(self.header.as_bytes());

        buf.extend_from_slice(&(self.textures.len() as u32).to_le_bytes());
        for _ in &self.textures {
            buf.extend_from_slice(&[0, 0, 0, 0]);
        }
        for texture in &self.textures {
            buf.extend_from_slice(texture.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(self.model_header.as_bytes());

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for gpu_mesh_unk_a in &self.gpu_mesh_unk_as {
            buf.extend_from_slice(gpu_mesh_unk_a.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for obj_model in &self.obj_models {
            buf.extend_from_slice(obj_model.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for model_unk_a in &self.model_unk_as {
            buf.extend_from_slice(model_unk_a.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for model_unk_b in &self.model_unk_bs {
            buf.extend_from_slice(model_unk_b.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(&(self.world_collision_vbuf.len() as u32).to_le_bytes());
        buf.extend_from_slice(self.world_collision_vbuf.as_bytes());
        buf.extend_from_slice(&(self.unknown6.len() as u32 / 3).to_le_bytes());
        buf.extend_from_slice(&self.unknown6);
        buf.extend_from_slice(&(self.unknown7.len() as u32 / 4).to_le_bytes());
        buf.extend_from_slice(&self.unknown7);
        buf.extend_from_slice(&(self.unknown8.len() as u32 / 12).to_le_bytes());
        buf.extend_from_slice(&self.unknown8);

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(&(self.mopp_buf.len() as u32).to_le_bytes());
        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(&self.mopp_buf);

        while buf.len() % 4 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(self.unk_bb_min.as_bytes());
        buf.extend_from_slice(self.unk_bb_max.as_bytes());

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for mesh_buffer in &self.mesh_buffers {
            buf.extend_from_slice(&mesh_buffer.to_bytes());
        }
        for mesh_buffer in &self.mesh_buffers {
            for vbuf in &mesh_buffer.vertex_buffers {
                buf.extend_from_slice(&vbuf.to_bytes());
            }
        }
        for mesh_buffer in &self.mesh_buffers {
            if mesh_buffer.buffer_type != MeshBufferType::Cpu {
                continue;
            }
            while buf.len() % 16 != 0 {
                buf.push(0);
            }
            for vbuf in &mesh_buffer.vertex_buffers {
                buf.extend_from_slice(&vbuf.data);
            }
            while buf.len() % 16 != 0 {
                buf.push(0);
            }
            buf.extend_from_slice(mesh_buffer.indices.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(self.mat_header.as_bytes());
        for material in &self.materials {
            buf.extend_from_slice(&material.to_bytes());
        }
        for material in &self.materials {
            while buf.len() % 4 != 0 {
                buf.push(0);
            }
            buf.extend_from_slice(material.unknown2.as_bytes());
        }
        for material in &self.materials {
            buf.extend_from_slice(&material.unknown_16b_struct);
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(self.shader_consts.as_bytes());

        for material in &self.materials {
            buf.extend_from_slice(material.textures.as_bytes());
            let num_unused = 16 - material.textures.len();
            for _ in 0..num_unused {
                buf.extend_from_slice(MaterialTextureEntry::placeholder().as_bytes());
            }
        }

        for unk3 in &self.material_unk_3 {
            buf.extend_from_slice(&unk3.to_bytes());
        }
        for unk3 in &self.material_unk_3 {
            buf.extend_from_slice(unk3.unk_data.as_bytes());
        }

        buf.extend_from_slice(&self.remaining_data);

        buf
    }
}

fn seek_align<R: Seek>(reader: &mut R, size: i64) -> Result<(), std::io::Error> {
    let pos = reader.stream_position().unwrap() as i64;
    reader.seek_relative((size - (pos % size)) % size)
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ChunkHeader {
    pub magic: u32,
    pub version: u32,
    pub header0x08: i32,
    pub header0x0c: i32,
    pub header0x10: i32,

    /// Hash?
    pub header0x14: i32,
    /// Hash?
    pub header0x18: i32,
    /// Hash?
    pub header0x20: i32,
    /// Hash?
    pub header0x24: i32,
    /// Hash?
    pub header0x28: i32,
    /// Hash?
    pub header0x2c: i32,
    /// Hash?
    pub header0x30: i32,
    /// Hash?
    pub header0x34: i32,
    /// Hash?
    pub header0x38: i32,
    /// Hash?
    pub header0x3c: i32,
    /// Hash?
    pub header0x40: i32,
    /// Hash?
    pub header0x44: i32,
    /// Hash?
    pub header0x48: i32,
    /// Hash?
    pub header0x4c: i32,
    /// Hash?
    pub header0x50: i32,
    /// Hash?
    pub header0x54: i32,
    /// Hash?
    pub header0x58: i32,
    /// Hash?
    pub header0x5c: i32,
    /// Hash?
    pub header0x60: i32,
    /// Hash?
    pub header0x64: i32,
    /// Hash?
    pub header0x68: i32,
    /// Hash?
    pub header0x6c: i32,
    /// Hash?
    pub header0x70: i32,
    /// Hash?
    pub header0x74: i32,
    /// Hash?
    pub header0x78: i32,
    /// Hash?
    pub header0x7c: i32,
    /// Hash?
    pub header0x80: i32,
    /// Hash?
    pub header0x84: i32,
    /// Hash?
    pub header0x88: i32,
    /// Hash?
    pub header0x8c: i32,

    /// First half of GPU chunk. Contains models.
    pub len_gpu_chunk_a: i32,
    /// Second half of GPU chunk.
    /// Probably contains destroyable objects.
    pub len_gpu_chunk_b: i32,

    pub num_city_objects: i32,
    pub num_unknown23s: i32,
    /// num_something?
    pub header0x9c: i32,
    /// num_something?
    pub header0xa0: i32,
    /// num_something?
    pub header0xa4: i32,
    /// num_something?
    pub header0xa8: i32,
    /// num_something?
    pub header0xac: i32,
    /// num_something?
    pub header0xb4: i32,
    pub num_mesh_movers: i32,
    pub num_unknown27s: i32,
    pub num_unknown28s: i32,
    pub num_unknown29s: i32,
    pub num_unknown30s: i32,
    pub num_unknown31s: i32,
    /// num_something?
    pub header0xcc: i32,
    /// num_something?
    pub header0xd0: i32,

    /// Load zone???
    pub bbox_min: Vector,
    /// Load zone???
    pub bbox_max: Vector,

    pub header0xec: f32,
    pub header0xf0: i32,
    pub header0xf4: i32,
    pub header0xf8: i32,
    pub header0xfc: i32,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_chunk_header_size() {
        assert_eq!(size_of::<ChunkHeader>(), 0x100);
    }
}
