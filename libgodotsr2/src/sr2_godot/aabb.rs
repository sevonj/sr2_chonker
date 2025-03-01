// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{mesh::PrimitiveType, ArrayMesh, SurfaceTool};

pub fn sr2_aabb_to_godot(min: &sr2::Vector, max: &sr2::Vector) -> (Vector3, Vector3) {
    let aabb_min = Vector3 {
        x: -max.x / 2.0,
        y: min.y / 2.0,
        z: min.z / 2.0,
    };
    let aabb_max = Vector3 {
        x: -min.x / 2.0,
        y: max.y / 2.0,
        z: max.z / 2.0,
    };

    (aabb_min, aabb_max)
}

pub fn godot_aabb_to_sr2(min: &sr2::Vector, max: &sr2::Vector) -> (sr2::Vector, sr2::Vector) {
    let aabb_min = sr2::Vector {
        x: -max.x * 2.0,
        y: min.y * 2.0,
        z: min.z * 2.0,
    };
    let aabb_max = sr2::Vector {
        x: -min.x * 2.0,
        y: max.y * 2.0,
        z: max.z * 2.0,
    };

    (aabb_min, aabb_max)
}

pub fn build_bbox_mesh(min: Vector3, max: Vector3) -> Gd<ArrayMesh> {
    let mut st = SurfaceTool::new_gd();

    st.begin(PrimitiveType::LINES);

    let a = min;
    let b = Vector3::new(min.x, min.y, max.z);
    let c = Vector3::new(min.x, max.y, max.z);
    let d = Vector3::new(min.x, max.y, min.z);
    let e = Vector3::new(max.x, min.y, min.z);
    let f = Vector3::new(max.x, min.y, max.z);
    let g = max;
    let h = Vector3::new(max.x, max.y, min.z);

    st.add_vertex(a);
    st.add_vertex(b);
    st.add_vertex(b);
    st.add_vertex(c);
    st.add_vertex(c);
    st.add_vertex(d);
    st.add_vertex(d);
    st.add_vertex(a);

    st.add_vertex(e);
    st.add_vertex(f);
    st.add_vertex(f);
    st.add_vertex(g);
    st.add_vertex(g);
    st.add_vertex(h);
    st.add_vertex(h);
    st.add_vertex(e);

    st.add_vertex(a);
    st.add_vertex(e);
    st.add_vertex(b);
    st.add_vertex(f);
    st.add_vertex(c);
    st.add_vertex(g);
    st.add_vertex(d);
    st.add_vertex(h);

    st.commit().unwrap()
}
