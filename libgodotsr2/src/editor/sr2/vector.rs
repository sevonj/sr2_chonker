// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::builtin::Vector3;

use crate::sr2_types::Sr2Vector;

pub fn sr2_vec_to_godot(vector: Sr2Vector) -> Vector3 {
    Vector3 {
        x: -vector.x,
        y: vector.y,
        z: vector.z,
    }
}

pub fn godot_vec_to_sr2(vector: Vector3) -> Sr2Vector {
    Sr2Vector {
        x: -vector.x,
        y: vector.y,
        z: vector.z,
    }
}
