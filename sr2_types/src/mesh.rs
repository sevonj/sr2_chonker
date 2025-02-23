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

use zerocopy_derive::{FromBytes, IntoBytes};

use super::{Transform, Vector};

/// An instance of SR2 mesh buffer used in chunks.
/// Covers both CPU and GPU data.
/// Corresponds to [MeshHeader]
pub struct MeshBufferInstance {
    /// see [MeshHeader::mesh_type]
    pub mesh_type: u16,
    pub vertex_buffers: Vec<VertexBufferInstance>,
    pub indices: Vec<u16>,
}

/// An instance of SR2 vertex buffer used in chunks.
/// Covers both CPU and GPU data.
/// Corresponds to [VertexBufHeader]
pub struct VertexBufferInstance {
    pub num_vertex_a: u8,
    pub num_uvs: u8,
    pub data: Vec<u8>,
}

#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct ModelHeader {
    pub num_gpu_meshes: u32,
    pub num_obj_models: u32,
    pub num_meshes: u32,
    pub num_model_unknown_a: u32,
    pub num_model_unknown_b: u32,
}

/// Always same count as GPU meshes. Purpose unknown.
#[derive(Debug, FromBytes, IntoBytes)]
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
#[derive(Debug, FromBytes, IntoBytes)]
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
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct ModelUnknownA {
    pub lotsa_floats: [f32; 0x19],
}

/// Probably part of objects or their models somehow
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct ModelUnknownB {
    pub lotsa_floats: [f32; 0xd],
}

/// Describes buffers both in cpu and gpu chunk file.
#[derive(Debug, FromBytes, IntoBytes)]
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

impl Default for MeshHeader {
    fn default() -> Self {
        Self {
            mesh_type: 0,
            num_vertex_buffers: 0,
            num_indices: 0,
            runtime_0x08: -1,
            runtime_0x0c: -1,
            runtime_0x10: 0,
        }
    }
}

impl MeshHeader {
    pub fn is_valid(&self) -> bool {
        (self.mesh_type == 0 || self.mesh_type == 7)
            && self.runtime_0x08 == -1
            && self.runtime_0x0c == -1
            && self.runtime_0x10 == 0
    }
}

/// Describes vertex format and count
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct VertexBufHeader {
    /// Number of unknown data in vertex. Each adds 2B to vert size.
    pub num_vertex_a: u8,
    /// Number of uv sets. Each adds 4B to vert size.
    pub num_uvs: u8,
    /// Size of one vert. Value is size of [Vector] + the two above fields:
    /// 12B + 4B * num_vertex_a + 4B * num_uvs
    pub len_vertex: u16,
    pub num_vertices: u32,
    /// Always -1, probably runtime-only.
    runtime_0x08: i32,
    /// Always 0, probably runtime-only.
    runtime_0x0c: u32,
}

impl Default for VertexBufHeader {
    fn default() -> Self {
        Self {
            num_vertex_a: 0,
            num_uvs: 0,
            len_vertex: 12,
            num_vertices: 0,
            runtime_0x08: -1,
            runtime_0x0c: 0,
        }
    }
}

impl VertexBufHeader {
    pub fn is_valid(&self) -> bool {
        self.runtime_0x08 == -1 && self.runtime_0x0c == 0
    }
}

/// A mesh that gets its data from the GPU chunk
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct GpuMesh {
    pub unknown_0x00: u16,
    pub num_surfaces: u16,
    pub unknown_0x04: u32,
    pub unknown_0x08: u32,
    /// Always 0
    runtime_0x0c: u32,
}

#[allow(clippy::derivable_impls)]
impl Default for GpuMesh {
    fn default() -> Self {
        Self {
            unknown_0x00: 0,
            num_surfaces: 0,
            unknown_0x04: 0,
            unknown_0x08: 0,
            runtime_0x0c: 0,
        }
    }
}

/// One surface of a [GpuMesh]
#[derive(Debug, FromBytes, IntoBytes)]
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
        assert_eq!(size_of::<VertexBufHeader>(), 0x10);
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
