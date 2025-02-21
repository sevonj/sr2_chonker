// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::error::Error;

#[derive(Debug)]
pub enum ChunkError {
    IOError { source: std::io::Error },
    InvalidMagic(u32),
    InvalidVersion(u32),
}

impl Error for ChunkError {}

impl std::fmt::Display for ChunkError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            ChunkError::IOError { source } => source.fmt(f),
            ChunkError::InvalidMagic(value) => write!(
                f,
                "Chunk has invalid signature. It this really a chunkfile? (got '{value:#10X}')"
            ),
            ChunkError::InvalidVersion(value) => write!(
                f,
                "Chunk has unknown version‽ Where'd you get this from? (got '{value}')"
            ),
        }
    }
}

impl From<std::io::Error> for ChunkError {
    fn from(source: std::io::Error) -> Self {
        ChunkError::IOError { source }
    }
}
