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

use crate::sr2_types::Sr2ChunkHeader;

use super::{sr2_vec_to_godot, ChunkError};

#[derive(Debug, GodotClass)]
#[allow(dead_code)]
#[class(no_init, base=Node)]
pub struct Chunk {
    /// Load zone???
    pub bbox_min: Vector3,
    /// Load zone???
    pub bbox_max: Vector3,

    pub textures: Vec<String>,

    base: Base<Node>,
}

#[godot_api]
impl INode for Chunk {
    fn ready(&mut self) {
        let mut bbox = CsgBox3D::new_alloc();
        let bbox_size = self.bbox_max - self.bbox_min;
        // Abs: temporary
        bbox.set_size(bbox_size.abs());
        bbox.set_global_position(self.bbox_min + bbox_size / 2.0);

        self.base_mut().add_child(&bbox);
    }
}

impl Chunk {
    pub fn new<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Gd<Self>, ChunkError> {
        let mut header_buf = vec![0; size_of::<Sr2ChunkHeader>()];
        reader.read_exact(&mut header_buf)?;

        let header = Sr2ChunkHeader::read_from_bytes(&header_buf).unwrap();

        if header.magic != Sr2ChunkHeader::MAGIC {
            return Err(ChunkError::InvalidMagic(header.magic));
        } else if header.version != Sr2ChunkHeader::VERION {
            return Err(ChunkError::InvalidVersion(header.version));
        }

        let num_textures = reader.read_u32::<LittleEndian>().unwrap();
        reader.seek_relative(num_textures as i64 * 4)?;

        let mut textures = vec![];
        for _ in 0..num_textures {
            let mut buf = vec![];
            reader.read_until(0x00, &mut buf)?;
            textures.push(String::from_utf8_lossy(&buf).to_string());
        }

        Ok(Gd::from_init_fn(|base| Self {
            bbox_min: sr2_vec_to_godot(header.bbox_min),
            bbox_max: sr2_vec_to_godot(header.bbox_max),

            textures,

            base,
        }))
    }
}
