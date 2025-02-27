// SPDX-License-Identifier: MPL-2.0
// SPDX-FileCopyrightText: 2025 sevonj
/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

use std::{
    fs::File,
    io::{BufReader, Read, Write},
    path::PathBuf,
};

use clap::Parser;
use colored::Colorize;
use rust_search::SearchBuilder;
use sr2::Chunk;

#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    /// Print errors
    #[arg(short, long)]
    verbose: bool,
    /// Can be either a file, or a dir for batch check
    #[arg(short, long)]
    path: String,
    /// Deserialize -> Re-Serialize -> Compare
    #[arg(short, long)]
    intense: bool,
    /// Save failed output files
    #[arg(short, long)]
    save_failed: bool,
    /// Only show failed files
    #[arg(short, long)]
    quiet: bool,
}

fn main() {
    let args = Args::parse();

    let pathbuf = PathBuf::from(&args.path);
    if !pathbuf.exists() {
        println!("Path doesn't exist.");
        return;
    }

    let files = get_files(&args.path);

    if files.is_empty() {
        println!("No files found!");
        return;
    }

    let num_files = files.len();
    println!("Found {num_files} files.");

    let mut failed = vec![];

    let mut progress_tracker = vec![];

    for (i, path) in files.iter().enumerate() {
        print!(
            "{}/{num_files} : {} {path}",
            i + 1,
            "[checking...]".yellow().bold()
        );
        match validate(&path, args.intense) {
            Ok(chunk) => {
                let chunk_total_size = chunk.bytes_mapped + chunk.remaining_data.len();
                let progress_percentage =
                    (chunk.bytes_mapped as f64 * 100.0 / chunk_total_size as f64) as usize;
                progress_tracker.push(progress_percentage);
                
                if !args.quiet {
                    println!(
                        "\r{}/{num_files} : {} ({:#3}%) {path}     ",
                        i + 1,
                        "[OK] ".green().bold(),
                        progress_percentage
                    );
                } else {
                    print!(
                        "\r                                                                                                    \r"
                    );
                }
            }
            Err((e, data)) => {
                failed.push(path.clone());
                println!(
                    "\r{}/{num_files} : {} {path}        ",
                    i + 1,
                    "[ERR]".red().bold()
                );
                if args.verbose {
                    println!("{}", format!("└> {e}").bright_black());
                }
                if args.save_failed {
                    if let Some(data) = data {
                        save_file(path, &data);
                    }
                }
                continue;
            }
        };
    }

    match failed.len() {
        0 => {
            if args.intense {
                println!("{}", "All files intensely ok!".bright_green())
            } else {
                println!("{}", "All files non-intensely ok!".bright_green())
            }
        }
        _ => println!(
            "{}",
            format!("{} out of {num_files} files failed.", failed.len()).yellow()
        ),
    }
    let sum: usize = progress_tracker.iter().sum();
    let len = progress_tracker.len();
    let avg = if len > 0 {
        format!("{}", sum / len)
    } else {
        "N/A".into()
    };
    println!("Roughly {avg}% of cpu chunk data mapped (excluding failed chunks)")
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

fn validate(path: &str, intense: bool) -> Result<Chunk, (String, Option<Vec<u8>>)> {
    let chunk = match sr2::Chunk::open(path) {
        Ok(chunk) => chunk,
        Err(e) => return Err((e.to_string(), None)),
    };

    if !intense {
        return Ok(chunk);
    }

    let serialized = chunk.to_bytes();
    let mut original = vec![];
    let file = File::open(path).unwrap();
    let mut reader = BufReader::new(file);
    reader.read_to_end(&mut original).unwrap();

    if serialized.len() != original.len() {
        let diff = serialized.len() as isize - original.len() as isize;
        if diff < 0 {
            return Err((
                format!("Sizes don't match. Output fell short by {}B", diff.abs()),
                Some(serialized),
            ));
        } else {
            return Err((
                format!("Sizes don't match. Output is bigger by {diff}B"),
                Some(serialized),
            ));
        }
    }
    if serialized != original {
        return Err(("Content doesn't match".into(), Some(serialized)));
    }

    Ok(chunk)
}

fn save_file(input_filepath: &str, bytes: &[u8]) {
    let mut pbuf = PathBuf::from(input_filepath);
    pbuf.set_extension("failed_chunk_pc");
    let filename = pbuf.file_name().unwrap();
    let output_dir = PathBuf::from("./failed_output/");
    let output_filepath = output_dir.join(filename);

    std::fs::create_dir_all(&output_dir).unwrap();
    let mut new_file = File::create(output_filepath).unwrap();

    new_file.write_all(&bytes).unwrap();
}
