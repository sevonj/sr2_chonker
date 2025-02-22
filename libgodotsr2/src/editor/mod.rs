// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

//! Contains editor components.
//! Shouldn't depend on [crate::sr2_types] directly, but use [crate::sr2_godot] types instead.

mod camera_cursor_gizmo;
mod camera_rig_orbit;
mod chonker_editor;
mod gizmo3d;
mod scene_grid;
mod ui_browser_panel;
mod ui_browser_textures;
mod viewport_camera_panel;
mod viewport_ui_root;

pub use camera_cursor_gizmo::{CameraCursorGizmo, CameraCursorGizmoMode};
pub use camera_rig_orbit::CameraRigOrbit;
pub use chonker_editor::ChonkerEditor;
pub use gizmo3d::Gizmo3D;
pub use scene_grid::SceneGrid;
pub use ui_browser_panel::UiBrowserPanel;
pub use ui_browser_textures::UiBrowserTextures;
pub use viewport_camera_panel::ViewportCameraPanel;
