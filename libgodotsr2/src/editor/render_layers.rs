// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#[repr(i32)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum RenderLayer {
    Common = 1,
    Gizmos = 2,
    Collisions = 3,
    BBox = 4,
    Unknown = 5,
    Markers = 6,
}

impl RenderLayer {
    pub const DEFAULT_MASK: u32 = Self::Common as u32
        | Self::Gizmos.mask() as u32
        | Self::Collisions.mask() as u32
        | Self::BBox.mask() as u32
        // | Self::Unknown.mask() as u32
        | Self::Markers.mask() as u32;

    pub const fn mask(&self) -> u32 {
        let shift = *self as usize - 1;
        1 << shift
    }
}
