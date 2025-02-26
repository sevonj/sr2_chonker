// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! SR2 mesh geometry types
//!
//! Layout:
//! - align(16)
//! - [ModelHeader]
//! - align(16)
//! - [GpuMeshUnkA] * num_gpu_meshes
//! - align(16)
//! - [ObjectModel] * num_obj_model
//! - align(16)
//! - [ModelUnknownA] * num_model_unknown_a
//! - align(16)
//! - [ModelUnknownB] * num_model_unknown_b
//!
//! ... other stuff ...
//!
//! - [MeshHeader] * num_meshes
//! - [VertexBufHeader] * mesh_header.num_vertex_buffers for mesh_header
//! - ([Vector] * mesh_header.num_indices, [u16] * vert_header for mesh_header.num_vertex_buffers) for mesh_header

use byteorder::{LittleEndian, ReadBytesExt};
use std::io::{BufReader, Read, Seek};
use zerocopy::{FromBytes, IntoBytes};
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::{io_helper::seek_align, Sr2TypeError};

use super::{Transform, Vector};

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(u16)]
pub enum MeshBufferType {
    /// Stored in GPU chunk
    Gpu = 0,
    /// Stored in CPU chunk
    Cpu = 7,
}

impl TryFrom<u16> for MeshBufferType {
    type Error = Sr2TypeError;

