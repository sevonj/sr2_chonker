// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{Material, MeshInstance3D, QuadMesh};

use super::sr2_vec_to_godot;

#[derive(Debug, GodotClass)]
#[class(no_init, base=Node3D)]
pub struct MeshMoverPosition {
    cull_bbox: Gd<MeshInstance3D>,
    cull_bbox_mat: Gd<Material>,

    base: Base<Node3D>,
}

#[godot_api]
impl INode3D for MeshMoverPosition {
    fn ready(&mut self) {
        self.setup_node();
    }
}

impl MeshMoverPosition {
    pub fn from_sr2(position: &sr2::Vector) -> Gd<Self> {
        let mut this = Gd::from_init_fn(|base| MeshMoverPosition {
            cull_bbox: MeshInstance3D::new_alloc(),
            cull_bbox_mat: load("res://assets/ui/gizmos/mat_gizmo_keyframe.tres"),
            //cull_bbox_mat_selected: load("res://assets/materials/mat_gizmo_bbox.tres"),
            base,
        });
        this.set_position(sr2_vec_to_godot(position));
        this
    }

    fn setup_node(&mut self) {
        let mesh = QuadMesh::new_gd();

        let mut cull_bbox = self.cull_bbox.clone();

        cull_bbox.set_mesh(&mesh);
        cull_bbox.set_material_override(&self.cull_bbox_mat);
        cull_bbox.set_name("gizmo");

        self.base_mut().add_child(&cull_bbox);
    }
}
