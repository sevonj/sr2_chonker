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

use zerocopy_derive::{FromBytes, IntoBytes};

use super::{Transform, Vector};

#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct ModelHeader {
    pub num_gpu_meshes: u32,
    pub num_obj_models: u32,
    pub num_cpu_meshes: u32,
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
}
