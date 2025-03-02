// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use byteorder::{LittleEndian, ReadBytesExt};
use godot::{
    classes::{ArrayMesh, Material},
    prelude::*,
};
use sr2::MeshBuffer;
use std::io::{BufReader, Cursor, Seek, SeekFrom};

use godot::classes::{mesh::PrimitiveType, MeshInstance3D, SurfaceTool};

use crate::editor::RenderLayer;

use super::{sr2_vec_to_godot, sr2_xform_to_godot};

#[derive(Debug, GodotClass)]
#[class(no_init, base=Node3D)]
pub struct Sr2MeshInstance {
    pub data: sr2::MeshInstance,

    pub submeshinst_a: Option<Gd<MeshInstance3D>>,
    pub submeshinst_b: Option<Gd<MeshInstance3D>>,

    base: Base<Node3D>,
}

#[godot_api]
impl INode3D for Sr2MeshInstance {
    fn ready(&mut self) {
        self.setup_node();
    }
}

impl Sr2MeshInstance {
    pub fn from_sr2(data: &sr2::MeshInstance, meshes: &[Sr2Mesh]) -> Gd<Self> {
        let mesh = &meshes[data.mesh_idx as usize];

        let submeshinst_a = mesh.submesh_a.as_ref().map(|sub| {
            let mut inst = MeshInstance3D::new_alloc();
            inst.set_mesh(sub);
            if mesh.submesh_b.is_some() {
                inst.set_layer_mask(RenderLayer::Unknown.mask());
            } else {
                inst.set_layer_mask(RenderLayer::Common.mask());
            }
            inst
        });

        let submeshinst_b = mesh.submesh_b.as_ref().map(|sub| {
            let mut inst = MeshInstance3D::new_alloc();
            inst.set_mesh(sub);
            inst.set_layer_mask(RenderLayer::Unknown.mask());
            inst
        });

        Gd::from_init_fn(|base| Self {
            data: data.clone(),
            submeshinst_a,
            submeshinst_b,
            base,
        })
    }

    fn setup_node(&mut self) {
        if let Some(mut inst) = self.submeshinst_a.clone() {
            inst.set_name("submeshinst_a");
            self.base_mut().add_child(&inst);
        }
        if let Some(mut inst) = self.submeshinst_b.clone() {
            inst.set_name("submeshinst_b");
            self.base_mut().add_child(&inst);
        }

        let position = sr2_vec_to_godot(&self.data.origin);
        let basis = sr2_xform_to_godot(&self.data.xform);

        self.base_mut().set_position(position);
        self.base_mut().set_basis(basis);
    }
}

#[derive(Debug)]
pub struct Sr2Mesh {
    pub submesh_a: Option<Gd<ArrayMesh>>,
    pub submesh_b: Option<Gd<ArrayMesh>>,
}

impl Sr2Mesh {
    pub fn from_sr2(mesh: &sr2::Mesh, buffer: &MeshBuffer) -> Self {
        let mat_b: Gd<Material> = load("res://assets/materials/mat_debug_yellow.tres");

        let submesh_a = mesh.submesh_a.as_ref().map(|sub| {
            if mesh.submesh_b.is_none() {
                let mat_a: Gd<Material> = load("res://assets/materials/mat_wireframe_opaque.tres");
                build_submesh(sub, buffer, &mat_a)
            } else {
                let mat_a_alt: Gd<Material> = load("res://assets/materials/mat_debug_pink.tres");
                build_pointcloud(sub, buffer, &mat_a_alt)
            }
        });

        let submesh_b = mesh
            .submesh_b
            .as_ref()
            .map(|sub| build_pointcloud(sub, buffer, &mat_b));

        Self {
            submesh_a,
            submesh_b,
        }
    }
}

fn build_submesh(submesh: &sr2::Submesh, buffer: &MeshBuffer, mat: &Gd<Material>) -> Gd<ArrayMesh> {
    let mut st = SurfaceTool::new_gd();

    st.begin(PrimitiveType::TRIANGLES);

    let mut mesh = ArrayMesh::new_gd();

    for surf in &submesh.surfaces {
        let vbuf = &buffer.vertex_buffers[surf.idx_vertex_buffer as usize];
        let stride = vbuf.stride() as u64;

        let cursor = Cursor::new(vbuf.data.as_slice());
        let mut reader = BufReader::new(cursor);

        let start_index = surf.start_index as usize;
        let end_index = start_index + surf.num_indices as usize;
        let start_vertex = surf.start_vertex as u64;
        for index in start_index..end_index - 2 {
            let ia = start_vertex + buffer.indices[index] as u64;
            let ib = start_vertex + buffer.indices[index + 1] as u64;
            let ic = start_vertex + buffer.indices[index + 2] as u64;

            add_vertex(&mut st, vbuf, stride, &mut reader, ia);
            add_vertex(&mut st, vbuf, stride, &mut reader, ib);
            add_vertex(&mut st, vbuf, stride, &mut reader, ic);
        }

        st.set_material(mat);
        mesh = st.commit_ex().existing(&mesh).done().unwrap();
    }

    mesh
}

fn add_vertex(
    st: &mut Gd<SurfaceTool>,
    vbuf: &sr2::VertexBuffer,
    stride: u64,
    reader: &mut BufReader<Cursor<&[u8]>>,
    i: u64,
) {
    reader.seek(SeekFrom::Start(i * stride)).unwrap();
    let pos = sr2::Vector::read(reader).unwrap();
    reader.seek_relative(2 * vbuf.num_vertex_a as i64).unwrap();
    let _u = reader.read_u16::<LittleEndian>().unwrap();
    let _v = reader.read_u16::<LittleEndian>().unwrap();
    st.add_vertex(sr2_vec_to_godot(&pos));
}

fn build_pointcloud(
    submesh: &sr2::Submesh,
    buffer: &MeshBuffer,
    mat: &Gd<Material>,
) -> Gd<ArrayMesh> {
    let mut st = SurfaceTool::new_gd();

    st.begin(PrimitiveType::POINTS);

    let mut mesh = ArrayMesh::new_gd();

    for surf in &submesh.surfaces {
        let vbuf = &buffer.vertex_buffers[surf.idx_vertex_buffer as usize];
        let ibuf = &buffer.indices;
        let stride = vbuf.stride() as u64;

        let cursor = Cursor::new(vbuf.data.as_slice());
        let mut reader = BufReader::new(cursor);

        let start = surf.start_index as usize;
        let end = start + surf.num_indices as usize;
        for index in ibuf.iter().take(end).skip(start) {
            let index = *index as u64 + surf.start_vertex as u64;
            reader.seek(SeekFrom::Start(index * stride)).unwrap();

            let pos = sr2::Vector::read(&mut reader).unwrap();
            reader.seek_relative(2 * vbuf.num_vertex_a as i64).unwrap();
            let _u = reader.read_u16::<LittleEndian>().unwrap();
            let _v = reader.read_u16::<LittleEndian>().unwrap();
            st.add_vertex(sr2_vec_to_godot(&pos));
        }

        st.set_material(mat);
        mesh = st.commit_ex().existing(&mesh).done().unwrap();
    }

    mesh
}
