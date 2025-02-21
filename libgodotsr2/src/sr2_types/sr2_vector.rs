// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use zerocopy_derive::{FromBytes, IntoBytes};

/// A 3D vector, for coords and whatnot
#[derive(Debug, FromBytes, IntoBytes)]
#[repr(C)]
pub struct Sr2Vector {
    pub x: f32,
    pub y: f32,
    pub z: f32,
}
