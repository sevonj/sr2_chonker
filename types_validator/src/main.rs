// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use clap::Parser;
use colored::Colorize;
use rust_search::SearchBuilder;

#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    /// Print errors
    #[arg(short, long)]
    verbose: bool,
}

fn main() {
    let args = Args::parse();

    let path = "samples";
    let files = get_files(path);

    if files.is_empty() {
        println!("No files found!");
        return;
    }

    let num_files = files.len();
    println!("Found {num_files} files.");

    let mut failed = vec![];

    for (i, path) in files.iter().enumerate() {
        match sr2::Chunk::open(path) {
            Ok(_) => {
                println!("{}/{num_files} : {} {path}", i + 1, "[OK] ".green().bold());
            }
            Err(e) => {
                failed.push(path.clone());
                println!("{}/{num_files} : {} {path}", i + 1, "[ERR]".red().bold());
                if args.verbose {
                    println!("{}", format!("└> {e}").bright_black());
                }
            }
        }
    }

    match failed.len() {
        0 => println!("{}", "All  files ok!".bright_green()),
        _ => println!(
            "{}",
            format!("{} out of {num_files} files failed.", failed.len()).yellow()
        ),
    }
}

fn get_files(path: &str) -> Vec<String> {
    SearchBuilder::default()
        .location(path)
        .ext("chunk_pc")
        .strict()
        .hidden()
        .build()
        .collect()
}
