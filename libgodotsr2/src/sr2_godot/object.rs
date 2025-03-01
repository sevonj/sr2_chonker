// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::classes::mesh::PrimitiveType;
use godot::prelude::*;

use godot::classes::{ArrayMesh, Material, MeshInstance3D, SurfaceTool};

use super::sr2_aabb_to_godot;

/// Corresponds to [sr2::Object]
/// Name doesn't match, because "Object" is a thing in Godot already
#[derive(Debug, GodotClass)]
#[class(no_init, base=Node)]
pub struct CityObject {
    data: sr2::Object,

    cull_bbox: Gd<MeshInstance3D>,
    cull_bbox_mat: Gd<Material>,
    //cull_bbox_mat_selected: Gd<Material>,
    base: Base<Node>,
}

#[godot_api]
impl INode for CityObject {
    fn ready(&mut self) {
        self.setup_node();
    }
}

impl CityObject {
    pub fn from_sr2(data: sr2::Object) -> Gd<Self> {
        Gd::from_init_fn(|base| CityObject {
            data,
            cull_bbox: MeshInstance3D::new_alloc(),
            cull_bbox_mat: load("res://assets/materials/mat_gizmo_bbox.tres"),
            //cull_bbox_mat_selected: load("res://assets/materials/mat_gizmo_bbox.tres"),
            base,
        })
    }

    fn setup_node(&mut self) {
        let mut cull_bbox = self.cull_bbox.clone();
        let (cullbox_min, cullbox_max) =
            sr2_aabb_to_godot(&self.data.cull_box_min, &self.data.cull_box_max);
        let cullbox_size = cullbox_max - cullbox_min;
        let cullbox_center = cullbox_min + cullbox_size / 2.0;
        cull_bbox.set_position(cullbox_center);
        cull_bbox.set_mesh(&build_bbox_mesh(cullbox_min, cullbox_max));
        cull_bbox.set_material_override(&self.cull_bbox_mat);
        cull_bbox.set_name("cull_bbox");

        self.base_mut().add_child(&cull_bbox);
        let name = self.data.name.clone();
        self.base_mut().set_name(&name);
    }
}

fn build_bbox_mesh(min: Vector3, max: Vector3) -> Gd<ArrayMesh> {
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
