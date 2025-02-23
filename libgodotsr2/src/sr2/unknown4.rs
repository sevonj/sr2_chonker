// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, IntoBytes};

/// Probably part of objects or their models somehow
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct Unknown4 {
    pub lotsa_floats: [f32; 0xd],
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_unknown4_size() {
        assert_eq!(size_of::<Unknown4>(), 0x34);
    }
}
