using System;
using static Sr2ChunkMatlib;
using static Sr2GenericJSON;

public static class Sr2ChunkMatlibJSON
{
    // Top level struct, serialized into JSON.
    public struct Sr2ChunkMatlibJSONRoot
    {
        public String[] Textures { get; set; } // Filename of every texture. Don't touch; changes affecting file size will break the chunk.
        public Sr2ChunkMatlibJSONMaterial[] Materials { get; set; }
        public Sr2ChunkMatlibJSONShaderTextureProp[] ShaderTextureProps { get; set; }
    }

    // 4x floats. Can be RGBA values, or whatever else that's passed to shader.
    // Looks like often less than all four floats are used, leaving unused ones at 0.


    public struct Sr2ChunkMatlibJSONUnknown2
    {
        public UInt16 X { get; set; }
        public UInt16 Y { get; set; }
        public UInt16 Z { get; set; }

        public Sr2ChunkMatlibJSONUnknown2(Sr2ChunkMatlibUnknown2 unk2) : this()
        {
            this.X = unk2.X;
            this.Y = unk2.Y;
            this.Z = unk2.Z;
        }
    }

    public struct Sr2ChunkMatlibJSONTexture
    {
        public String Name { get; set; }   // Only use a name from texture list and make sure it matches exactly.
        public Int16 Flags { get; set; }
    }

    public struct Sr2ChunkMatlibJSONMaterial
    {
        public string Name { get; set; } // Not the real name. I created this for convenience.
        public UInt32 Unknown0x00 { get; set; }
        public UInt32 Shader { get; set; }  // Probably
        public UInt32 Unknown0x08 { get; set; }
        public Sr2ChunkMatlibJSONUnknown2[] Unknown2s { get; set; }
        public Sr2ChunkMatlibJSONTexture[] Textures { get; set; }
        public UInt32 Flags { get; set; }
        public Int32 Unknown0x14 { get; set; }
        // 2 different streams of constants per material
        public UInt32 IdxConstants0 { get; set; } // Starting index of this stream in constant block
        public UInt32 IdxConstants1 { get; set; } // Don't touch these
        public Sr2Vector4JSON[] Constants0 { get; set; } // Shader constants
        public Sr2Vector4JSON[] Constants1 { get; set; } // Shader constants
    }

    // These are probably texture properties for a particular shader.
    public struct Sr2ChunkMatlibJSONShaderTextureProp
    {
        public UInt32 Unknown0x00 { get; set; } // Texture?
        public UInt32 Shader { get; set; } // You should always be able to find one or more materials with matching shader
        public UInt32[] ShaderTexturePropData { get; set; }
        public UInt16 Unknown0x0A { get; set; }
    }


}

