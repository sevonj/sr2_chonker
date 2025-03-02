pub struct City {}

impl City {
    pub const MAGIC: u32 = 0xBBC17EEE;
    pub const VERSION: u32 = 43;

    /// Read from stream
    pub fn read<R: Read + Seek>(reader: &mut BufReader<R>) -> Result<Self, Sr2TypeError> {
        let magic = reader.read_u32::<LittleEndian>()?;
        let version = reader.read_u32::<LittleEndian>()?;
        
        if chunk_header.magic != Self::MAGIC {
            return Err(Sr2TypeError::ChunkInvalidMagic(chunk_header.magic));
        }
        if chunk_header.version != Self::VERSION {
            return Err(Sr2TypeError::ChunkInvalidVersion(chunk_header.version));
        }

        let this = Self {
            shader_name_checksum,
            mat_name_checksum,
            flags,
            unknown2: vec![MaterialUnknown2::placeholder(); num_unknown_2b as usize],
            textures: vec![MaterialTextureEntry::placeholder(); num_textures as usize],
            unk_0x10,
            flags_0x12,
            runtime_0x14,
            unknown_16b_struct: [0_u8; 16],
        };

        if runtime_0x14 != -1 && runtime_0x14 != 0 {
            let pos = reader.stream_position()? - 0x4;
            return Err(Sr2TypeError::UnexpectedData { pos });
        }

        Ok(this)
    }
}
