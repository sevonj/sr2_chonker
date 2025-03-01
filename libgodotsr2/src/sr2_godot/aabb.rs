// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::builtin::Vector3;

pub fn sr2_aabb_to_godot(min: &sr2::Vector, max: &sr2::Vector) -> (Vector3, Vector3) {
    let aabb_min = Vector3 {
        x: -max.x,
        y: min.y,
        z: min.z,
    };
    let aabb_max = Vector3 {
        x: -min.x,
        y: max.y,
        z: max.z,
    };

    (aabb_min, aabb_max)
}

pub fn godot_aabb_to_sr2(min: &sr2::Vector, max: &sr2::Vector) -> (sr2::Vector, sr2::Vector) {
    let aabb_min = sr2::Vector {
        x: -max.x,
        y: min.y,
        z: min.z,
    };
    let aabb_max = sr2::Vector {
        x: -min.x,
        y: max.y,
        z: max.z,
    };

    (aabb_min, aabb_max)
}
