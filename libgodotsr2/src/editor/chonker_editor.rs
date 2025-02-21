// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::fs::File;
use std::io::BufReader;

use godot::classes::control::{LayoutPreset, MouseFilter, SizeFlags};
use godot::prelude::*;

use godot::classes::{HBoxContainer, MarginContainer, SubViewport, SubViewportContainer};

use super::sr2::{Chunk, ChunkError};
use super::UiBrowserPanel;
use super::{viewport_ui_root::ViewportUiRoot, CameraRigOrbit, SceneGrid, ViewportCameraPanel};

/// The root [Node] of an
#[derive(Debug, GodotClass)]
#[class(base=Node)]
pub struct ChonkerEditor {
    edited_chunk: Option<Gd<Chunk>>,

    scn_camera: Gd<CameraRigOrbit>,
    scn_grid: Gd<SceneGrid>,

    ui_hbox: Gd<HBoxContainer>,
    ui_browser: Gd<UiBrowserPanel>,

    viewport_margin: Gd<MarginContainer>,
    viewport_cont: Gd<SubViewportContainer>,
    viewport_ui: Gd<ViewportUiRoot>,
    viewport: Gd<SubViewport>,

    base: Base<Node>,
}

#[godot_api]
impl INode for ChonkerEditor {
    fn init(base: Base<Self::Base>) -> Self {
        Self {
            edited_chunk: None,

            scn_camera: CameraRigOrbit::new_alloc(),
            scn_grid: SceneGrid::new_alloc(),

            ui_hbox: HBoxContainer::new_alloc(),
            ui_browser: UiBrowserPanel::new_alloc(),

            viewport_margin: MarginContainer::new_alloc(),
            viewport_cont: SubViewportContainer::new_alloc(),
            viewport_ui: ViewportUiRoot::new_alloc(),
            viewport: SubViewport::new_alloc(),

            base,
        }
    }

    fn ready(&mut self) {
        self.setup_scene();
        self.setup_ui();
        self.setup_viewport_ui();

        let Some(mut vport) = self.base().get_viewport() else {
            godot_error!("Couldn't get viewport!");
            return;
        };
        vport.connect(
            "files_dropped",
            &Callable::from_object_method(&self.to_gd(), "on_files_dropped"),
        );
    }
}

#[godot_api]
impl ChonkerEditor {
    #[func]
    fn on_files_dropped(&mut self, files: PackedStringArray) {
        if files.len() != 1 {
            godot_warn!("Dropped too many files!")
        }
        if let Err(e) = self.load_chunk(files[0].to_string()) {
            godot_error!("{e}");
        }
    }
}

impl ChonkerEditor {
    fn setup_scene(&mut self) {
        let mut scn_camera = self.scn_camera.clone();
        scn_camera.set_name("scn_camera");

        let mut scn_grid = self.scn_grid.clone();
        scn_grid.bind_mut().follow_target = Some(scn_camera.clone().upcast());
        scn_grid.set_name("scn_grid");

        let mut viewport = self.viewport.clone();
        viewport.add_child(&scn_camera);
        viewport.add_child(&scn_grid);
        viewport.set_name("viewport");
    }

    fn setup_ui(&mut self) {
        let mut ui_browser = self.ui_browser.clone();
        ui_browser.set_name("ui_browser");

        let mut viewport_cont = self.viewport_cont.clone();
        viewport_cont.add_child(&self.viewport);
        viewport_cont.set_stretch(true);
        viewport_cont.set_name("viewport_cont");

        let mut viewport_ui = self.viewport_ui.clone();
        viewport_ui.set_name("viewport_ui");

        let mut viewport_margin = self.viewport_margin.clone();
        viewport_margin.add_child(&viewport_cont);
        viewport_margin.add_child(&viewport_ui);
        viewport_margin.set_h_size_flags(SizeFlags::EXPAND_FILL);
        viewport_margin.set_name("viewport_margin");

        let mut ui_hbox = self.ui_hbox.clone();
        ui_hbox.add_child(&ui_browser);
        ui_hbox.add_child(&viewport_margin);
        ui_hbox.add_theme_constant_override("separation", 0);
        ui_hbox.set_mouse_filter(MouseFilter::IGNORE);
        ui_hbox.set_anchors_preset(LayoutPreset::FULL_RECT);
        ui_hbox.set_name("ui_hbox");

        self.base_mut().add_child(&ui_hbox);
    }

    fn setup_viewport_ui(&mut self) {
        let mut viewport_camera_panel = ViewportCameraPanel::new(self.scn_camera.clone());
        viewport_camera_panel.set_name("viewport_camera_panel");

        let mut viewport = self.viewport_ui.bind_mut();
        viewport.add_ui_panel(&viewport_camera_panel.upcast());
    }

    pub fn edited_chunk(&self) -> Option<Gd<Chunk>> {
        self.edited_chunk.clone()
    }

    fn unload_chunk(&mut self) {
        if let Some(old_chunk) = &mut self.edited_chunk {
            old_chunk.queue_free();
            self.edited_chunk = None;
        }

        self.ui_browser.bind_mut().clear_chunk();
    }

    fn load_chunk(&mut self, filepath: String) -> Result<(), ChunkError> {
        self.unload_chunk();

        let mut reader = BufReader::new(File::open(filepath)?);
        let chunk = Chunk::new(&mut reader)?;

        self.viewport.add_child(&chunk);

        self.ui_browser.bind_mut().set_chunk(chunk.clone());
        self.edited_chunk = Some(chunk);

        Ok(())
    }
}
