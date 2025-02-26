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
//! - [MaterialTexture] * num_materials * 16 // 16 are always allocated, even if not that many, or even any are used.
//! - [MaterialUnknown3] * num_mat_unknown3
//! - Buffer belonging to [MaterialUnknown3]. size = (mat_unknown3s[_index].unk2_count * 4) for each

use byteorder::{LittleEndian, ReadBytesExt};
use std::io::{BufReader, Read, Seek};
use zerocopy::FromBytes;
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

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

/// SR2 chunk material.
#[derive(Debug, Clone)]
pub struct Material {
    pub shader_name_checksum: u32,
    pub mat_name_checksum: u32,
    pub flags: u32,
    pub unknown2: Vec<MaterialUnknown2>,
    pub textures: Vec<MaterialTextureEntry>,
    pub unk_0x10: i16,
    pub flags_0x12: i16,
    pub runtime_0x14: i32,

    pub unknown_16b_struct: [u8; 16],
}

impl Material {
    pub fn blank() -> Self {
        Self {
            shader_name_checksum: 0,
            mat_name_checksum: 0,
            flags: 0,
            unknown2: vec![],
            textures: vec![],
            unk_0x10: 0,
            flags_0x12: 0,
            runtime_0x14: 0,
            unknown_16b_struct: [0_u8; 16],
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let shader_name_checksum = reader.read_u32::<LittleEndian>()?;
        let mat_name_checksum = reader.read_u32::<LittleEndian>()?;
        let flags = reader.read_u32::<LittleEndian>()?;
        let num_unknown_2b = reader.read_u16::<LittleEndian>()?;
        let num_textures = reader.read_u16::<LittleEndian>()?;
        let unk_0x10 = reader.read_i16::<LittleEndian>()?;
        let flags_0x12 = reader.read_i16::<LittleEndian>()?;
        let runtime_0x14 = reader.read_i32::<LittleEndian>()?;

        let this = Self {
            shader_name_checksum,
            mat_name_checksum,
            flags,
            unknown2: vec![MaterialUnknown2::placeholder(); num_unknown_2b as usize],
            textures: vec![MaterialTextureEntry::placeholder(); num_textures as usize],
            unk_0x10,
            flags_0x12,
            runtime_0x14,
            unknown_16b_struct: [0_u8; 16],
        };

        if runtime_0x14 != -1 && runtime_0x14 != 0 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(this)
    }

    /// Serialize
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.shader_name_checksum.to_le_bytes());
        bytes.extend_from_slice(&self.mat_name_checksum.to_le_bytes());
        bytes.extend_from_slice(&self.flags.to_le_bytes());
        bytes.extend_from_slice(&(self.unknown2.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&(self.textures.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x10.to_le_bytes());
        bytes.extend_from_slice(&self.flags_0x12.to_le_bytes());
        bytes.extend_from_slice(&self.runtime_0x14.to_le_bytes());
        bytes
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MaterialUnknown2 {
    pub unk_0x00: i16,
    pub unk_0x02: i16,
    pub unk_0x04: i16,
}

impl MaterialUnknown2 {
    pub fn placeholder() -> Self {
        Self {
            unk_0x00: 0,
            unk_0x02: 0,
            unk_0x04: 0,
        }
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct MaterialTextureEntry {
    /// Texture index. -1 if entry is unused
    pub index: i16,
    /// Texture flags? -1 if entry is unused
    pub flags: i16,
}

impl MaterialTextureEntry {
    pub fn placeholder() -> Self {
        Self {
            index: -1,
            flags: -1,
        }
    }
}

/// Unknown 16B struct
#[derive(Debug, Clone)]
#[repr(C)]
pub struct MaterialUnknown3 {
    pub unk_0x00: u32,
    pub unk_0x04: u32,
    pub unk_data: Vec<u32>,
    pub unk_0x06: u16,
}

impl MaterialUnknown3 {
    #[allow(clippy::new_without_default)]
    pub fn new() -> Self {
        Self {
            unk_0x00: 0,
            unk_0x04: 0,
            unk_data: vec![],
            unk_0x06: 0,
        }
    }

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let unk_0x00 = reader.read_u32::<LittleEndian>()?;
        let unk_0x04 = reader.read_u32::<LittleEndian>()?;
        let num_unk = reader.read_u16::<LittleEndian>()?;
        let unk_0x06 = reader.read_u16::<LittleEndian>()?;
        let runtime_0x08 = reader.read_i32::<LittleEndian>()?;

        let this = Self {
            unk_0x00,
            unk_0x04,
            unk_data: vec![0_u32; num_unk as usize],
            unk_0x06,
        };

        if runtime_0x08 != -1 {
            let pos = reader.stream_position().unwrap() - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(this)
    }

    /// Serialize
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.unk_0x00.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x04.to_le_bytes());
        bytes.extend_from_slice(&(self.unk_data.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x06.to_le_bytes());
        bytes.extend_from_slice(&(-1_i32).to_le_bytes());
        bytes
    }
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
        let mat = Material::blank();
        let bytes = mat.to_bytes();
        assert_eq!(bytes.len(), 0x18);
    }

    #[test]
    fn test_material_texture_entry_size() {
        assert_eq!(size_of::<MaterialTextureEntry>(), 0x04);
    }

    #[test]
    fn test_material_unknown3_size() {
        let unk3 = MaterialUnknown3::new();
        assert_eq!(unk3.to_bytes().len(), 0x10);
    }
}
