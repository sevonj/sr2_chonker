// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use byteorder::{LittleEndian, ReadBytesExt};
use std::{
    cmp::Ordering,
    fs::File,
    io::{BufRead, BufReader, Read, Seek},
};
use zerocopy::{FromBytes, IntoBytes};
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::{
    io_helper::seek_align, CribSomething, Material, MaterialHeader, MaterialTextureEntry,
    MaterialUnknown2, MaterialUnknown3, Mesh, MeshBuffer, MeshBufferType, MeshMover, Object,
    Unknown19, Unknown20, Unknown21, Unknown23, Unknown24, Unknown27, Unknown28, Unknown31,
    Unknown32, VertexBuffer,
};

use super::{
    GpuMeshUnkA, ModelHeader, ModelUnknownA, ModelUnknownB, ObjectModel, Sr2TypeError, Vector,
};

/// An instance of SR2 CPU chunk (.chunk_pc) file.
#[derive(Debug, Clone)]
pub struct Chunk {
    /// Not part of chunk format. Used to track progress of format reversing.
    pub bytes_mapped: usize,

    pub header: ChunkHeader,

    pub textures: Vec<String>,

    // Objects, object models
    pub mesh_unk_as: Vec<GpuMeshUnkA>,
    pub obj_models: Vec<ObjectModel>,
    pub obj_unknown_as: Vec<ModelUnknownA>,
    pub obj_unknown_bs: Vec<ModelUnknownB>,

    // This block is probably all collisions
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

    // Vertex & index buffers definitions and data
    pub mesh_buffers: Vec<MeshBuffer>,

    // Materials
    pub materials: Vec<Material>,
    /// These should go inside material. Figure out the relationship.
    pub shader_consts: Vec<f32>,
    /// These probably should go inside material. Figure out the relationship.
    pub material_unk_3: Vec<MaterialUnknown3>,

    /// GPU meshes that pull data from mesh_buffer
    pub meshes: Vec<Mesh>,

    pub objects: Vec<Object>,

    pub unknown_names: Vec<String>,

    pub unknown13: Vec<u32>,

    /// Yeah so there's a buffer filled with 0xCD bytes.
    pub cd_pad_size: u32,

    /// Only present in crib chunks.
    pub crib_smthn: Vec<CribSomething>,

    pub unknown19: Vec<Unknown19>,
    pub unknown20: Vec<Unknown20>,
    pub unknown21: Vec<Unknown21>,
    pub unknown22: Vec<u32>,
    pub unknown23: Vec<Unknown23>,
    pub unknown24: Unknown24,

    /// Only present in these 4 chunks:
    /// sr2_intaicutjyucar, sr2_intarcutlimo, sr2_intdkmissunkdk, sr2_skybox
    pub cutscn_skybox_smthn: Vec<u16>,

    pub movers: Vec<MeshMover>,
    pub mover_unknown27: Vec<Unknown27>,
    pub mover_unknown28: Vec<Unknown28>,
    pub mover_unknown29: Vec<u32>,
    /// Probably. Coords are relative.
    pub mover_positions: Vec<Vector>,
    pub mover_unknown31: Vec<Unknown31>,
    pub mover_unknown32: Vec<Unknown32>,

    pub remaining_data: Vec<u8>,
}

impl Chunk {
    pub const MAGIC: u32 = 0xBBCACA12;
    pub const VERSION: u32 = 121;

