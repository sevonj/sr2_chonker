// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{IVBoxContainer, VBoxContainer};

use crate::editor::ui::UiListButton;
use crate::sr2_godot::Chunk;

#[derive(Debug, GodotClass)]
#[class(no_init, base=VBoxContainer)]
pub struct TextureBrowser {
    chunk: Gd<Chunk>,

    base: Base<VBoxContainer>,
}

#[godot_api]
impl IVBoxContainer for TextureBrowser {
    fn ready(&mut self) {
        self.refresh();
    }
}

#[godot_api]
impl TextureBrowser {
    #[signal]
    fn selected(idx: u32);
}

impl TextureBrowser {
    pub fn new(chunk: Gd<Chunk>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self { chunk, base })
    }

    pub fn refresh(&mut self) {
        for mut child in self.base_mut().get_children().iter_shared() {
            child.queue_free();
        }

        let chunk = self.chunk.clone();
        for (i, name) in chunk.bind().data.textures.iter().enumerate() {
            let mut button = UiListButton::new(name);
            button.connect(
                "pressed",
                &Callable::from_object_method(&self.to_gd(), "emit_signal")
                    .bind(&[Variant::from("selected"), Variant::from(i as u32)]),
            );
            self.base_mut().add_child(&button);
        }
    }
}
