// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{control::MouseFilter, Control, HBoxContainer, IHBoxContainer};

/// Renders a grid at scene floor
#[derive(Debug, GodotClass)]
#[class(base=HBoxContainer)]
pub struct ViewportUiRoot {
    base: Base<HBoxContainer>,
}

#[godot_api]
impl IHBoxContainer for ViewportUiRoot {
    fn init(base: Base<Self::Base>) -> Self {
        Self { base }
    }

    fn ready(&mut self) {
        self.setup_ui();
    }

    fn process(&mut self, _delta: f64) {
        //
    }
}

impl ViewportUiRoot {
    fn setup_ui(&mut self) {
        self.base_mut().set_mouse_filter(MouseFilter::IGNORE);

        //
    }

    pub fn add_ui_panel(&mut self, panel: &Gd<Control>) {
        self.base_mut().add_child(panel);
    }
}
