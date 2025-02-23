// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! SR2 Materials and related types.
//! It at least resembles materials from later games which are documented here:
//! <https://www.saintsrowmods.com/forum/threads/crunched-mesh-formats.15962/>
//!
//!
//! Layout:
//! - [MaterialHeader]
//! - [MaterialData]
//! - unknown_2B buffer
//! - unknown 16B struct * num_materials
//! - Align(16)
//! - [f32] * num_shader_constants
//! - [MaterialTextureCont] * num_materials
//! - [MaterialUnknown3] * num_mat_unknown3
//! - Buffer belonging to [MaterialUnknown3]. size = (mat_unknown3s[_index].unk2_count * 4) for each

use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

#[derive(Debug, FromBytes, IntoBytes, Immutable)]
#[repr(C)]
pub struct MaterialHeader {
    /// Number of [MaterialData] immediately after this header.
    pub num_materials: u32,
    /// Runtime only? Always Zero.
    pub runtime_0x04: u32,
    /// Runtime only? Always Zero.
    pub runtime_0x08: u32,
    /// Runtime only? Always Zero.
    pub runtime_0x0c: u32,
    /// Shader constants are just standard floats
    pub num_shader_constants: u32,
    /// Runtime only? Always Zero.
    pub runtime_0x14: u32,
    /// Runtime only? Always Zero.
    pub runtime_0x18: u32,
    /// Unknown 16B struct
    pub num_mat_unknown3: u32,
    /// Runtime only? Always Zero maybe?
    pub runtime_0x20: u32,
}

/// Material from chunk files
#[derive(Debug, FromBytes, IntoBytes, Immutable)]
#[repr(C)]
pub struct MaterialData {
    /// A guess, not confirmed.
    /// Values see a lot of reuse between materials,
    /// so probably multiple mats using the same shader.
    pub shader_name_checksum: u32,
    /// A guess, not confirmed.
    /// Values seem unique to each mat, need to check if really so.
    pub mat_name_checksum: u32,
    /// Not confirmed, but looks like
    pub flags: u32,
    /// Number of entries belonging to this mat in the block immediately after
    /// materials.
    pub num_unknown_2b: u16,
    pub num_textures: u16,
    /// Always 0?
    pub unk_0x10: i16,
    /// Not confirmed, but looks like
    pub flags_0x12: i16,
    /// Runtime only? Always -1.
    pub runtime_0x14: i32,
}

#[derive(Debug, FromBytes, IntoBytes, Immutable)]
#[repr(C)]
pub struct MaterialTexCont {
    /// 16 textures are always allocated, even if some or all are unused.
    pub textures: [MaterialTexEntry; 16],
}

#[derive(Debug, FromBytes, IntoBytes, Immutable)]
#[repr(C)]
pub struct MaterialTexEntry {
    /// Texture index. -1 if entry is unused
    pub index: i16,
    /// Texture flags? -1 if entry is unused
    pub flags: i16,
}

/// Unknown 16B struct
#[derive(Debug, FromBytes, IntoBytes, Immutable)]
#[repr(C)]
pub struct MaterialUnknown3 {
    pub unk_0x00: u32,
    pub unk_0x04: u32,
    /// Number of entries in following buffer
    pub num_unk: u16,
    pub unk_0x06: u16,
    /// Always -1. Runtime pointer?
    pub runtime_0x08: u32,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_material_header_size() {
        assert_eq!(size_of::<MaterialHeader>(), 0x24);
    }

    #[test]
    fn test_material_data_size() {
        assert_eq!(size_of::<MaterialData>(), 0x18);
    }

    #[test]
    fn test_material_texture_cont_size() {
        assert_eq!(size_of::<MaterialTexCont>(), 0x40);
    }

    #[test]
    fn test_material_texture_entry_size() {
        assert_eq!(size_of::<MaterialTexEntry>(), 0x04);
    }

    #[test]
    fn test_material_unknown3_size() {
        assert_eq!(size_of::<MaterialUnknown3>(), 0x10);
    }
}
