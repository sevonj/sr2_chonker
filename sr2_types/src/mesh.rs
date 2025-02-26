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
use zerocopy::FromBytes;
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

use super::{Transform, Vector};

/// Not a direct SR2 type - this is an instance type of which purpose is make
/// the data easier to work with. Corresponds to [MeshHeader].
///
/// Defines vertex/index buffers used in chunks.
/// Covers both CPU and GPU data.
#[derive(Debug, Clone)]
pub struct MeshBufferInstance {
    /// see [MeshHeader::mesh_type]
    pub mesh_type: u16,
    pub vertex_buffers: Vec<VertexBuffer>,
    pub indices: Vec<u16>,
}

impl MeshBufferInstance {
    /// Serialize instance to SR2 type
    pub fn header(&self) -> MeshHeader {
        let mut header = MeshHeader::new();

        header.mesh_type = self.mesh_type;
        header.num_indices = self.indices.len() as u32;
        header.num_vertex_buffers = self.vertex_buffers.len() as u16;

        header
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

/// Describes buffers both in cpu and gpu chunk file.
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MeshHeader {
    /// Either 7 or 0? 7 is cpu, 0 gpu
    pub mesh_type: u16,
    pub num_vertex_buffers: u16,
    pub num_indices: u32,
    /// Always -1, probably runtime-only.
    runtime_0x08: i32,
    /// Always -1, probably runtime-only.
    runtime_0x0c: i32,
    /// Always 0, probably runtime-only.
    runtime_0x10: u32,
}

impl MeshHeader {
    fn new() -> Self {
        Self {
            mesh_type: 0,
            num_vertex_buffers: 0,
            num_indices: 0,
            runtime_0x08: -1,
            runtime_0x0c: -1,
            runtime_0x10: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let this = {
            let mut buf = vec![0_u8; size_of::<Self>()];
            reader.read_exact(&mut buf)?;
            Self::read_from_bytes(&buf).unwrap()
        };
        if this.runtime_0x08 != -1 {
            let pos = reader.stream_position().unwrap() - 0xc;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x0c != -1 {
            let pos = reader.stream_position().unwrap() - 0x8;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x10 != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        Ok(this)
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
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct GpuMeshHeader {
    pub bbox_min: Vector,
    pub bbox_max: Vector,
    pub unk_0x18: u32,
    pub unk_0x1c: u32,
    pub mesh_a: u32,
    pub mesh_b: u32,
}

/// A mesh that gets its data from the GPU chunk
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct GpuMesh {
    pub unknown_0x00: u16,
    pub num_surfaces: u16,
    pub unknown_0x04: u32,
    pub unknown_0x08: u32,
    /// Always 0
    runtime_0x0c: u32,
}

impl GpuMesh {
    #[allow(clippy::new_without_default)]
    pub fn new() -> Self {
        Self {
            unknown_0x00: 0,
            num_surfaces: 0,
            unknown_0x04: 0,
            unknown_0x08: 0,
            runtime_0x0c: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let this = {
            let mut buf = vec![0_u8; size_of::<Self>()];
            reader.read_exact(&mut buf)?;
            Self::read_from_bytes(&buf).unwrap()
        };
        if this.runtime_0x0c != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        Ok(this)
    }
}

/// One surface of a [GpuMesh]
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct GpuSurface {
    /// Which vertex buffer to use
    pub idx_vertex_buffer: u32,
    /// Index index is zeroed to this value
    pub start_index: u32,
    /// Vertex index is zeroed to this value
    pub start_vertex: u32,
    pub num_indices: u16,
    pub idx_material: u16,
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
        assert_eq!(size_of::<MeshHeader>(), 0x14);
    }

    #[test]
    fn test_vertex_buf_header_size() {
let vertex_buffer = VertexBuffer::new(2, 1);
        assert_eq!(vertex_buffer.to_bytes().len(), 0x10);
    }

    #[test]
    fn test_gpu_mesh_size() {
        assert_eq!(size_of::<GpuMesh>(), 0x10);
    }

    #[test]
    fn test_gpu_surface_size() {
        assert_eq!(size_of::<GpuSurface>(), 0x10);
    }
}
