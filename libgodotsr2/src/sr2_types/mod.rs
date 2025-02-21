// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! SR2 types module
//! The purpose of this module is to hold all SR2 types.
//! No editor or Godot code allowed!

mod sr2_chunk_header;
mod sr2_vector;

pub use sr2_chunk_header::Sr2ChunkHeader;
pub use sr2_vector::Sr2Vector;
