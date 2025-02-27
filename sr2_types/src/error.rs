// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::error::Error;

#[derive(Debug)]
pub enum Sr2TypeError {
    IOError { source: std::io::Error },
    ChunkInvalidMagic(u32),
    ChunkInvalidVersion(u32),
    ChunkLostTrack { msg: String, pos: u64 },
    UnexpectedData { pos: u64 },
    VertexStrideMismatch { pos: u64 },
    UnexpectedMeshBufferType,
    VertexBufLenStrideMismatch,
}

impl Error for Sr2TypeError {}

impl std::fmt::Display for Sr2TypeError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Sr2TypeError::IOError { source } => source.fmt(f),
            Sr2TypeError::ChunkInvalidMagic(value) => write!(
                f,
                "Chunk has invalid signature. It this really a chunkfile? (got '{value:#10X}')"
            ),
            Sr2TypeError::ChunkInvalidVersion(value) => write!(
                f,
                "Chunk has unknown version‽ Where'd you get this from? (got '{value}')"
            ),
            Sr2TypeError::ChunkLostTrack { msg, pos } => {
                write!(f, "Something's gone wrong, lost track of chunk. (pos: {pos:#X}, message: '{msg}')")
            }
            Sr2TypeError::UnexpectedData { pos } => {
                write!(f, "Unexpected data at position '{pos:#X}'")
            }
            Sr2TypeError::VertexStrideMismatch { pos } => {
                write!(
                    f,
                    "Vertex stride doesn't match what it should be. Pos: '{pos:#X}'"
                )
            }
            Sr2TypeError::UnexpectedMeshBufferType => {
                write!(f, "Mesh has unexpected type.")
            }
            Sr2TypeError::VertexBufLenStrideMismatch => {
                write!(f, "Vertex buffer size isn't a multiple of vertex stride.")
            }
        }
    }
}

impl From<std::io::Error> for Sr2TypeError {
    fn from(source: std::io::Error) -> Self {
        Sr2TypeError::IOError { source }
    }
}
