// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use crate::editor::ui::*;
use crate::sr2_godot::Chunk;
use godot::classes::control::SizeFlags;
use godot::prelude::*;

use godot::classes::{HBoxContainer, IMarginContainer, Label, MarginContainer, VBoxContainer};

#[derive(Debug, GodotClass)]
#[class(no_init, base=MarginContainer)]
pub struct MaterialInspector {
    chunk: Gd<Chunk>,
    mat_index: usize,

    ui_mat_checksum: Gd<UiTextField>,
    ui_shad_checksum: Gd<UiTextField>,
    //flags
    //unknown2
    ui_textures: Gd<VBoxContainer>,
    //unk_0x10
    //flags_0x12
    //runtime_0x14
    base: Base<MarginContainer>,
}

#[godot_api]
impl IMarginContainer for MaterialInspector {
    fn ready(&mut self) {
        self.base_mut().set_h_size_flags(SizeFlags::EXPAND_FILL);
        self.setup_ui();
    }
}

impl MaterialInspector {
    pub fn new(chunk: Gd<Chunk>, mat_index: usize) -> Gd<Self> {
        let material = &chunk.bind().data.materials[mat_index].clone();

        let ui_mat_checksum = UiTextField::new_read_only(
            "Material:",
            format!("{:#10X}", material.mat_name_checksum).as_str(),
        );

        let ui_shad_checksum = UiTextField::new_read_only(
            "Shader:",
            format!("{:#10X}", material.shader_name_checksum).as_str(),
        );

        let ui_textures = VBoxContainer::new_alloc();

        Gd::from_init_fn(|base| Self {
            chunk,
            mat_index,
            ui_mat_checksum,
            ui_shad_checksum,
            ui_textures,
            base,
        })
    }

    pub fn setup_ui(&mut self) {
        let mut main_vbox = VBoxContainer::new_alloc();
        main_vbox.set_name("main_vbox");
        self.base_mut().add_child(&main_vbox);

        let mut ui_mat_checksum = self.ui_mat_checksum.clone();
        ui_mat_checksum.set_name("ui_mat_checksum");
        main_vbox.add_child(&ui_mat_checksum);

        let mut ui_shad_checksum = self.ui_shad_checksum.clone();
        ui_shad_checksum.set_name("ui_shad_checksum");
        main_vbox.add_child(&ui_shad_checksum);

        let mut ui_textures = self.ui_textures.clone();
        ui_textures.set_name("ui_textures");
        ui_textures.set_h_size_flags(SizeFlags::EXPAND_FILL);

        let mat = &self.chunk.bind().data.materials[self.mat_index].clone();

        for (idx, tex) in mat.textures.iter().enumerate() {
            ui_textures.add_child(&UiTextureEntry::new(idx, tex, &self.chunk.bind().data));
        }

        main_vbox.add_child(&ui_textures);
    }
}

#[derive(Debug, GodotClass)]
#[class(no_init, base=HBoxContainer)]
struct UiTextureEntry {
    _ui_field: Gd<Label>,
    _ui_texture: Gd<UiTextField>,
    _ui_flags: Gd<UiTextField>,
    base: Base<HBoxContainer>,
}

impl UiTextureEntry {
    pub fn new(idx: usize, data: &sr2::MaterialTextureEntry, chunk: &sr2::Chunk) -> Gd<Self> {
        let mut vbox = VBoxContainer::new_alloc();
        vbox.set_h_size_flags(SizeFlags::EXPAND_FILL);

        let mut ui_field = Label::new_alloc();
        ui_field.set_text(format!("Field {idx}: TODO").as_str());
        vbox.add_child(&ui_field);

        let texture_name = &chunk.textures[data.index as usize];
        let ui_texture = UiTextField::new_read_only("Texture:", &texture_name);
        vbox.add_child(&ui_texture);

        let ui_flags = UiTextField::new_read_only("Flags:", format!("{:#6X}", data.flags).as_str());
        vbox.add_child(&ui_flags);

        let mut this = Gd::from_init_fn(|base| Self {
            _ui_field: ui_field,
            _ui_texture: ui_texture,
            _ui_flags: ui_flags,
            base,
        });

        this.set_h_size_flags(SizeFlags::EXPAND_FILL);
        this.add_child(&vbox);
        this
    }
}
