// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */
//! This module contains Nodes and other Godot types that wrap the bare SR2 types.

mod aabb;
mod chunk;
mod chunk_error;
mod city_object;
mod transform;
mod vector;

pub use chunk::Chunk;
pub use chunk_error::ChunkError;
pub use city_object::CityObjectModel;

pub use aabb::{godot_aabb_to_sr2, sr2_aabb_to_godot};
pub use transform::{godot_xform_to_sr2, sr2_xform_to_godot};
pub use vector::{godot_vec_to_sr2, sr2_vec_to_godot};
