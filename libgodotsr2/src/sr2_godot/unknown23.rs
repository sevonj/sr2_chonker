// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{Material, MeshInstance3D, QuadMesh};

use crate::editor::RenderLayer;

use super::sr2_vec_to_godot;

#[derive(Debug, GodotClass)]
#[class(no_init, base=Node3D)]
pub struct Unknown23 {
    marker: Gd<MeshInstance3D>,
    marker_mat: Gd<Material>,

    base: Base<Node3D>,
}

#[godot_api]
impl INode3D for Unknown23 {
    fn ready(&mut self) {
        self.setup_node();
    }
}

impl Unknown23 {
    pub fn from_sr2(data: &sr2::Unknown23) -> Gd<Self> {
        let mut this = Gd::from_init_fn(|base| Unknown23 {
            marker: MeshInstance3D::new_alloc(),
            marker_mat: load("res://assets/ui/gizmos/mat_gizmo_unk23.tres"),
            base,
        });
        this.set_position(sr2_vec_to_godot(&data.origin));
        this
    }

    fn setup_node(&mut self) {
        let mesh = QuadMesh::new_gd();

        let mut cull_bbox = self.marker.clone();
        cull_bbox.set_mesh(&mesh);
        cull_bbox.set_material_override(&self.marker_mat);
        cull_bbox.set_layer_mask(RenderLayer::Markers.mask());
        cull_bbox.set_name("gizmo");

        self.base_mut().add_child(&cull_bbox);
    }
}
