// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::{BufReader, Read, Seek};

use zerocopy::FromBytes;
use zerocopy_derive::{FromBytes, Immutable, IntoBytes};

use crate::Sr2TypeError;

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown19 {
    pub data_todo: [u8; 28],
    pub other_data: Unknown19Sub,
}

impl Unknown19 {
    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let mut buf = vec![0_u8; size_of::<Self>()];
        reader.read_exact(&mut buf)?;
        let this = Self::read_from_bytes(&buf).unwrap();

        Ok(this)
    }
}

#[derive(Debug, FromBytes, IntoBytes, Immutable, Clone)]
pub struct Unknown19Sub {
    pub data_todo: [f32; 7],
}
