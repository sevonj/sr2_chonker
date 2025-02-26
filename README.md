# SR2 Chonker
Saints Row 2 city map editor  
Restart from scratch


## Repository Contents

- **SR2 Chonker**  
    Map editor built in Godot game engine. Not particularly useful in its current state.

- **SR2 Types**  
    Rust library for SR2 types, reverse-engineered from game assets. Completely separate of Godot or Chonker, and therefore potentially useful for other tools as well. Note that things may and will be renamed and moved around as more gets figured out.

- **Misc. developent tools**
    - [./types_validator](./types_validator) - A development tool for testing if sr2_types  behaves correctly
    - [./chunk_mangler`](./chunk_mangler) - A research tool for finding the purpose of different fields in chunk files.


## Development


### Directories

- [./project`](./project) - Editor project directory
- [./libgodotsr2`](./libgodotsr2) - Editor source code (a Rust [GDExtension](https://docs.godotengine.org/en/stable/tutorials/scripting/gdextension/what_is_gdextension.html))


### Building

**Requirements:**  
- [Godot 4.3](https://godotengine.org/)
- [Rust programming language](https://www.rust-lang.org/)

**Steps:**  
1. Build the Rust source.
    - Run `cargo build` in project/libgodorsr2
    - Use `--release` flag to make it release build.
2. Export godot project like normal or run it in the editor.


<sub><sup>(Volition pls share documentation)</sup></sub>  
<sub><sup>(rip)</sup></sub>
