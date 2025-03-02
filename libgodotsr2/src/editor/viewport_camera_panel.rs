// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::classes::control::SizeFlags;
use godot::prelude::*;

use godot::classes::{Font, IPanelContainer, Label, PanelContainer, StyleBox, VBoxContainer};

use super::CameraRigOrbit;

/// Renders a grid at scene floor
#[derive(Debug, GodotClass)]
#[class(no_init ,base=PanelContainer)]
pub struct ViewportCameraPanel {
    camera: Gd<CameraRigOrbit>,

    lab_camerapos: Gd<Label>,

    base: Base<PanelContainer>,
}

#[godot_api]
impl IPanelContainer for ViewportCameraPanel {
    fn ready(&mut self) {
        self.setup_ui();
    }

    fn process(&mut self, _delta: f64) {
        self.lab_camerapos
            .set_text(format_vec3(self.camera.get_global_position()).as_str());
    }
}

impl ViewportCameraPanel {
    pub fn new(camera: Gd<CameraRigOrbit>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self {
            camera,

            lab_camerapos: Label::new_alloc(),

            base,
        })
    }

    fn setup_ui(&mut self) {
        let font_monospace: Gd<Font> = load("res://assets/ui/fonts/font_monospace.tres");

        let mut title = Label::new_alloc();
        title.add_theme_font_size_override("font_size", 14);
        title.set_text("View");
        title.set_name("title");

        let mut lab_camerapos_title = Label::new_alloc();
        lab_camerapos_title.add_theme_font_size_override("font_size", 14);
        lab_camerapos_title.set_text("Camera origin");
        lab_camerapos_title.set_name("lab_camerapos_title");

        let mut lab_camerapos = self.lab_camerapos.clone();
        lab_camerapos.add_theme_font_override("font", &font_monospace);
        lab_camerapos.add_theme_font_size_override("font_size", 14);
        lab_camerapos.set_name("lab_camerapos");

        let mut vbox = VBoxContainer::new_alloc();
        vbox.add_child(&title);
        vbox.add_child(&lab_camerapos_title);
        vbox.add_child(&lab_camerapos);
        vbox.set_name("vbox");

        let stylebox: Gd<StyleBox> = load("res://assets/ui/theme/stylebox_viewport_panel.tres");

        self.base_mut().add_child(&vbox);
        self.base_mut().set_v_size_flags(SizeFlags::SHRINK_BEGIN);
        self.base_mut()
            .add_theme_stylebox_override("panel", &stylebox);
        //self.base_mut().set_custom_minimum_size(Vector2::ONE * 64.0);
    }
}

fn format_vec3(vector: Vector3) -> String {
    format!("x: {}\ny: {}\nz: {}", vector.x, vector.y, vector.z)
}
