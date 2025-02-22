// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use byteorder::{LittleEndian, ReadBytesExt};
use godot::prelude::*;
use std::io::{BufRead, BufReader, Read, Seek};
use zerocopy::FromBytes;

use godot::{classes::CsgBox3D, obj::WithBaseField};

use crate::sr2_types::{
    Sr2ChunkHeader, Sr2CityObjectModel, Sr2GpuMeshUnk0, Sr2Unknown3, Sr2Unknown4, Sr2Vector,
};

use super::{sr2_vec_to_godot, ChunkError, CityObjectModel};

#[derive(Debug, GodotClass)]
#[allow(dead_code)]
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
    }
}

impl Chunk {
    pub fn new<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Gd<Self>, ChunkError> {
        let header = {
            let mut buf = vec![0; size_of::<Sr2ChunkHeader>()];
            reader.read_exact(&mut buf)?;
            Sr2ChunkHeader::read_from_bytes(&buf).unwrap()
        };

        if header.magic != Sr2ChunkHeader::MAGIC {
            return Err(ChunkError::InvalidMagic(header.magic));
        } else if header.version != Sr2ChunkHeader::VERION {
            return Err(ChunkError::InvalidVersion(header.version));
        }

        let num_textures = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_textures as i64 * 4)?;
        let mut textures = vec![];
        for _ in 0..num_textures {
            let mut buf = vec![];
            reader.read_until(0x00, &mut buf)?;
            textures.push(String::from_utf8_lossy(&buf).to_string());
        }

        seek_align(reader, 16)?;

        let num_rendermodels = reader.read_u32::<LittleEndian>()?;
        let num_city_object_models = reader.read_u32::<LittleEndian>()?;
        let num_models = reader.read_u32::<LittleEndian>()?;
        let num_unknown3 = reader.read_u32::<LittleEndian>()?;
        let num_unknown4 = reader.read_u32::<LittleEndian>()?;

        let mut rendermodel_unk0s = vec![];
        let mut city_object_models = vec![];
        let mut unknown3s = vec![];
        let mut unknown4s = vec![];

        seek_align(reader, 16)?;

        for _ in 0..num_rendermodels {
            let mut buf = vec![0; size_of::<Sr2GpuMeshUnk0>()];
            reader.read_exact(&mut buf)?;
            rendermodel_unk0s.push(Sr2GpuMeshUnk0::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..num_city_object_models {
            let mut buf = vec![0; size_of::<Sr2CityObjectModel>()];
            reader.read_exact(&mut buf)?;
            city_object_models.push(Sr2CityObjectModel::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;

        for _ in 0..num_unknown3 {
            let mut buf = vec![0; size_of::<Sr2Unknown3>()];
            reader.read_exact(&mut buf)?;
            unknown3s.push(Sr2Unknown3::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;
        godot_print!("tell: {:#X}", reader.stream_position().unwrap());

        for _ in 0..num_unknown4 {
            let mut buf = vec![0; size_of::<Sr2Unknown4>()];
            reader.read_exact(&mut buf)?;
            unknown4s.push(Sr2Unknown4::read_from_bytes(&buf).unwrap());
        }

        seek_align(reader, 16)?;
        godot_print!("tell: {:#X}", reader.stream_position().unwrap());

        // TODO: Unskip
        // maybe collision vertices.
        // Type: vector3
        let num_unk_worldpos = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_unk_worldpos as i64 * 12)?;

        // Type: unknown 3B. triangle indices?
        let num_unknown6 = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_unknown6 as i64 * 3)?;

        // Type: unknown 4B
        let num_unknown7 = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_unknown7 as i64 * 4)?;

        // Type: unknown 12B (recheck, were these floats or something?)
        let num_unknown8 = reader.read_u32::<LittleEndian>()?;
        reader.seek_relative(num_unknown8 as i64 * 12)?;

        seek_align(reader, 16)?;

        // Havok collisions stuff
        let len_mopp = reader.read_u32::<LittleEndian>()?;

        seek_align(reader, 16)?;

        {
            // Using MOPP magic for sanity check
            let mut buf = vec![0; 4];
            reader.read_exact(&mut buf)?;
            let mopp_magic = String::from_utf8_lossy(&buf).to_string();
            if mopp_magic.as_str() != "MOPP" {
                let msg = "First MOPP signature didn't appear where expected.".into();
                let pos = reader.stream_position().unwrap() as i64;
                return Err(ChunkError::LostTrack { msg, pos });
            }
        }
        reader.seek_relative(len_mopp as i64 - 4)?;

        seek_align(reader, 4)?;

        // Collision area AABB?
        let unk_bb_min = {
            let mut buf = vec![0; size_of::<Sr2Vector>()];
            reader.read_exact(&mut buf)?;
            Sr2Vector::read_from_bytes(&buf).unwrap()
        };
        let unk_bb_max = {
            let mut buf = vec![0; size_of::<Sr2Vector>()];
            reader.read_exact(&mut buf)?;
            Sr2Vector::read_from_bytes(&buf).unwrap()
        };

        seek_align(reader, 16)?;

        Ok(Gd::from_init_fn(|base| Self {
            bbox_min: sr2_vec_to_godot(&header.bbox_min),
            bbox_max: sr2_vec_to_godot(&header.bbox_max),

            textures,

            unk_bb_min: sr2_vec_to_godot(&unk_bb_min),
            unk_bb_max: sr2_vec_to_godot(&unk_bb_max),

            city_object_models: city_object_models
                .iter()
                .map(CityObjectModel::from_sr2)
                .collect(),

            base,
        }))
    }
}

fn seek_align<R: Seek>(reader: &mut R, size: i64) -> Result<(), std::io::Error> {
    let pos = reader.stream_position().unwrap() as i64;
    reader.seek_relative((size - (pos % size)) % size)
}
