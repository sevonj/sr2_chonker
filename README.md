# SR2 Chonker
Saints Row 2 city map editor  
Restart from scratch


## Repository Contents

- **SR2 Chonker**  
    Map editor built in Godot game engine. Not particularly useful in its current state.

- **SR2 Types**  
    Rust library for SR2 types, reverse-engineered from game assets. It's completely independent of chonker, and potentially useful for other tools as well.


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
    <details>
    <summary> Instructions </summary>
    - Example build command:  
    ```cargo build -p godotsr2 --release --target=x86_64-pc-windows-gnu```  
    Explanation:
        - `-p` which package to build. `godotsr2` is the editor.
        - `--release` Optional release build flag. Defaults to debug build if left out.
        - `--target` Target platform. Use`x86_64-unknown-linux-gnu` for Linux and `x86_64-pc-windows-gnu` for Windows.
            - You must specify a target for the editor, because Godot expects the binary from a specific path. For the other tools, this isn't necessary.
            - If the compiler complains about a missing target, you can install a target with `rustup target add <target>`
    </details>
2. Export godot project like normal or run it in the editor.


<sub><sup>(Volition pls share documentation)</sup></sub>  
<sub><sup>(rip)</sup></sub>
