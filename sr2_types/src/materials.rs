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

use std::io::{BufReader, Read, Seek};
use zerocopy::FromBytes;
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

/// Not a direct SR2 type - this is an instance type of which purpose is make
/// the data easier to work with. Corresponds to [MaterialData]
///
/// An instance of SR2 material.
#[derive(Debug, Clone)]
pub struct Material {
    pub shader_name_checksum: u32,
    pub mat_name_checksum: u32,
    pub flags: u32,
    pub unknown_2b: Vec<u16>,
    pub textures: Vec<MaterialTexEntry>,
    pub unk_0x10: i16,
    pub flags_0x12: i16,
    pub runtime_0x14: i32,
}

impl Material {
    /// Serialize instance to SR2 type
    pub fn material_data(&self) -> MaterialData {
        let mut data = MaterialData::new();

        data.shader_name_checksum = self.shader_name_checksum;
        data.mat_name_checksum = self.mat_name_checksum;
        data.flags = self.flags;
        data.num_unknown_2b = self.unknown_2b.len() as u16;
        data.num_textures = self.textures.len() as u16;
        data.unk_0x10 = self.unk_0x10;
        data.flags_0x12 = self.flags_0x12;
        data.runtime_0x14 = self.runtime_0x14;

        data
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MaterialHeader {
    /// Number of [MaterialData] immediately after this header.
    pub num_materials: u32,
    /// Runtime only? Always Zero.
    runtime_0x04: u32,
    /// Runtime only? Always Zero.
    runtime_0x08: u32,
    /// Runtime only? Always Zero.
    runtime_0x0c: u32,
    /// Shader constants are just standard floats
    pub num_shader_constants: u32,
    /// Runtime only? Always Zero.
    runtime_0x14: u32,
    /// Runtime only? Always Zero.
    runtime_0x18: u32,
    /// Unknown 16B struct
    pub num_mat_unknown3: u32,
    /// Runtime only? Always Zero.
    runtime_0x20: u32,
}

impl MaterialHeader {
    #[allow(clippy::new_without_default)]
    pub fn new() -> Self {
        Self {
            num_materials: 0,
            runtime_0x04: 0,
            runtime_0x08: 0,
            runtime_0x0c: 0,
            num_shader_constants: 0,
            runtime_0x14: 0,
            runtime_0x18: 0,
            num_mat_unknown3: 0,
            runtime_0x20: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let this = {
            let mut buf = vec![0_u8; size_of::<Self>()];
            reader.read_exact(&mut buf)?;
            Self::read_from_bytes(&buf).unwrap()
        };
        if this.runtime_0x04 != 0 {
            let pos = reader.stream_position().unwrap() - 0x20;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x08 != 0 {
            let pos = reader.stream_position().unwrap() - 0x1c;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x0c != 0 {
            let pos = reader.stream_position().unwrap() - 0x18;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x14 != 0 {
            let pos = reader.stream_position().unwrap() - 0x10;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x18 != 0 {
            let pos = reader.stream_position().unwrap() - 0xc;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if this.runtime_0x20 != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        Ok(this)
    }
}

/// Material from chunk files
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
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
    /// Runtime only? Always -1 OR 0
    pub runtime_0x14: i32,
}

impl MaterialData {
    #[allow(clippy::new_without_default)]
    pub fn new() -> Self {
        Self {
            shader_name_checksum: 0,
            mat_name_checksum: 0,
            flags: 0,
            num_unknown_2b: 0,
            num_textures: 0,
            unk_0x10: 0,
            flags_0x12: 0,
            runtime_0x14: -1,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let this = {
            let mut buf = vec![0_u8; size_of::<Self>()];
            reader.read_exact(&mut buf)?;
            Self::read_from_bytes(&buf).unwrap()
        };
        if this.runtime_0x14 != -1 && this.runtime_0x14 != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MaterialTexCont {
    /// 16 textures are always allocated, even if some or all are unused.
    pub textures: [MaterialTexEntry; 16],
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MaterialTexEntry {
    /// Texture index. -1 if entry is unused
    pub index: i16,
    /// Texture flags? -1 if entry is unused
    pub flags: i16,
}

impl MaterialTexEntry {
    pub fn placeholder() -> Self {
        Self {
            index: -1,
            flags: -1,
        }
    }
}

/// Unknown 16B struct
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
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
