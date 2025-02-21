// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

/// A 3D transform gizmo for selected objects.
#[derive(Debug, GodotClass)]
#[class(base=Node3D)]
pub struct Gizmo3D {
    base: Base<Node3D>,
}

#[godot_api]
impl INode3D for Gizmo3D {
    fn init(base: Base<Self::Base>) -> Self {
        Self { base }
    }
}
