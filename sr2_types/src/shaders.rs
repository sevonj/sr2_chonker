// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

pub enum TextureType {
    Alpha,
    Metal,
    Normal,
    Specular,
    Unknown,
}

pub enum Shader {
    /// This shader hasn't been mapped.
    Unknown,
    X100742ad,
}

impl From<u32> for Shader {
    fn from(value: u32) -> Self {
        match value {
            0x100742AD => Shader::X100742ad,
            _ => (),
        }
    }
}

impl Shader {
    pub fn textures(&self) -> Vec<TextureType> {
        match self {
            Shader::Unknown => vec![],
            Shader::X100742ad => vec![TextureType::Metal, TextureType::Normal, TextureType::Metal],
        }
    }
}
