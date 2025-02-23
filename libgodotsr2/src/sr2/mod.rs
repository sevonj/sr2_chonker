// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! Raw SR2 types.
//! The purpose of this module is to hold all SR2 types.
//! No editor or Godot code allowed!

mod chunk_header;
mod city_object;
mod gpu_mesh;
mod transform;
mod unknown3;
mod unknown4;
mod vector;
mod materials;

pub use chunk_header::Sr2ChunkHeader;
pub use city_object::Sr2CityObjectModel;
pub use gpu_mesh::Sr2GpuMeshUnk0;
pub use transform::Sr2Transform;
pub use unknown3::Sr2Unknown3;
pub use unknown4::Sr2Unknown4;
pub use vector::Sr2Vector;
