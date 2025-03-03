// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{Control, IPanelContainer, PanelContainer, ScrollContainer, StyleBox};

/// The right sidebar
#[derive(Debug, GodotClass)]
#[class(base=PanelContainer)]
pub struct UiInspector {
    ui_scroll: Gd<ScrollContainer>,
    ui_content: Option<Gd<Control>>,

    base: Base<PanelContainer>,
}

#[godot_api]
impl IPanelContainer for UiInspector {
    fn init(base: Base<Self::Base>) -> Self {
        Self {
            ui_scroll: ScrollContainer::new_alloc(),
            ui_content: None,

            base,
        }
    }

    fn ready(&mut self) {
        self.setup_ui();
    }

    fn process(&mut self, _delta: f64) {
        let visible = self.ui_content.is_some();
        self.base_mut().set_visible(visible);
    }
}

impl UiInspector {
    fn setup_ui(&mut self) {
        let mut ui_scroll = self.ui_scroll.clone();
        ui_scroll.set_name("ui_scroll");

        let stylebox: Gd<StyleBox> = load("res://assets/ui/theme/stylebox_browser.tres");

        self.base_mut().add_child(&ui_scroll);
        self.base_mut()
            .set_custom_minimum_size(Vector2::ONE * 256.0);
        self.base_mut()
            .add_theme_stylebox_override("panel", &stylebox);
    }

    pub fn set_ui<T: Inherits<Control>>(&mut self, content: Gd<T>) {
        let content = content.upcast();
        self.clear_ui();

        self.ui_scroll.add_child(&content);
        self.ui_content = Some(content);
    }

    pub fn clear_ui(&mut self) {
        if let Some(old_content) = &mut self.ui_content {
            old_content.queue_free();
            self.ui_content = None;
        }
    }
}