    fn try_from(value: u16) -> Result<Self, Self::Error> {
        match value {
            0 => Ok(Self::Gpu),
            7 => Ok(Self::Cpu),
            _ => Err(Sr2TypeError::UnexpectedMeshBufferType),
        }
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ModelHeader {
    pub num_gpu_meshes: u32,
    pub num_obj_models: u32,
    pub num_meshes: u32,
    pub num_model_unknown_a: u32,
    pub num_model_unknown_b: u32,
}

/// Always same count as GPU meshes. Purpose unknown.
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct GpuMeshUnkA {
    pub unk_0x00: i32,
    pub unk_0x04: i32,
    pub unk_0x08: i32,
    pub unk_0x0c: i32,
    pub unk_0x10: i32,
    pub unk_0x14: i32,
}

/// Contains transform and rendermodel id.
/// Every object has one of these, but usually a few are left over.
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ObjectModel {
    pub origin: Vector,
    pub xform: Transform,

    /// Always null?
    pub unk_0x30: i32,
    /// Always null?
    pub unk_0x34: i32,
    /// Always null?
    pub unk_0x38: i32,
    /// Always null?
    pub unk_0x3c: i32,
    /// Always null?
    pub unk_0x40: i32,
    /// Always null?
    pub unk_0x44: i32,
    /// Always null?
    pub unk_0x48: i32,

    /// Always 1f?
    pub unk_0x4c: f32,

    /// Always null?
    pub unk_0x50: i32,

    pub unk_0x54: f32,
    pub idx_gpu_model: i32,
    /// flags?
    pub unk_0x5c: i32,
}

/// Probably part of objects or their models somehow
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ModelUnknownA {
    pub lotsa_floats: [f32; 0x19],
}

/// Probably part of objects or their models somehow
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct ModelUnknownB {
    pub lotsa_floats: [f32; 0xd],
}

/// Contains both vertex and index buffers. Data may be pulled from either CPU or GPU chunk.
#[derive(Debug, Clone)]
#[repr(C)]
pub struct MeshBuffer {
    pub buffer_type: MeshBufferType,
    pub vertex_buffers: Vec<VertexBuffer>,
    pub indices: Vec<u16>,
}

impl MeshBuffer {
    pub fn new(buffer_type: MeshBufferType) -> Self {
        Self {
            buffer_type,
            vertex_buffers: vec![],
            indices: vec![],
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mesh_type = reader.read_u16::<LittleEndian>()?;
        let num_vertex_buffers = reader.read_u16::<LittleEndian>()?;
        let num_indices = reader.read_u32::<LittleEndian>()?;
        let runtime_0x08 = reader.read_i32::<LittleEndian>()?;
        let runtime_0x0c = reader.read_i32::<LittleEndian>()?;
        let runtime_0x10 = reader.read_u32::<LittleEndian>()?;

        let this = Self {
            buffer_type: MeshBufferType::try_from(mesh_type)?,
            vertex_buffers: vec![VertexBuffer::placeholder(); num_vertex_buffers as usize],
            indices: vec![0_u16; num_indices as usize],
        };

        if runtime_0x08 != -1 {
            let pos = reader.stream_position().unwrap() - 0xc;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x0c != -1 {
            let pos = reader.stream_position().unwrap() - 0x8;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x10 != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(this)
    }

    /// Serialize header
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&(self.buffer_type as u16).to_le_bytes());
        bytes.extend_from_slice(&(self.vertex_buffers.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&(self.indices.len() as u32).to_le_bytes());
        bytes.extend_from_slice(&(-1_i32).to_le_bytes());
        bytes.extend_from_slice(&(-1_i32).to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes
    }
}

#[derive(Debug, Clone)]
#[repr(C)]
pub struct VertexBuffer {
    /// Number of unknown data in vertex. Each adds 2B to vert size.
    pub num_vertex_a: u8,
    /// Number of uv sets. Each adds 4B to vert size.
    pub num_uvs: u8,
    /// Vertex buffer data in bytes
    pub data: Vec<u8>,
}

impl VertexBuffer {
    /// New with empty buffer
    pub fn new(num_vertex_a: u8, num_uvs: u8) -> Self {
        Self {
            num_vertex_a,
            num_uvs,

            data: vec![],
        }
    }

    pub fn placeholder() -> Self {
        Self {
            num_vertex_a: 0xff,
            num_uvs: 0xff,

            data: vec![],
        }
    }

    /// Size of one vertex
    pub fn stride(&self) -> usize {
        (12 + 2 * self.num_vertex_a + 4 * self.num_uvs) as usize
    }

    pub fn num_vertices(&self) -> usize {
        self.data.len() / self.stride()
    }

    pub fn validate(&self) -> Result<(), Sr2TypeError> {
        if self.data.len() % self.stride() != 0 {
            return Err(Sr2TypeError::VertexBufLenStrideMismatch);
        }
        Ok(())
    }

    /// Construct from stream.
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let num_vertex_a = reader.read_u8()?;
        let num_uvs = reader.read_u8()?;
        let stride = reader.read_u16::<LittleEndian>()?;
        let num_vertices = reader.read_u32::<LittleEndian>()?;
        let runtime_0x08 = reader.read_i32::<LittleEndian>()?;
        let runtime_0x0c = reader.read_u32::<LittleEndian>()?;

        let buffer_size = stride as usize * num_vertices as usize;

        let this = Self {
            num_vertex_a,
            num_uvs,
            data: vec![0_u8; buffer_size],
        };

        if this.stride() != stride as usize {
            let pos = reader.stream_position().unwrap() - 0x10;
            return Err(Sr2TypeError::VertexStrideMismatch { pos });
        }
        if runtime_0x08 != -1 {
            let pos = reader.stream_position().unwrap() - 0x8;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x0c != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(this)
    }

    /// Serialize header
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.num_vertex_a.to_le_bytes());
        bytes.extend_from_slice(&self.num_uvs.to_le_bytes());
        bytes.extend_from_slice(&(self.stride() as u16).to_le_bytes());
        bytes.extend_from_slice(&(self.num_vertices() as u32).to_le_bytes());
        bytes.extend_from_slice(&(-1_i32).to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes
    }
}

/// A mesh that gets its data from the GPU chunk
#[derive(Debug, Clone)]
#[repr(C)]
pub struct MeshData {
    pub bbox_min: Vector,
    pub bbox_max: Vector,
    pub unk_0x18: u32,
    pub unk_0x1c: u32,
    pub submesh_a: Option<Submesh>,
    pub submesh_b: Option<Submesh>,
}

impl MeshData {
    /// Construct from stream.
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let bbox_min = Vector::read(reader)?;
        let bbox_max = Vector::read(reader)?;
        let unk_0x18 = reader.read_u32::<LittleEndian>()?;
        let unk_0x1c = reader.read_u32::<LittleEndian>()?;
        let ptr_submesh_a = reader.read_i32::<LittleEndian>()?;
        let ptr_submesh_b = reader.read_i32::<LittleEndian>()?;

        seek_align(reader, 16)?;

        let mut submesh_a = match ptr_submesh_a {
            0 => None,
            -1 => Some(Submesh::read(reader)?),
            _ => {
                return Err(Sr2TypeError::UnexpectedData {
                    pos: reader.stream_position().unwrap() - 0x8,
                });
            }
        };
        let mut submesh_b = match ptr_submesh_b {
            0 => None,
            -1 => Some(Submesh::read(reader)?),
            _ => {
                return Err(Sr2TypeError::UnexpectedData {
                    pos: reader.stream_position().unwrap() - 0x4,
                });
            }
        };

        if let Some(submesh) = &mut submesh_a {
            for surf in &mut submesh.surfaces {
                *surf = Surface::read(reader)?;
            }
        }
        if let Some(submesh) = &mut submesh_b {
            for surf in &mut submesh.surfaces {
                *surf = Surface::read(reader)?;
            }
        }

        let mesh_data = Self {
            bbox_min,
            bbox_max,
            unk_0x18,
            unk_0x1c,
            submesh_a,
            submesh_b,
        };

        Ok(mesh_data)
    }

    /// Serialize
    pub fn to_bytes(&self) -> Vec<u8> {
        let ptr_submesh_a: i32 = if self.submesh_a.is_some() { -1 } else { 0 };
        let ptr_submesh_b: i32 = if self.submesh_b.is_some() { -1 } else { 0 };

        let mut bytes = vec![];
        bytes.extend_from_slice(self.bbox_min.as_bytes());
        bytes.extend_from_slice(self.bbox_max.as_bytes());
        bytes.extend_from_slice(&self.unk_0x18.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x1c.to_le_bytes());
        bytes.extend_from_slice(&ptr_submesh_a.to_le_bytes());
        bytes.extend_from_slice(&ptr_submesh_b.to_le_bytes());

        bytes
    }
}

/// A mesh that gets its data from the GPU chunk
#[derive(Debug, Clone)]
#[repr(C)]
pub struct Submesh {
    pub unknown_0x00: u16,
    pub surfaces: Vec<Surface>,
    pub unknown_0x04: i32,
    pub unknown_0x08: u32,
}

impl Submesh {
    #[allow(clippy::new_without_default)]
    pub fn new() -> Self {
        Self {
            unknown_0x00: 0,
            surfaces: vec![],
            unknown_0x04: 0,
            unknown_0x08: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let unknown_0x00 = reader.read_u16::<LittleEndian>()?;
        let num_surfaces = reader.read_u16::<LittleEndian>()?;
        let unknown_0x04 = reader.read_i32::<LittleEndian>()?;
        let unknown_0x08 = reader.read_u32::<LittleEndian>()?;
        let runtime_0x0c = reader.read_u32::<LittleEndian>()?;

        if runtime_0x0c != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(Self {
            unknown_0x00,
            surfaces: vec![Surface::placeholder(); num_surfaces as usize],
            unknown_0x04,
            unknown_0x08,
        })
    }

    /// Serialize
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.unknown_0x00.to_le_bytes());
        bytes.extend_from_slice(&(self.surfaces.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&self.unknown_0x04.to_le_bytes());
        bytes.extend_from_slice(&self.unknown_0x08.to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());

        bytes
    }
}

/// One surface of a [GpuMesh]
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct Surface {
    /// Which vertex buffer to use
    pub idx_vertex_buffer: u32,
    /// Index index is zeroed to this value
    pub start_index: u32,
    /// Vertex index is zeroed to this value
    pub start_vertex: u32,
    pub num_indices: u16,
    pub idx_material: u16,
}

impl Surface {
    pub fn placeholder() -> Self {
        Self {
            idx_vertex_buffer: 0,
            start_index: 0,
            start_vertex: 0,
            num_indices: 0,
            idx_material: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_model_header_size() {
        assert_eq!(size_of::<ModelHeader>(), 0x14);
    }

    #[test]
    fn test_gpu_mesh_unk_a_size() {
        assert_eq!(size_of::<GpuMeshUnkA>(), 0x18);
    }

    #[test]
    fn test_object_model_size() {
        assert_eq!(size_of::<ObjectModel>(), 0x60);
    }

    #[test]
    fn test_model_unknown_a_size() {
        assert_eq!(size_of::<ModelUnknownA>(), 0x64);
    }

    #[test]
    fn test_model_unknown_b_size() {
        assert_eq!(size_of::<ModelUnknownB>(), 0x34);
    }

    #[test]
    fn test_mesh_header_size() {
        let mesh_buffer = MeshBuffer::new(MeshBufferType::Cpu);
        assert_eq!(mesh_buffer.to_bytes().len(), 0x14);
    }

    #[test]
    fn test_vertex_buf_header_size() {
        let vertex_buffer = VertexBuffer::new(2, 1);
        assert_eq!(vertex_buffer.to_bytes().len(), 0x10);
    }

    #[test]
    fn test_submesh_size() {
        let submesh = Submesh::new();
        assert_eq!(submesh.to_bytes().len(), 0x10);
    }

    #[test]
    fn test_gpu_surface_size() {
        assert_eq!(size_of::<Surface>(), 0x10);
    }
}
