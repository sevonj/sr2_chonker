// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{Control, IPanelContainer, PanelContainer, ScrollContainer, StyleBox};

use super::UiBrowserTextures;
use crate::sr2_godot::Chunk;

/// The browser is the left side panel that lists chunk contents
#[derive(Debug, GodotClass)]
#[class(base=PanelContainer)]
pub struct UiBrowserPanel {
    edited_chunk: Option<Gd<Chunk>>,

    ui_scroll: Gd<ScrollContainer>,
    ui_content: Option<Gd<Control>>,

    base: Base<PanelContainer>,
}

#[godot_api]
impl IPanelContainer for UiBrowserPanel {
    fn init(base: Base<Self::Base>) -> Self {
        Self {
            edited_chunk: None,

            ui_scroll: ScrollContainer::new_alloc(),
            ui_content: None,

            base,
        }
    }

    fn ready(&mut self) {
        self.setup_ui();
    }
}

impl UiBrowserPanel {
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

    pub fn set_chunk(&mut self, chunk: Gd<Chunk>) {
        self.clear_chunk();

        let tex_browser = UiBrowserTextures::new(chunk.clone());
        self.set_ui(tex_browser.upcast());

        self.edited_chunk = Some(chunk);
    }

    pub fn clear_chunk(&mut self) {
        self.clear_ui();
        self.edited_chunk = None;
    }

    fn set_ui(&mut self, content: Gd<Control>) {
        self.clear_ui();

        self.ui_scroll.add_child(&content);
        self.ui_content = Some(content);
    }

    fn clear_ui(&mut self) {
        if let Some(old_content) = &mut self.ui_content {
            old_content.queue_free();
            self.ui_content = None;
        }
    }
}
