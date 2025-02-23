// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{
    mesh::PrimitiveType, ArrayMesh, IMeshInstance3D, Material, MeshInstance3D, SurfaceTool,
};

use crate::sr2;

use super::sr2_vec_to_godot;

/// Potentially world collisions
#[derive(Debug, GodotClass)]
#[class(no_init, base=MeshInstance3D)]
pub struct MaybeStaticCollision {
    base: Base<MeshInstance3D>,
}

#[godot_api]
impl IMeshInstance3D for MaybeStaticCollision {}

impl MaybeStaticCollision {
    pub fn from_sr2(vertex_buffer: &[sr2::Vector]) -> Gd<Self> {
        let vertex_buffer: Vec<Vector3> = vertex_buffer.iter().map(sr2_vec_to_godot).collect();

        let mut this = Gd::from_init_fn(|base| Self { base });

        let mesh = this.bind().build_mesh(&vertex_buffer);
        this.set_mesh(&mesh);

        this
    }

    fn build_mesh(&self, vertex_buffer: &[Vector3]) -> Gd<ArrayMesh> {
        let mut st = SurfaceTool::new_gd();
        st.begin(PrimitiveType::POINTS);

        for vertex in vertex_buffer {
            st.add_vertex(*vertex);
        }

        let material: Gd<Material> = load("res://assets/materials/mat_wireframe_vcol.tres");
        st.set_material(&material);

        st.commit().unwrap()
    }
}
