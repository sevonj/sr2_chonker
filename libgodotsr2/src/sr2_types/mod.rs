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

mod sr2_chunk_header;
mod sr2_city_object;
mod sr2_gpu_mesh;
mod sr2_transform;
mod sr2_unknown3;
mod sr2_unknown4;
mod sr2_vector;

pub use sr2_chunk_header::Sr2ChunkHeader;
pub use sr2_city_object::Sr2CityObjectModel;
pub use sr2_gpu_mesh::Sr2GpuMeshUnk0;
pub use sr2_transform::Sr2Transform;
pub use sr2_unknown3::Sr2Unknown3;
pub use sr2_unknown4::Sr2Unknown4;
pub use sr2_vector::Sr2Vector;
