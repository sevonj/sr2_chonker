// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::classes::control::SizeFlags;
use godot::prelude::*;

use godot::classes::{
    CheckBox, Font, IPanelContainer, Label, PanelContainer, StyleBox, VBoxContainer,
};

use super::{CameraRigOrbit, RenderLayer};

/// Renders a grid at scene floor
#[derive(Debug, GodotClass)]
#[class(no_init ,base=PanelContainer)]
pub struct ViewportVisibilityPanel {
    pub camera: Gd<CameraRigOrbit>,

    check_grid: Gd<CheckBox>,
    check_common: Gd<CheckBox>,
    check_collision: Gd<CheckBox>,
    check_bbox: Gd<CheckBox>,
    check_unk_misc: Gd<CheckBox>,

    base: Base<PanelContainer>,
}

#[godot_api]
impl IPanelContainer for ViewportVisibilityPanel {
    fn ready(&mut self) {
        self.setup_ui();
    }

    fn process(&mut self, _delta: f64) {
        let cam = &self.camera.bind().camera;
        self.check_grid
            .set_pressed_no_signal(cam.get_cull_mask_value(RenderLayer::Gizmos as i32));
        self.check_common
            .set_pressed_no_signal(cam.get_cull_mask_value(RenderLayer::Common as i32));
        self.check_collision
            .set_pressed_no_signal(cam.get_cull_mask_value(RenderLayer::Collisions as i32));
        self.check_bbox
            .set_pressed_no_signal(cam.get_cull_mask_value(RenderLayer::BBox as i32));
        self.check_unk_misc
            .set_pressed_no_signal(cam.get_cull_mask_value(RenderLayer::UnknownMisc as i32));
    }
}

#[godot_api]
impl ViewportVisibilityPanel {
    #[func]
    fn set_layer(&mut self, value: bool, layer_number: u32) {
        self.camera
            .bind_mut()
            .camera
            .set_cull_mask_value(layer_number as i32, value);
    }
}

impl ViewportVisibilityPanel {
    pub fn new(camera: Gd<CameraRigOrbit>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self {
            camera,
            check_grid: CheckBox::new_alloc(),
            check_common: CheckBox::new_alloc(),
            check_collision: CheckBox::new_alloc(),
            check_bbox: CheckBox::new_alloc(),
            check_unk_misc: CheckBox::new_alloc(),
            base,
        })
    }

    fn setup_ui(&mut self) {
        let font_monospace: Gd<Font> = load("res://assets/ui/fonts/font_monospace.tres");

        let mut title = Label::new_alloc();
        title.add_theme_font_size_override("font_size", 14);
        title.set_text("Visibility");
        title.set_name("title");

        let mut check_grid = self.check_grid.clone();
        check_grid.set_text("Grid");
        check_grid.add_theme_font_size_override("font_size", 14);
        check_grid.add_theme_font_override("font", &font_monospace);
        check_grid.connect(
            "toggled",
            &Callable::from_object_method(&self.to_gd(), "set_layer")
                .bind(&[Variant::from(RenderLayer::Gizmos as u32)]),
        );

        let mut check_common = self.check_common.clone();
        check_common.set_text("Geometry");
        check_common.add_theme_font_size_override("font_size", 14);
        check_common.add_theme_font_override("font", &font_monospace);
        check_common.connect(
            "toggled",
            &Callable::from_object_method(&self.to_gd(), "set_layer")
                .bind(&[Variant::from(RenderLayer::Common as u32)]),
        );

        let mut check_collision = self.check_collision.clone();
        check_collision.set_text("Collisions");
        check_collision.add_theme_font_size_override("font_size", 14);
        check_collision.add_theme_font_override("font", &font_monospace);
        check_collision.connect(
            "toggled",
            &Callable::from_object_method(&self.to_gd(), "set_layer")
                .bind(&[Variant::from(RenderLayer::Collisions as u32)]),
        );

        let mut check_bbox = self.check_bbox.clone();
        check_bbox.set_text("Bounding boxes");
        check_bbox.add_theme_font_size_override("font_size", 14);
        check_bbox.add_theme_font_override("font", &font_monospace);
        check_bbox.connect(
            "toggled",
            &Callable::from_object_method(&self.to_gd(), "set_layer")
                .bind(&[Variant::from(RenderLayer::BBox as u32)]),
        );

        let mut check_unk_misc = self.check_unk_misc.clone();
        check_unk_misc.set_text("Unknown & misc");
        check_unk_misc.add_theme_font_size_override("font_size", 14);
        check_unk_misc.add_theme_font_override("font", &font_monospace);
        check_unk_misc.connect(
            "toggled",
            &Callable::from_object_method(&self.to_gd(), "set_layer")
                .bind(&[Variant::from(RenderLayer::UnknownMisc as u32)]),
        );

        let mut vbox = VBoxContainer::new_alloc();
        vbox.add_child(&title);
        vbox.add_child(&check_grid);
        vbox.add_child(&check_common);
        vbox.add_child(&check_collision);
        vbox.add_child(&check_bbox);
        vbox.add_child(&check_unk_misc);
        vbox.set_name("vbox");

        let stylebox: Gd<StyleBox> = load("res://assets/ui/theme/stylebox_viewport_panel.tres");

        self.base_mut().add_child(&vbox);
        self.base_mut().set_v_size_flags(SizeFlags::SHRINK_BEGIN);
        self.base_mut()
            .add_theme_stylebox_override("panel", &stylebox);
    }
}
