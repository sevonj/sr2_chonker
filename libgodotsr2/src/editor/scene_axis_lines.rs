// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{mesh::PrimitiveType, IMeshInstance3D, Material, MeshInstance3D, SurfaceTool};

use super::RenderLayer;

const LINE_LEN: f32 = 2048.0;

/// Renders axis lines at scene origin
#[derive(Debug, GodotClass)]
#[class(base=MeshInstance3D)]
pub struct SceneAxisLines {
    base: Base<MeshInstance3D>,
}

#[godot_api]
impl IMeshInstance3D for SceneAxisLines {
    fn init(base: Base<Self::Base>) -> Self {
        Self { base }
    }

    fn ready(&mut self) {
        self.base_mut().set_layer_mask(RenderLayer::Gizmos.mask());
        self.setup_mesh();
    }
}

impl SceneAxisLines {
    fn setup_mesh(&mut self) {
        let material: Gd<Material> = load("res://assets/materials/mat_scene_axis.tres");

        let col_x = Color::RED;
        let col_y = Color::LAWN_GREEN;
        let col_z = Color::DODGER_BLUE;

        let mut st = SurfaceTool::new_gd();
        st.begin(PrimitiveType::LINES);
        st.set_color(col_x);
        st.add_vertex(Vector3::LEFT * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.add_vertex(Vector3::RIGHT * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.set_color(col_y);
        st.add_vertex(Vector3::UP * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.add_vertex(Vector3::DOWN * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.set_color(col_z);
        st.add_vertex(Vector3::FORWARD * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.add_vertex(Vector3::BACK * LINE_LEN);
        st.add_vertex(Vector3::ZERO);
        st.set_material(&material);
        let mesh = st.commit().unwrap();

        self.base_mut().set_mesh(&mesh);
    }
}
