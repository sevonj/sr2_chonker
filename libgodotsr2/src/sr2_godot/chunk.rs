// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;
use std::path::PathBuf;

use godot::classes::{Material, MeshInstance3D};

use crate::editor::RenderLayer;

use super::{
    aabb::build_bbox_mesh, sr2_vec_to_godot, MeshMoverPosition, Sr2Mesh, Sr2MeshInstance,
    Sr2Object, WorldCollision,
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

    pub meshes: Vec<Sr2Mesh>,
    pub objects: Vec<Gd<Sr2Object>>,

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
            if obj.get_parent().is_some() {
                continue;
            }
            self.base_mut().add_child(&obj);
        }
        for mover in self.mover_positions.clone() {
            self.base_mut().add_child(&mover);
        }
        let unknown5 = self.world_collision.clone();

        self.base_mut().add_child(&unknown5);
    }
}

impl Chunk {
    pub fn new<P: Into<PathBuf>>(filepath: P) -> Result<Gd<Self>, sr2::Sr2TypeError> {
        let cpu_path: PathBuf = filepath.into();
        let gpu_path = cpu_path.with_extension("g_chunk_pc");

        let mut chunk = sr2::Chunk::open(&cpu_path)?;
        chunk.open_gpu(&gpu_path).unwrap();

        let mut chunk_bbox = MeshInstance3D::new_alloc();
        chunk_bbox.set_mesh(&build_bbox_mesh(
            sr2_vec_to_godot(&chunk.header.bbox_min),
            sr2_vec_to_godot(&chunk.header.bbox_max),
        ));
        chunk_bbox.set_layer_mask(RenderLayer::BBox.mask());

        let gpu_mesh_buf = chunk.gpu_mesh_buffer().unwrap();

        let meshes: Vec<_> = chunk
            .meshes
            .iter()
            .map(|mesh| Sr2Mesh::from_sr2(mesh, gpu_mesh_buf))
            .collect();

        let mesh_insts: Vec<_> = chunk
            .mesh_insts
            .iter()
            .map(|m| Sr2MeshInstance::from_sr2(m, &meshes))
            .collect();

        let objects = chunk
            .objects
            .iter()
            .map(|o| Sr2Object::from_sr2(o.clone(), &mesh_insts))
            .collect();

        let this = Gd::from_init_fn(|base| Self {
            chunk_bbox,

            textures: chunk.textures,

            unk_bb_min: sr2_vec_to_godot(&chunk.unk_bb_min),
            unk_bb_max: sr2_vec_to_godot(&chunk.unk_bb_max),

            world_collision: WorldCollision::from_sr2(&chunk.world_collision_vbuf),

            meshes,
            objects,

            mover_positions: chunk
                .mover_positions
                .iter()
                .map(MeshMoverPosition::from_sr2)
                .collect(),

            base,
        });

        Ok(this)
    }
}
