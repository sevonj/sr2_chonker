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

use crate::Sr2TypeError;

/// Only present in crib chunks
#[derive(Debug, Clone)]
pub struct CribSomething {
    pub unknown_0x00: u32,
    pub unk_a: Vec<CribSomethingSub>,
    pub unk_b: Vec<CribSomethingSub>,
    pub unk_c: Vec<CribSomethingSub>,
}

impl CribSomething {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let unknown_0x00 = reader.read_u32::<LittleEndian>()?;

        let num_unk_a = reader.read_u32::<LittleEndian>()?;
        let mut unk_a = vec![];
        for _ in 0..num_unk_a {
            unk_a.push(CribSomethingSub::read(reader)?);
        }

        let num_unk_b = reader.read_u32::<LittleEndian>()?;
        let mut unk_b = vec![];
        for _ in 0..num_unk_b {
            unk_b.push(CribSomethingSub::read(reader)?);
        }

        let num_unk_c = reader.read_u32::<LittleEndian>()?;
        let mut unk_c = vec![];
        for _ in 0..num_unk_c {
            unk_c.push(CribSomethingSub::read(reader)?);
        }

        Ok(Self {
            unknown_0x00,
            unk_a,
            unk_b,
            unk_c,
        })
    }

    /// Serialize the entire thing
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = vec![];
        bytes.extend_from_slice(&self.unknown_0x00.to_le_bytes());
        bytes.extend_from_slice(&(self.unk_a.len() as u32).to_le_bytes());
        for value in &self.unk_a {
            bytes.extend_from_slice(value.as_bytes());
        }
        bytes.extend_from_slice(&(self.unk_b.len() as u32).to_le_bytes());
        for value in &self.unk_b {
            bytes.extend_from_slice(value.as_bytes());
        }
        bytes.extend_from_slice(&(self.unk_c.len() as u32).to_le_bytes());
        for value in &self.unk_c {
            bytes.extend_from_slice(value.as_bytes());
        }
        bytes
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct CribSomethingSub {
    pub unknown_0x0: [u8; 12],
    pub unknown_0xc: u32,
}

impl CribSomethingSub {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}
