// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::io::Seek;

pub fn seek_align<R: Seek>(reader: &mut R, size: i64) -> Result<(), std::io::Error> {
    let pos = reader.stream_position()? as i64;
    reader.seek_relative((size - (pos % size)) % size)
}
