// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::builtin::Vector3;

use crate::sr2_types::Sr2Vector;

pub fn sr2_aabb_to_godot(min: Sr2Vector, max: Sr2Vector) -> (Vector3, Vector3) {
    let min = Vector3 {
        x: -max.x,
        y: min.y,
        z: min.z,
    };
    let max = Vector3 {
        x: -min.x,
        y: max.y,
        z: max.z,
    };

    (min, max)
}

pub fn godot_aabb_to_sr2(min: Sr2Vector, max: Sr2Vector) -> (Sr2Vector, Sr2Vector) {
    let min = Sr2Vector {
        x: -max.x,
        y: min.y,
        z: min.z,
    };
    let max = Sr2Vector {
        x: -min.x,
        y: max.y,
        z: max.z,
    };

    (min, max)
}
