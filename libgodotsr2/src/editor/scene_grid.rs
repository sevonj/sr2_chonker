// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{IMeshInstance3D, Material, MeshInstance3D, PlaneMesh};

/// Renders a grid at scene floor
#[derive(Debug, GodotClass)]
#[class(base=MeshInstance3D)]
pub struct SceneGrid {
    pub follow_target: Option<Gd<Node3D>>,

    base: Base<MeshInstance3D>,
}

#[godot_api]
impl IMeshInstance3D for SceneGrid {
    fn init(base: Base<Self::Base>) -> Self {
        Self {
            follow_target: None,

            base,
        }
    }

    fn ready(&mut self) {
        self.setup_mesh();
    }

    fn process(&mut self, _delta: f64) {
        let Some(follow_target) = &self.follow_target else {
            return;
        };

        let mut position = follow_target.get_global_position();
        position.y = 0.0;
        self.base_mut().set_global_position(position);
    }
}

impl SceneGrid {
    pub fn with_follow_target(follow_target: Gd<Node3D>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self {
            follow_target: Some(follow_target),

            base,
        })
    }

    fn setup_mesh(&mut self) {
        let material: Gd<Material> = load("res://assets/materials/mat_scene_grid.tres");

        let mut mesh = PlaneMesh::new_gd();
        mesh.set_size(Vector2::ONE * 1024.0);
        mesh.set_material(&material);

        self.base_mut().set_mesh(&mesh);
    }
}
