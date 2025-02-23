// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, IntoBytes};

use super::{Transform, Vector};

/// Contains transform and rendermodel id.
/// Every city object has one of these, but usually a few are left over.
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct CityObjectModel {
    pub origin: Vector,
    pub xform: Transform,

    /// Always null?
    pub unk_0x30: i32,
    /// Always null?
    pub unk_0x34: i32,
    /// Always null?
    pub unk_0x38: i32,
    /// Always null?
    pub unk_0x3c: i32,
    /// Always null?
    pub unk_0x40: i32,
    /// Always null?
    pub unk_0x44: i32,
    /// Always null?
    pub unk_0x48: i32,

    /// Always 1f?
    pub unk_0x4c: f32,

    /// Always null?
    pub unk_0x50: i32,

    pub unk_0x54: f32,
    pub idx_gpu_model: i32,
    /// flags?
    pub unk_0x5c: i32,
}

#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct CityObject {}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn test_city_object_model_size() {
        assert_eq!(size_of::<CityObjectModel>(), 0x60);
    }
}
