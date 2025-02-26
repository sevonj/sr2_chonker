// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use byteorder::{LittleEndian, ReadBytesExt};
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

/// A 3D vector, for coords and whatnot
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct Vector {
    pub x: f32,
    pub y: f32,
    pub z: f32,
}

impl Vector {
    /// Construct from stream.
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let x = reader.read_f32::<LittleEndian>()?;
        let y = reader.read_f32::<LittleEndian>()?;
        let z = reader.read_f32::<LittleEndian>()?;

        Ok(Self { x, y, z })
    }
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_vector_size() {
        assert_eq!(size_of::<Vector>(), 0x0c);
    }
}
