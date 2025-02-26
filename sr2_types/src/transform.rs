// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

use super::Vector;

/// Transform matrix
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct Transform {
    pub basis_x: Vector,
    pub basis_y: Vector,
    pub basis_z: Vector,
}

impl Transform {
    /// Construct from stream.
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let basis_x = Vector::read(reader)?;
        let basis_y = Vector::read(reader)?;
        let basis_z = Vector::read(reader)?;

        Ok(Self {
            basis_x,
            basis_y,
            basis_z,
        })
    }
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_transform_size() {
        assert_eq!(size_of::<Transform>(), 0x24);
    }
}
