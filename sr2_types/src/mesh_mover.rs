// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use byteorder::{LittleEndian, ReadBytesExt};
use zerocopy::FromBytes;
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

#[derive(Debug, Clone)]
pub struct MeshMover {
    pub name: String,

    pub unk_0x00: u32,
    pub unk_0x04: u32,
    pub unk_0x08: i16,
    pub unk_0x0a: u16,
    pub starts: Vec<String>,
    pub unk_0x10: u32,
    pub unk_0x14: u32,
    pub unk_0x18: u32,
}
impl MeshMover {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let unk_0x00 = reader.read_u32::<LittleEndian>()?;
        let unk_0x04 = reader.read_u32::<LittleEndian>()?;
        let unk_0x08 = reader.read_i16::<LittleEndian>()?;
        let unk_0x0a = reader.read_u16::<LittleEndian>()?;
        let pad_maybe_0x0c = reader.read_u16::<LittleEndian>()?;
        let num_starts = reader.read_u16::<LittleEndian>()?;
        let unk_0x10 = reader.read_u32::<LittleEndian>()?;
        let unk_0x14 = reader.read_u32::<LittleEndian>()?;
        let unk_0x18 = reader.read_u32::<LittleEndian>()?;

        if pad_maybe_0x0c != 0 {
            let pos = reader.stream_position()? - 0x10;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        let starts = vec![String::new(); num_starts as usize];

        Ok(Self {
            name: "".into(),
            unk_0x00,
            unk_0x04,
            unk_0x08,
            unk_0x0a,
            starts,
            unk_0x10,
            unk_0x14,
            unk_0x18,
        })
    }

    /// Serialize header
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.unk_0x00.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x04.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x08.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x0a.to_le_bytes());
        bytes.extend_from_slice(&0_u16.to_le_bytes());
        bytes.extend_from_slice(&(self.starts.len() as u16).to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x10.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x14.to_le_bytes());
        bytes.extend_from_slice(&self.unk_0x18.to_le_bytes());

        bytes
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown27 {
    pub todo_data: [u8; 24],
}

impl Unknown27 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown28 {
    pub todo_data: [u8; 36],
}

impl Unknown28 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown31 {
    pub unk_0x00: u32,
    pub unk_0x04: u32,
}

impl Unknown31 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown32 {
    pub unk_0x00: f32,
    pub unk_0x04: f32,
}

impl Unknown32 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}
