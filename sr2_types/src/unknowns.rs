// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use byteorder::{LittleEndian, ReadBytesExt};
use zerocopy::{FromBytes, IntoBytes};
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::{Sr2TypeError, Vector};

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown19 {
    pub data_todo: [u8; 28],
    pub other_data: Unknown19Sub,
}

impl Unknown19 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown19Sub {
    pub data_todo: [f32; 7],
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown20 {
    pub data_todo: [u8; 12],
}

impl Unknown20 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown21 {
    pub unknown_0x0: u32,
    pub unknown_0x4: u32,
}

impl Unknown21 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown23 {
    pub origin: Vector,

    pub unknown_0x0c: f32,
    pub unknown_0x10: f32,
    pub unknown_0x14: f32,

    pub unknown_0x18: f32,
    pub unknown_0x1c: f32,
    pub unknown_0x20: f32,

    pub unknown_0x24: f32,
    pub unknown_0x28: f32,
    pub unknown_0x2c: f32,
}

impl Unknown23 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, Clone)]
pub struct Unknown24 {
    pub data_a: Vec<Unknown24A>,
    pub data_b: Vec<u16>,
}

impl Unknown24 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let num_a = reader.read_u32::<LittleEndian>()?;
        let runtime_0x04 = reader.read_u32::<LittleEndian>()?;
        let num_b = reader.read_u32::<LittleEndian>()?;
        let runtime_0x0c = reader.read_u32::<LittleEndian>()?;

        if runtime_0x04 != 0 {
            let pos = reader.stream_position()? - 0xc;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }
        if runtime_0x0c != 0 {
            let pos = reader.stream_position()? - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        let mut data_a = vec![];
        for _ in 0..num_a {
            data_a.push(Unknown24A::read(reader)?);
        }
        let mut data_b = vec![];
        for _ in 0..num_b {
            data_b.push(reader.read_u16::<LittleEndian>()?);
        }

        Ok(Self { data_a, data_b })
    }

    /// Serialize the entire thing
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&(self.data_a.len() as u32).to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());
        bytes.extend_from_slice(&(self.data_b.len() as u32).to_le_bytes());
        bytes.extend_from_slice(&0_u32.to_le_bytes());

        for value in &self.data_a {
            bytes.extend_from_slice(value.as_bytes());
        }
        for value in &self.data_b {
            bytes.extend_from_slice(&value.to_le_bytes());
        }

        bytes
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown24A {
    pub todo_data: [u8; 48],
}

impl Unknown24A {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}
