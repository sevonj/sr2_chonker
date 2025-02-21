// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, IntoBytes};

#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct Sr2GpuMeshUnk0 {
    pub unk_0x00: i32,
    pub unk_0x04: i32,
    pub unk_0x08: i32,
    pub unk_0x0c: i32,
    pub unk_0x10: i32,
    pub unk_0x14: i32,
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_sr2_gpu_mesh_unk0_size() {
        assert_eq!(size_of::<Sr2GpuMeshUnk0>(), 0x18);
    }
}
