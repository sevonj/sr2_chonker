// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use super::Vector;

/// Transform matrix
#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
#[repr(C)]
pub struct Transform {
    pub basis_x: Vector,
    pub basis_y: Vector,
    pub basis_z: Vector,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_transform_size() {
        assert_eq!(size_of::<Transform>(), 0x24);
    }
}
