// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::{fs::File, io::Write, path::PathBuf};

use clap::Parser;
use sr2::Chunk;

/// Command line arguments
#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    /// File to mangle
    #[arg(short, long)]
    path: String,
}

fn main() {
    let args = Args::parse();
    let path = &args.path;
    println!("mangling: {path}");

    // Open chunk
    let mut chunk = match sr2::Chunk::open(path) {
        Ok(chunk) => chunk,
        Err(e) => {
            println!("{e}");
            return;
        }
    };

    // Mangle chunk
    do_things(&mut chunk);

    // Save chunk
    let bytes = chunk.to_bytes();

    let pbuf = PathBuf::from(path);
    let filename = pbuf.file_name().unwrap();
    let output_dir = PathBuf::from("./mangled/");
    let output_filepath = output_dir.join(filename);

    std::fs::create_dir_all(&output_dir).unwrap();
    let mut new_file = File::create(output_filepath).unwrap();

    new_file.write_all(&bytes).unwrap();
}

fn do_things(chunk: &mut Chunk) {
    // Move every object model 1m to the left
    //for model in &mut chunk.obj_models {
    //    model.origin.x += 1.0;
    //}

    // Randomize shader constants for clown vomit

    for value in &mut chunk.shader_consts {
        *value = rand::random_range(0.0..1.0);
    }
}
