// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! Contains editor components.
//! Shouldn't depend on [crate::sr2] directly, but use [crate::sr2_godot] types instead.

mod camera_cursor_gizmo;
mod camera_rig_orbit;
mod chonker_editor;
mod gizmo3d;
pub mod materials;
mod render_layers;
mod scene_axis_lines;
mod scene_grid;
pub mod ui;
mod viewport_camera_panel;
mod viewport_ui_root;
mod viewport_visibility_panel;

pub use camera_cursor_gizmo::{CameraCursorGizmo, CameraCursorGizmoMode};
pub use camera_rig_orbit::CameraRigOrbit;
pub use chonker_editor::ChonkerEditor;
pub use gizmo3d::Gizmo3D;
pub use render_layers::*;
pub use scene_axis_lines::SceneAxisLines;
pub use scene_grid::SceneGrid;
pub use viewport_camera_panel::ViewportCameraPanel;
pub use viewport_visibility_panel::ViewportVisibilityPanel;
