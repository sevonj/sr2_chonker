// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::builtin::{Basis, Vector3};

pub fn sr2_xform_to_godot(xform: &sr2::Transform) -> Basis {
    Basis {
        rows: [
            Vector3 {
                x: -xform.basis_x.x,
                y: xform.basis_x.y,
                z: xform.basis_x.z,
            },
            Vector3 {
                x: -xform.basis_y.x,
                y: xform.basis_y.y,
                z: xform.basis_y.z,
            },
            Vector3 {
                x: -xform.basis_z.x,
                y: xform.basis_z.y,
                z: xform.basis_z.z,
            },
        ],
    }
}

pub fn godot_xform_to_sr2(basis: &Basis) -> sr2::Transform {
    sr2::Transform {
        basis_x: sr2::Vector {
            x: -basis.rows[0].x,
            y: basis.rows[0].y,
            z: basis.rows[0].z,
        },
        basis_y: sr2::Vector {
            x: -basis.rows[1].x,
            y: basis.rows[1].y,
            z: basis.rows[1].z,
        },
        basis_z: sr2::Vector {
            x: -basis.rows[2].x,
            y: basis.rows[2].y,
            z: basis.rows[2].z,
        },
    }
}
