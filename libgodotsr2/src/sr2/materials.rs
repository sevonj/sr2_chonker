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

use zerocopy_derive::{FromBytes, IntoBytes};

/// Material from chunk files
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct MaterialData {
    /// A guess, not confirmed.
    /// Values see a lot of reuse between materials,
    /// so probably multiple mats using the same shader.
    pub shader_name_checksum: u32,
    /// A guess, not confirmed.
    /// Values seem unique to each mat, need to check if really so.
    pub mat_name_checksum: u32,
    /// Not confirmed, but looks like
    pub flags: u32,
    /// Number of entries belonging to this mat in the block immediately after
    /// materials.
    pub num_unknown_2b: u16,
    pub num_textures: u16,
    /// Always 0?
    pub unk_0x10: i16,
    /// Not confirmed, but looks like
    pub flags_0x12: i16,
    /// Runtime only? Always -1.
    pub runtime_0x14: i32,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_material_data_size() {
        assert_eq!(size_of::<MaterialData>(), 0x18);
    }
}