    /// Open a .chunk_pc file
    pub fn open(path: &str) -> Result<Self, Sr2TypeError> {
        let mut reader = BufReader::new(File::open(path)?);
        Self::read(&mut reader)
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let stream_start = reader.stream_position()? as usize;

        let chunk_header = {
            let mut buf = vec![0_u8; size_of::<ChunkHeader>()];
            reader.read_exact(&mut buf)?;
            ChunkHeader::read_from_bytes(&buf).unwrap()
        };
        if chunk_header.magic != Self::MAGIC {
            return Err(Sr2TypeError::ChunkInvalidMagic(chunk_header.magic));
        }
        if chunk_header.version != Self::VERSION {
            return Err(Sr2TypeError::ChunkInvalidVersion(chunk_header.version));
        }

        seek_align(reader, 16)?;

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

        let mut mesh_unk_as = vec![];
        let mut obj_models = vec![];
        let mut obj_unknown_as = vec![];
        let mut obj_unknown_bs = vec![];

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_meshes {
            let mut buf = vec![0_u8; size_of::<GpuMeshUnkA>()];
            reader.read_exact(&mut buf)?;
            mesh_unk_as.push(GpuMeshUnkA::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_obj_models {
            let mut buf = vec![0_u8; size_of::<ObjectModel>()];
            reader.read_exact(&mut buf)?;
            obj_models.push(ObjectModel::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_obj_unknown_a {
            let mut buf = vec![0_u8; size_of::<ModelUnknownA>()];
            reader.read_exact(&mut buf)?;
            obj_unknown_as.push(ModelUnknownA::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..model_header.num_obj_unknown_b {
            let mut buf = vec![0_u8; size_of::<ModelUnknownB>()];
            reader.read_exact(&mut buf)?;
            obj_unknown_bs.push(ModelUnknownB::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        let num_unknown5_vertices = reader.read_u32::<LittleEndian>()?;
        let mut world_collision_vbuf = vec![];
        for _ in 0..num_unknown5_vertices {
            let mut buf = vec![0_u8; size_of::<Vector>()];
            reader.read_exact(&mut buf)?;
            world_collision_vbuf.push(Vector::read_from_bytes(&buf).unwrap());
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
                    pos: reader.stream_position()?,
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
        for _ in 0..model_header.num_mesh_buffers {
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

        let mut meshes = vec![];
        for _ in 0..model_header.num_meshes {
            meshes.push(Mesh::read(reader)?);
        }

        let mut objects = vec![];
        for _ in 0..chunk_header.num_objects {
            objects.push(Object::read(reader)?);
        }
        for object in &mut objects {
            let mut buf = vec![];
            reader.read_until(0x00, &mut buf)?;
            object.name = String::from_utf8_lossy(&buf).to_string();
        }

        seek_align(reader, 16)?;

        let len_unk_names_buf = reader.read_u32::<LittleEndian>()?;
        let unk_names_buf_end = reader.stream_position()? + len_unk_names_buf as u64;
        let mut unknown_names = vec![];
        loop {
            let stream_pos = reader.stream_position()?;
            match stream_pos.cmp(&unk_names_buf_end) {
                Ordering::Less => {
                    let mut buf = vec![];
                    reader.read_until(0x00, &mut buf)?;
                    unknown_names.push(String::from_utf8_lossy(&buf).to_string());
                }
                Ordering::Equal => break,
                Ordering::Greater => {
                    let msg = "Overshot unknown names buffer".into();
                    return Err(Sr2TypeError::ChunkLostTrack {
                        msg,
                        pos: stream_pos,
                    });
                }
            }
        }

        seek_align(reader, 16)?;
        let num_unknown13 = reader.read_u32::<LittleEndian>()?;
        let mut unknown13 = vec![];
        for _ in 0..num_unknown13 {
            unknown13.push(reader.read_u32::<LittleEndian>()?);
        }

        seek_align(reader, 16)?;

        let cd_pad_size = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(cd_pad_size as i64)?;

        seek_align(reader, 16)?;

        let num_crib_smthn = reader.read_u32::<LittleEndian>()?;
        let mut crib_smthn = vec![];
        for _ in 0..num_crib_smthn {
            crib_smthn.push(CribSomething::read(reader)?);
        }

        seek_align(reader, 16)?;

        let num_unknown19 = reader.read_u32::<LittleEndian>()?;
        let mut unknown19 = vec![];
        for _ in 0..num_unknown19 {
            unknown19.push(Unknown19::read(reader)?);
        }

        let num_unknown20 = reader.read_u32::<LittleEndian>()?;
        let mut unknown20 = vec![];
        // Another 0xCD block
        reader.seek_relative(num_unknown20 as i64 * 12)?;
        for _ in 0..num_unknown20 {
            unknown20.push(Unknown20::read(reader)?);
        }

        seek_align(reader, 16)?;

        let num_unknown21 = reader.read_u32::<LittleEndian>()?;
        // Another 0xCD block
        reader.seek_relative(num_unknown21 as i64 * 8)?;
        let mut unknown21 = vec![];
        for _ in 0..num_unknown21 {
            unknown21.push(Unknown21::read(reader)?);
        }

        seek_align(reader, 16)?;

        let num_unknown22 = reader.read_u32::<LittleEndian>()?;
        let mut unknown22 = vec![];
        for _ in 0..num_unknown22 {
            unknown22.push(reader.read_u32::<LittleEndian>()?);
        }

        seek_align(reader, 16)?;

        let mut unknown23 = vec![];
        for _ in 0..chunk_header.num_unknown23 {
            unknown23.push(Unknown23::read(reader)?);
        }

        seek_align(reader, 16)?;

        let unknown24 = Unknown24::read(reader)?;

        seek_align(reader, 16)?;

        let mut cutscn_skybox_things = vec![];
        for _ in 0..chunk_header.num_cutscn_skybox_thing {
            cutscn_skybox_things.push(reader.read_u16::<LittleEndian>()?);
        }

        seek_align(reader, 16)?;

        let mut movers = vec![];
        let mut unknown27 = vec![];
        let mut unknown28 = vec![];
        let mut unknown29 = vec![];
        let mut mover_positions = vec![];
        let mut unknown31 = vec![];
        let mut unknown32 = vec![];
        for _ in 0..chunk_header.num_mesh_movers {
            movers.push(MeshMover::read(reader)?);
        }
        for _ in 0..chunk_header.num_unknown27 {
            unknown27.push(Unknown27::read(reader)?);
        }
        for _ in 0..chunk_header.num_unknown28 {
            unknown28.push(Unknown28::read(reader)?);
        }
        for _ in 0..chunk_header.num_unknown29 {
            unknown29.push(reader.read_u32::<LittleEndian>()?);
        }
        for _ in 0..chunk_header.num_unknown30 {
            let value = Vector::read(reader)?;
            mover_positions.push(value);
        }
        for _ in 0..chunk_header.num_unknown31 {
            unknown31.push(Unknown31::read(reader)?);
        }
        for _ in 0..chunk_header.num_unknown32 {
            unknown32.push(Unknown32::read(reader)?);
        }
        for mover in &mut movers {
            let mut buf = vec![];
            reader.read_until(0x00, &mut buf)?;
            mover.name = String::from_utf8_lossy(&buf).to_string();

            for start_name in &mut mover.starts {
                let mut buf = vec![];
                reader.read_until(0x00, &mut buf)?;
                *start_name = String::from_utf8_lossy(&buf).to_string();
            }
        }

        seek_align(reader, 16)?;

        let bytes_mapped = reader.stream_position()? as usize - stream_start;
        let mut remaining_data = vec![];
        reader.read_to_end(&mut remaining_data)?;

        Ok(Self {
            bytes_mapped,
            header: chunk_header,
            textures,
            mesh_unk_as,
            obj_models,
            obj_unknown_as,
            obj_unknown_bs,
            world_collision_vbuf,
            unknown6,
            unknown7,
            unknown8,
            mopp_buf,
            unk_bb_min,
            unk_bb_max,
            mesh_buffers,
            remaining_data,
            materials,
            shader_consts,
            material_unk_3,
            meshes,
            objects,
            unknown_names,
            unknown13,
            cd_pad_size,
            crib_smthn,
            unknown19,
            unknown20,
            unknown21,
            unknown22,
            unknown23,
            unknown24,
            cutscn_skybox_smthn: cutscn_skybox_things,
            movers,
            mover_unknown27: unknown27,
            mover_unknown28: unknown28,
            mover_unknown29: unknown29,
            mover_positions,
            mover_unknown31: unknown31,
            mover_unknown32: unknown32,
        })
    }

    /// Serialize back to a .chunk_pc file
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut buf = vec![];

        let mut chunk_header = self.header.clone();
        chunk_header.num_unknown23 = self.unknown23.len() as u32;
        chunk_header.num_mesh_movers = self.movers.len() as u32;
        chunk_header.num_unknown27 = self.mover_unknown27.len() as u32;
        chunk_header.num_unknown28 = self.mover_unknown28.len() as u32;
        chunk_header.num_unknown29 = self.mover_unknown29.len() as u32;
        chunk_header.num_unknown30 = self.mover_positions.len() as u32;
        chunk_header.num_unknown31 = self.mover_unknown31.len() as u32;
        chunk_header.num_unknown32 = self.mover_unknown32.len() as u32;
        buf.extend_from_slice(chunk_header.as_bytes());

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

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
        buf.extend_from_slice(
            (ModelHeader {
                num_meshes: self.meshes.len() as u32,
                num_obj_models: self.obj_models.len() as u32,
                num_mesh_buffers: self.mesh_buffers.len() as u32,
                num_obj_unknown_a: self.obj_unknown_as.len() as u32,
                num_obj_unknown_b: self.obj_unknown_bs.len() as u32,
            })
            .as_bytes(),
        );

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for gpu_mesh_unk_a in &self.mesh_unk_as {
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
        for model_unk_a in &self.obj_unknown_as {
            buf.extend_from_slice(model_unk_a.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        for model_unk_b in &self.obj_unknown_bs {
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
        buf.extend_from_slice(
            MaterialHeader::new(
                self.materials.len() as u32,
                self.shader_consts.len() as u32,
                self.material_unk_3.len() as u32,
            )
            .as_bytes(),
        );
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

        for mesh_data in &self.meshes {
            buf.extend_from_slice(&mesh_data.to_bytes());

            while buf.len() % 16 != 0 {
                buf.push(0);
            }

            if let Some(submesh) = &mesh_data.submesh_a {
                buf.extend_from_slice(&submesh.to_bytes());
            }
            if let Some(submesh) = &mesh_data.submesh_b {
                buf.extend_from_slice(&submesh.to_bytes());
            }

            if let Some(submesh) = &mesh_data.submesh_a {
                for surf in &submesh.surfaces {
                    buf.extend_from_slice(surf.as_bytes());
                }
            }
            if let Some(submesh) = &mesh_data.submesh_b {
                for surf in &submesh.surfaces {
                    buf.extend_from_slice(surf.as_bytes());
                }
            }
        }

        for object in &self.objects {
            buf.extend_from_slice(&object.to_bytes());
        }
        for object in &self.objects {
            buf.extend_from_slice(object.name.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        let mut unk_names_buf = vec![];
        for name in &self.unknown_names {
            unk_names_buf.extend_from_slice(name.as_bytes());
        }
        buf.extend_from_slice(&(unk_names_buf.len() as u32).to_le_bytes());
        buf.extend_from_slice(&unk_names_buf);

        while buf.len() % 16 != 0 {
            buf.push(0);
        }
        buf.extend_from_slice(&(self.unknown13.len() as u32).to_le_bytes());
        buf.extend_from_slice(self.unknown13.as_bytes());

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(self.cd_pad_size.as_bytes());
        buf.extend_from_slice(&vec![0xcd_u8; self.cd_pad_size as usize]);

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&(self.crib_smthn.len() as u32).to_le_bytes());
        for unknown18 in &self.crib_smthn {
            buf.extend_from_slice(&unknown18.to_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&(self.unknown19.len() as u32).to_le_bytes());
        for unknown19 in &self.unknown19 {
            buf.extend_from_slice(unknown19.as_bytes());
        }
        buf.extend_from_slice(&(self.unknown20.len() as u32).to_le_bytes());
        buf.extend_from_slice(&vec![0xcd_u8; self.unknown20.len() * 12]);
        for unknown20 in &self.unknown20 {
            buf.extend_from_slice(unknown20.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&(self.unknown21.len() as u32).to_le_bytes());
        buf.extend_from_slice(&vec![0xcd_u8; self.unknown21.len() * 8]);
        for unknown21 in &self.unknown21 {
            buf.extend_from_slice(unknown21.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&(self.unknown22.len() as u32).to_le_bytes());
        for unknown22 in &self.unknown22 {
            buf.extend_from_slice(&unknown22.to_le_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        for unknown23 in &self.unknown23 {
            buf.extend_from_slice(unknown23.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&self.unknown24.to_bytes());

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        for value in &self.cutscn_skybox_smthn {
            buf.extend_from_slice(value.as_bytes());
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        for mover in &self.movers {
            buf.extend_from_slice(&mover.to_bytes());
        }
        for unknown27 in &self.mover_unknown27 {
            buf.extend_from_slice(unknown27.as_bytes());
        }
        for unknown28 in &self.mover_unknown28 {
            buf.extend_from_slice(unknown28.as_bytes());
        }
        for unknown29 in &self.mover_unknown29 {
            buf.extend_from_slice(&unknown29.to_le_bytes());
        }
        for unknown30 in &self.mover_positions {
            buf.extend_from_slice(unknown30.as_bytes());
        }
        for unknown31 in &self.mover_unknown31 {
            buf.extend_from_slice(unknown31.as_bytes());
        }
        for unknown32 in &self.mover_unknown32 {
            buf.extend_from_slice(unknown32.as_bytes());
        }
        for mover in &self.movers {
            buf.extend_from_slice(mover.name.as_bytes());
            for start_name in &mover.starts {
                buf.extend_from_slice(start_name.as_bytes());
            }
        }

        while buf.len() % 16 != 0 {
            buf.push(0);
        }

        buf.extend_from_slice(&self.remaining_data);

        buf
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ChunkHeader {
    pub magic: u32,
    pub version: u32,
    pub header0x08: i32,
    pub header0x0c: i32,
    pub header0x10: i32,

    // Checksums? Volition uses checksums a lot, and the values really don't
    // seem to make sense for anything else. [V]'s hashing algorithms are known
    // but testing them with all data in the chunk would be a pain.
    pub hash_0x14: i32,
    pub hash_0x18: i32,
    pub hash_0x20: i32,
    pub hash_0x24: i32,
    pub hash_0x28: i32,
    pub hash_0x2c: i32,
    pub hash_0x30: i32,
    pub hash_0x34: i32,
    pub hash_0x38: i32,
    pub hash_0x3c: i32,
    pub hash_0x40: i32,
    pub hash_0x44: i32,
    pub hash_0x48: i32,
    pub hash_0x4c: i32,
    pub hash_0x50: i32,
    pub hash_0x54: i32,
    pub hash_0x58: i32,
    pub hash_0x5c: i32,
    pub hash_0x60: i32,
    pub hash_0x64: i32,
    pub hash_0x68: i32,
    pub hash_0x6c: i32,
    pub hash_0x70: i32,
    pub hash_0x74: i32,
    pub hash_0x78: i32,
    pub hash_0x7c: i32,
    pub hash_0x80: i32,
    pub hash_0x84: i32,
    pub hash_0x88: i32,
    pub hash_0x8c: i32,

    /// First half of GPU chunk. Contains models.
    pub len_gpu_chunk_a: u32,
    /// Second half of GPU chunk.
    /// Probably contains destroyable objects or something weird.
    pub len_gpu_chunk_b: u32,

    // Everything from here to bbox_min that is so far discovered is a count.
    // Therefore it's reasonable to expect the unknowns
    pub num_objects: u32,
    pub num_unknown23: u32,
    /// num_something?
    pub header0x9c: i32,
    /// num_something?
    pub header0xa0: u32,
    /// num_something?
    pub header0xa4: u32,
    /// num_something?
    pub header0xa8: u32,
    /// num_something?
    pub header0xac: u32,
    /// num_something?
    pub header0xb4: u32,

    // This block is potentially all mover stuff, as all of these unknowns'
    // data is sandwiched between mesh_movers and mesh_mover_names
    pub num_mesh_movers: u32,
    pub num_unknown27: u32,
    pub num_unknown28: u32,
    pub num_unknown29: u32,
    pub num_unknown30: u32,
    pub num_unknown31: u32,

    /// num_something?
    pub unk_0xcc: u32,

    /// Always zero, except in these 4 chunks:
    /// sr2_intaicutjyucar, sr2_intarcutlimo, sr2_intdkmissunkdk, sr2_skybox
    pub num_cutscn_skybox_thing: u32,

    /// Load zone???
    pub bbox_min: Vector,
    /// Load zone???
    pub bbox_max: Vector,
    /// Repeat of bbox_min.y?
    pub bbox_min_y: f32,

    /// Located between mesh mover stuff
    pub num_unknown32: u32,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_chunk_header_size() {
        assert_eq!(size_of::<ChunkHeader>(), 0xf4);
    }
}
