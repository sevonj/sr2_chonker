// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::classes::control::SizeFlags;
use godot::prelude::*;

use godot::classes::{IVBoxContainer, VBoxContainer};

use crate::editor::ui::UiListButton;
use crate::sr2_godot::Chunk;

#[derive(Debug, GodotClass)]
#[class(no_init, base=VBoxContainer)]
pub struct MaterialBrowser {
    chunk: Gd<Chunk>,

    base: Base<VBoxContainer>,
}

#[godot_api]
impl IVBoxContainer for MaterialBrowser {
    fn ready(&mut self) {
        self.base_mut().set_h_size_flags(SizeFlags::EXPAND_FILL);
        self.refresh();
    }
}

#[godot_api]
impl MaterialBrowser {
    #[signal]
    fn selected(idx: u32);
}

impl MaterialBrowser {
    pub fn new(chunk: Gd<Chunk>) -> Gd<Self> {
        Gd::from_init_fn(|base| Self { chunk, base })
    }

    pub fn refresh(&mut self) {
        for mut child in self.base_mut().get_children().iter_shared() {
            child.queue_free();
        }

        let chunk = self.chunk.clone();
        for (i, mat) in chunk.bind().data.materials.iter().enumerate() {
            let mat_id = format!("{:#10X}", mat.mat_name_checksum);

            let mut button = UiListButton::new(&mat_id);
            button.connect(
                "pressed",
                &Callable::from_object_method(&self.to_gd(), "emit_signal")
                    .bind(&[Variant::from("selected"), Variant::from(i as u32)]),
            );
            self.base_mut().add_child(&button);
        }
    }
}
