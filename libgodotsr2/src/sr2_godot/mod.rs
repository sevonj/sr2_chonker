// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! This module contains Godot-instanceable types that correspond to the bare SR2 types.
//! Lossless conversion between the the two should eventually be implemented.
//! This depends on [crate::sr2], but shouldn't depend on the editor.

mod aabb;
mod chunk;
mod city_object;
mod transform;
mod vector;
mod world_collision;

pub use chunk::Chunk;
pub use city_object::CityObjectModel;
pub use world_collision::WorldCollision;

pub use aabb::{godot_aabb_to_sr2, sr2_aabb_to_godot};
pub use transform::{godot_xform_to_sr2, sr2_xform_to_godot};
pub use vector::{godot_vec_to_sr2, sr2_vec_to_godot};
