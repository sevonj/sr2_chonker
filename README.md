# SR2 Chonker
Saints Row 2 Chunk Editor  
Restart from scratch branch

## Development

### Repository contents
- `./project` -  Godot project for the editor
- `./libgodotsr2` - A GDExtension that contains the actual source code for the editor
- `./sr2_types` - SR2 types crate
- `./types_validator` - A development tool for testing if `sr2_types` behaves correctly
- `./chunk_mangler` - A research tool for finding the purpose of different fields in chunk files.

### Building

Instructions for SR2 Chonker. For the other tools, see readmes in the subfolders

**Requirements:**  
- [Godot 4.3](https://godotengine.org/)
- [Rust programming language](https://www.rust-lang.org/)

**Steps:**  
1. Build the Rust source.
    - Example build command:  
    `cargo build -p godotsr2 --release --target=x86_64-pc-windows-gnu`  
    Explanation:
        - `-p` which package to build. `godotsr2` is the editor.
        - `--release` Optional release build flag. Defaults to debug build if left out.
        - `--target` Target platform. Use`x86_64-unknown-linux-gnu` for Linux and `x86_64-pc-windows-gnu` for Windows.
            - You must specify a target for the editor, because Godot expects the binary from a specific path. For the other tools, this isn't necessary.
            - If the compiler complains about a missing target, you can install a target with `rustup target add <target>`
2. Export godot project like normal or run it in the editor.

Read more about GDExtension at [Godot Docs](https://docs.godotengine.org/en/stable/tutorials/scripting/gdextension/what_is_gdextension.html)

<sub><sup>(Volition pls share documentation)</sup></sub>  
<sub><sup>(rip)</sup></sub>
