// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;
use std::io::{BufReader, Read, Seek};

use crate::sr2;

use super::{sr2_vec_to_godot, CityObjectModel, MaybeStaticCollision};

/// This [Node] is the Godot-representation of the entire SR2 Chunk, including CPU/GPU chunkfiles and the peg file.
#[derive(Debug, GodotClass)]
#[class(no_init, base=Node)]
pub struct Chunk {
    /// Load zone???
    pub bbox_min: Vector3,
    /// Load zone???
    pub bbox_max: Vector3,

    pub textures: Vec<String>,

    /// Found next to collision mopp
    pub unk_bb_min: Vector3,
    /// Found next to collision mopp
    pub unk_bb_max: Vector3,

    pub unknown5: Gd<MaybeStaticCollision>,

    pub city_object_models: Vec<Gd<CityObjectModel>>,

    base: Base<Node>,
}

#[godot_api]
impl INode for Chunk {
    fn ready(&mut self) {
        // let mut bbox = CsgBox3D::new_alloc();
        // let bbox_size = self.bbox_max - self.bbox_min;
        // // Abs: temporary
        // bbox.set_size(bbox_size.abs());
        // bbox.set_global_position(self.bbox_min + bbox_size / 2.0);
        // self.base_mut().add_child(&bbox);

        for cobj_model in self.city_object_models.clone() {
            self.base_mut().add_child(&cobj_model);
        }
        let unknown5 = self.unknown5.clone();
        self.base_mut().add_child(&unknown5);
    }
}

impl Chunk {
    pub fn new<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Gd<Self>, sr2::ChunkError> {
        let chunk = sr2::Chunk::read(reader)?;

        Ok(Gd::from_init_fn(|base| Self {
            bbox_min: sr2_vec_to_godot(&chunk.header.bbox_min),
            bbox_max: sr2_vec_to_godot(&chunk.header.bbox_max),

            textures: chunk.textures,

            unk_bb_min: sr2_vec_to_godot(&chunk.unk_bb_min),
            unk_bb_max: sr2_vec_to_godot(&chunk.unk_bb_max),

            unknown5: MaybeStaticCollision::from_sr2(&chunk.unknown5_vbuf),

            city_object_models: chunk
                .obj_models
                .iter()
                .map(CityObjectModel::from_sr2)
                .collect(),

            base,
        }))
    }
}
