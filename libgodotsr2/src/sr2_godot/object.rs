// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{Material, MeshInstance3D};

use crate::editor::RenderLayer;

use super::aabb::build_bbox_mesh;
use super::{sr2_aabb_to_godot, Sr2MeshInstance};

/// Corresponds to [sr2::Object]
/// Name doesn't match, because "Object" is a thing in Godot already
#[derive(Debug, GodotClass)]
#[class(no_init, base=Node)]
pub struct Sr2Object {
    data: sr2::Object,

    mesh_inst: Gd<Sr2MeshInstance>,
    cull_bbox: Gd<MeshInstance3D>,
    cull_bbox_mat: Gd<Material>,
    //cull_bbox_mat_selected: Gd<Material>,
    base: Base<Node>,
}

#[godot_api]
impl INode for Sr2Object {
    fn ready(&mut self) {
        self.setup_node();
    }
}

impl Sr2Object {
    pub fn from_sr2(data: sr2::Object, mesh_insts: &[Gd<Sr2MeshInstance>]) -> Gd<Self> {
        let mesh_inst = mesh_insts[data.idx_mesh_inst as usize].clone();

        Gd::from_init_fn(|base| Sr2Object {
            data,
            mesh_inst,
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
        cull_bbox.set_mesh(&build_bbox_mesh(cullbox_min, cullbox_max));
        cull_bbox.set_material_override(&self.cull_bbox_mat);
        cull_bbox.set_layer_mask(RenderLayer::BBox.mask());
        cull_bbox.set_name("cull_bbox");
        self.base_mut().add_child(&cull_bbox);

        let mut mesh = self.mesh_inst.clone();
        mesh.set_name("mesh");
        self.base_mut().add_child(&mesh);

        let name = self.data.name.clone();
        self.base_mut().set_name(&name);
    }
}
