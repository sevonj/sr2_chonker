// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use godot::prelude::*;

use godot::classes::input::MouseMode;

#[derive(Debug)]
pub enum CameraCursorGizmoMode {
    Rotate,
    Move,
    Zoom,
}

/// A 2D gizmo that shows in place of captured mouse cursor when camera is rotating/moving/etc.
/// Right now it's just a teal rectangle.
#[derive(Debug, GodotClass)]
#[class(base=Node2D)]
pub struct CameraCursorGizmo {
    mode: CameraCursorGizmoMode,

    position: Vector2,

    base: Base<Node2D>,
}

#[godot_api]
impl INode2D for CameraCursorGizmo {
    fn init(base: Base<Self::Base>) -> Self {
        Self {
            mode: CameraCursorGizmoMode::Move,

            position: Vector2::ZERO,

            base,
        }
    }

    fn ready(&mut self) {}

    fn process(&mut self, _delta: f64) {
        self.base_mut().queue_redraw();
    }

    fn draw(&mut self) {
        let input = Input::singleton();

        if input.get_mouse_mode() != MouseMode::CAPTURED {
            return;
        }

        const SIZE: Vector2 = Vector2::new(8.0, 8.0);
        let rect = Rect2::new(self.position - SIZE / 2.0, SIZE);
        let color = Color::TEAL;
        self.base_mut().draw_rect(rect, color);
    }
}

impl CameraCursorGizmo {
    pub fn set_mode(&mut self, mode: CameraCursorGizmoMode) {
        self.mode = mode;
    }

    pub fn set_position(&mut self, position: Vector2) {
        self.position = position;
    }
}
