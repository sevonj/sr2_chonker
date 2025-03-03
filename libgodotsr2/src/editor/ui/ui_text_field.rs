// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::classes::control::SizeFlags;
use godot::meta::AsArg;
use godot::prelude::*;

use godot::classes::{HBoxContainer, IHBoxContainer, Label, LineEdit};

#[derive(Debug, GodotClass)]
#[class(no_init, base=HBoxContainer)]
pub struct UiTextField {
    ui_title: Gd<Label>,
    ui_field: Gd<LineEdit>,

    base: Base<HBoxContainer>,
}

#[godot_api]
impl IHBoxContainer for UiTextField {
    fn ready(&mut self) {
        self.setup_ui();
    }
}

impl UiTextField {
    pub fn new_read_only<S: AsArg<GString>>(title: S, value: S) -> Gd<Self> {
        let mut ui_title = Label::new_alloc();
        ui_title.set_text(title);
        let mut ui_field = LineEdit::new_alloc();
        ui_field.set_text(value);
        ui_field.set_editable(false);

        Gd::from_init_fn(|base| Self {
            ui_title,
            ui_field,
            base,
        })
    }

    pub fn setup_ui(&mut self) {
        let mut ui_title = self.ui_title.clone();
        ui_title.set_h_size_flags(SizeFlags::EXPAND_FILL);
        self.base_mut().add_child(&ui_title);

        let mut ui_field = self.ui_field.clone();
        ui_field.set_h_size_flags(SizeFlags::EXPAND_FILL);
        self.base_mut().add_child(&ui_field);

        self.base_mut().set_h_size_flags(SizeFlags::EXPAND_FILL);

    }
}
