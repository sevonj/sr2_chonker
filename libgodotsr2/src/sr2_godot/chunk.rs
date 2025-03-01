// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::{
    classes::{Material, MeshInstance3D},
    prelude::*,
};
use std::io::{BufReader, Read, Seek};

use super::{
    aabb::build_bbox_mesh, sr2_vec_to_godot, CityObject, MeshMoverPosition, WorldCollision,
};

/// This [Node] is the Godot-representation of the entire SR2 Chunk, including CPU/GPU chunkfiles and the peg file.
#[derive(Debug, GodotClass)]
#[class(no_init, base=Node)]
pub struct Chunk {
    pub chunk_bbox: Gd<MeshInstance3D>,

    pub textures: Vec<String>,

    /// Found next to collision mopp
    pub unk_bb_min: Vector3,
    /// Found next to collision mopp
    pub unk_bb_max: Vector3,

    pub world_collision: Gd<WorldCollision>,

    pub objects: Vec<Gd<CityObject>>,

    pub mover_positions: Vec<Gd<MeshMoverPosition>>,

    base: Base<Node>,
}

#[godot_api]
impl INode for Chunk {
    fn ready(&mut self) {
        let chunk_bbox_mat: Gd<Material> = load("res://assets/materials/mat_gizmo_chunk_bbox.tres");

        let mut chunk_bbox = self.chunk_bbox.clone();
        chunk_bbox.set_material_override(&chunk_bbox_mat);
        chunk_bbox.set_name("chunk_bbox");

        self.base_mut().add_child(&chunk_bbox);

        for obj in self.objects.clone() {
            self.base_mut().add_child(&obj);
        }
        for obj in self.mover_positions.clone() {
            self.base_mut().add_child(&obj);
        }
        let unknown5 = self.world_collision.clone();

        self.base_mut().add_child(&unknown5);
    }
}

impl Chunk {
    pub fn new<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Gd<Self>, sr2::Sr2TypeError> {
        let chunk = sr2::Chunk::read(reader)?;

        let mut chunk_bbox = MeshInstance3D::new_alloc();
        chunk_bbox.set_mesh(&build_bbox_mesh(
            sr2_vec_to_godot(&chunk.header.bbox_min),
            sr2_vec_to_godot(&chunk.header.bbox_max),
        ));

        Ok(Gd::from_init_fn(|base| Self {
            chunk_bbox,

            textures: chunk.textures,

            unk_bb_min: sr2_vec_to_godot(&chunk.unk_bb_min),
            unk_bb_max: sr2_vec_to_godot(&chunk.unk_bb_max),

            world_collision: WorldCollision::from_sr2(&chunk.world_collision_vbuf),

            objects: chunk
                .objects
                .iter()
                .map(|o| CityObject::from_sr2(o.clone()))
                .collect(),

            mover_positions: chunk
                .mover_positions
                .iter()
                .map(MeshMoverPosition::from_sr2)
                .collect(),

            base,
        }))
    }
}
