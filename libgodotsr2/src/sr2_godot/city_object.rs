// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::{BoxMesh, IMeshInstance3D, MeshInstance3D};

use crate::sr2::Sr2CityObjectModel;

use super::sr2_vec_to_godot;
use super::sr2_xform_to_godot;

#[derive(Debug, GodotClass)]
#[class(no_init, base=MeshInstance3D)]
pub struct CityObjectModel {
    /// Always null?
    pub unk_0x30: i32,
    /// Always null?
    pub unk_0x34: i32,
    /// Always null?
    pub unk_0x38: i32,
    /// Always null?
    pub unk_0x3c: i32,
    /// Always null?
    pub unk_0x40: i32,
    /// Always null?
    pub unk_0x44: i32,
    /// Always null?
    pub unk_0x48: i32,

    /// Always 1f?
    pub unk_0x4c: f32,

    /// Always null?
    pub unk_0x50: i32,

    pub unk_0x54: f32,
    pub idx_gpu_model: i32,
    /// flags?
    pub unk_0x5c: i32,

    base: Base<MeshInstance3D>,
}

#[godot_api]
impl IMeshInstance3D for CityObjectModel {
    fn ready(&mut self) {
        self.setup_mesh();
    }
}

impl CityObjectModel {
    pub fn from_sr2(data: &Sr2CityObjectModel) -> Gd<Self> {
        let mut this = Gd::from_init_fn(|base| Self {
            unk_0x30: data.unk_0x30,
            unk_0x34: data.unk_0x34,
            unk_0x38: data.unk_0x38,
            unk_0x3c: data.unk_0x3c,
            unk_0x40: data.unk_0x40,
            unk_0x44: data.unk_0x44,
            unk_0x48: data.unk_0x48,
            unk_0x4c: data.unk_0x4c,
            unk_0x50: data.unk_0x50,
            unk_0x54: data.unk_0x54,
            idx_gpu_model: data.idx_gpu_model,
            unk_0x5c: data.unk_0x5c,

            base,
        });
        let basis = sr2_xform_to_godot(&data.xform);
        let origin = sr2_vec_to_godot(&data.origin);
        this.set_transform(Transform3D::new(basis, origin));

        this
    }

    fn setup_mesh(&mut self) {
        let mesh = BoxMesh::new_gd();

        self.base_mut().set_mesh(&mesh);
    }
}
