// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use byteorder::{LittleEndian, ReadBytesExt};
use zerocopy::IntoBytes;

use crate::{Sr2TypeError, Vector};

#[derive(Debug, Clone)]
#[repr(C)]
pub struct Object {
    pub cull_box_min: Vector,
    pub cull_box_max: Vector,
    pub render_distance: f32,

    pub unk_0x24: u32,
    pub unk_0x2c: u16,
    pub flags_0x30: i32,
    pub flags_0x34: i32,
    /// Always zero, except in these 4 chunks:
    /// sr2_intaicutjyucar, sr2_intarcutlimo, sr2_intdkmissunkdk, sr2_skybox
    pub unk_cutsceneskybox_0x38: u32,
    pub unk_0x3c: u16,
    pub unk_0x3e: u16,
    pub idx_obj_model: u32,
    pub unk_0x44: u32,
    pub unk_0x48: i32,
    pub unk_0x4c: i32,

    pub name: String,
}

impl Object {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let cull_box_min = Vector::read(reader)?;
        let runtime_0x0c = reader.read_u32::<LittleEndian>()?;
        let cull_box_max = Vector::read(reader)?;
        let render_distance = reader.read_f32::<LittleEndian>()?;
        let runtime_0x20 = reader.read_u32::<LittleEndian>()?;
        let unk_0x24 = reader.read_u32::<LittleEndian>()?;
        let runtime_0x28 = reader.read_u32::<LittleEndian>()?;
        let unk_0x2c = reader.read_u16::<LittleEndian>()?;
        let pad_0x2e = reader.read_u16::<LittleEndian>()?;
        let flags_0x30 = reader.read_i32::<LittleEndian>()?;
        let flags_0x34 = reader.read_i32::<LittleEndian>()?;
        let unk_cutsceneskybox_0x38 = reader.read_u32::<LittleEndian>()?;
        let unk_0x3c = reader.read_u16::<LittleEndian>()?;
        let unk_0x3e = reader.read_u16::<LittleEndian>()?;
        let idx_obj_model = reader.read_u32::<LittleEndian>()?;
        let unk_0x44 = reader.read_u32::<LittleEndian>()?;
        let unk_0x48 = reader.read_i32::<LittleEndian>()?;
        let unk_0x4c = reader.read_i32::<LittleEndian>()?;

        if runtime_0x0c != 0 {
            let pos = reader.stream_position()? - 0x44;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x20 != 0 {
            let pos = reader.stream_position()? - 0x34;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x28 != 0 {
            let pos = reader.stream_position()? - 0x28;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if pad_0x2e != 0 {
            let pos = reader.stream_position()? - 0x22;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        let this = Self {
            cull_box_min,
            cull_box_max,
            render_distance,
            unk_0x24,
            unk_0x2c,
            flags_0x30,
            flags_0x34,
            unk_cutsceneskybox_0x38,
            unk_0x3c,
            unk_0x3e,
            idx_obj_model,
            unk_0x44,
            unk_0x48,
            unk_0x4c,

            name: "".into(),
        };

        Ok(this)
    }

    /// Serialize
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(self.cull_box_min.as_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes.extend_from_slice(self.cull_box_max.as_bytes());
        bytes.extend_from_slice(&self.render_distance.to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x24.to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x2c.to_le_bytes());
        bytes.extend_from_slice(&0_u16.to_le_bytes());
        bytes.extend_from_slice(&self.flags_0x30.to_le_bytes());
        bytes.extend_from_slice(&self.flags_0x34.to_le_bytes());
        bytes.extend_from_slice(&self.unk_cutsceneskybox_0x38.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x3c.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x3e.to_le_bytes());
        bytes.extend_from_slice(&self.idx_obj_model.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x44.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x48.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x4c.to_le_bytes());
        bytes
    }
}
