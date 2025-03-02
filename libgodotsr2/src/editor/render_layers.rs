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
    UnknownMisc = 5,
}

impl RenderLayer {
    pub fn mask(&self) -> u32 {
        let shift = *self as usize - 1;
        1 << shift
    }
}
