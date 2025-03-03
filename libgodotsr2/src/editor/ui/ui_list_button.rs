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

use godot::global::HorizontalAlignment;
use godot::meta::AsArg;
use godot::prelude::*;

use godot::classes::{Button, IButton};

#[derive(Debug, GodotClass)]
#[class(no_init, base=Button)]
pub struct UiListButton {
    base: Base<Button>,
}

#[godot_api]
impl IButton for UiListButton {
    fn ready(&mut self) {}
}

impl UiListButton {
    pub fn new<S: AsArg<GString> + std::fmt::Display>(text: S) -> Gd<Self> {
        let mut this = Gd::from_init_fn(|base| Self { base });
        this.set_flat(true);
        this.set_name(format!("listbut_{text}").as_str());
        this.set_text(text);
        this.set_text_alignment(HorizontalAlignment::LEFT);
        this.add_theme_font_size_override("font_size", 14);
        this
    }
}
