// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, IntoBytes};

use super::Vector;

#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct ChunkHeader {
    pub magic: u32,
    pub version: u32,
    pub header0x08: i32,
    pub header0x0c: i32,
    pub header0x10: i32,

    /// Hash?
    pub header0x14: i32,
    /// Hash?
    pub header0x18: i32,
    /// Hash?
    pub header0x20: i32,
    /// Hash?
    pub header0x24: i32,
    /// Hash?
    pub header0x28: i32,
    /// Hash?
    pub header0x2c: i32,
    /// Hash?
    pub header0x30: i32,
    /// Hash?
    pub header0x34: i32,
    /// Hash?
    pub header0x38: i32,
    /// Hash?
    pub header0x3c: i32,
    /// Hash?
    pub header0x40: i32,
    /// Hash?
    pub header0x44: i32,
    /// Hash?
    pub header0x48: i32,
    /// Hash?
    pub header0x4c: i32,
    /// Hash?
    pub header0x50: i32,
    /// Hash?
    pub header0x54: i32,
    /// Hash?
    pub header0x58: i32,
    /// Hash?
    pub header0x5c: i32,
    /// Hash?
    pub header0x60: i32,
    /// Hash?
    pub header0x64: i32,
    /// Hash?
    pub header0x68: i32,
    /// Hash?
    pub header0x6c: i32,
    /// Hash?
    pub header0x70: i32,
    /// Hash?
    pub header0x74: i32,
    /// Hash?
    pub header0x78: i32,
    /// Hash?
    pub header0x7c: i32,
    /// Hash?
    pub header0x80: i32,
    /// Hash?
    pub header0x84: i32,
    /// Hash?
    pub header0x88: i32,
    /// Hash?
    pub header0x8c: i32,

    /// First half of GPU chunk. Contains models.
    pub len_gpu_chunk_a: i32,
    /// Second half of GPU chunk.
    /// Probably contains destroyable objects.
    pub len_gpu_chunk_b: i32,

    pub num_city_objects: i32,
    pub num_unknown23s: i32,
    /// num_something?
    pub header0x9c: i32,
    /// num_something?
    pub header0xa0: i32,
    /// num_something?
    pub header0xa4: i32,
    /// num_something?
    pub header0xa8: i32,
    /// num_something?
    pub header0xac: i32,
    /// num_something?
    pub header0xb4: i32,
    pub num_mesh_movers: i32,
    pub num_unknown27s: i32,
    pub num_unknown28s: i32,
    pub num_unknown29s: i32,
    pub num_unknown30s: i32,
    pub num_unknown31s: i32,
    /// num_something?
    pub header0xcc: i32,
    /// num_something?
    pub header0xd0: i32,

    /// Load zone???
    pub bbox_min: Vector,
    /// Load zone???
    pub bbox_max: Vector,

    pub header0xec: f32,
    pub header0xf0: i32,
    pub header0xf4: i32,
    pub header0xf8: i32,
    pub header0xfc: i32,
}

impl ChunkHeader {
    pub const MAGIC: u32 = 0xBBCACA12;
    pub const VERION: u32 = 121;
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_chunk_header_size() {
        assert_eq!(size_of::<ChunkHeader>(), 0x100);
    }
}
