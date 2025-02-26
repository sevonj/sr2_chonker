// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{IVBoxContainer, Label, VBoxContainer};

use crate::sr2_godot::Chunk;

#[derive(Debug, GodotClass)]
#[class(no_init, base=VBoxContainer)]
pub struct UiBrowserTextures {
    chunk: Gd<Chunk>,

    base: Base<VBoxContainer>,
}

#[godot_api]
impl IVBoxContainer for UiBrowserTextures {
    fn ready(&mut self) {
        self.setup_ui();
    }
}

impl UiBrowserTextures {
    pub fn new(chunk: Gd<Chunk>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self { chunk, base })
    }

    fn setup_ui(&mut self) {
        let textures = self.chunk.bind().textures.clone();
        for texture_name in textures {
            let mut lab = Label::new_alloc();
            lab.add_theme_font_size_override("font_size", 14);
            lab.set_text(&texture_name);

            self.base_mut().add_child(&lab);
        }
    }
}
