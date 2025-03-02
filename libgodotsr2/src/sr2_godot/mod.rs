// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! This module contains Godot-instanceable types that correspond to the bare SR2 types.
//! Lossless conversion between the the two should eventually be implemented.
//! This depends on [crate::sr2], ~~but shouldn't depend on the editor~~. We'll see about that later.

mod aabb;
mod chunk;
mod mesh;
mod mesh_mover;
mod object;
mod transform;
mod unknown23;
mod vector;
mod world_collision;

pub use aabb::*;
pub use chunk::*;
pub use mesh::*;
pub use mesh_mover::*;
pub use object::*;
pub use transform::*;
pub use unknown23::*;
pub use vector::*;
pub use world_collision::*;
