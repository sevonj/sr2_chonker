using Node = Godot.Node;
using System;
using System.IO;
using Kaitai;
using System.Text.Json;

public class MaterialHandler : Node
{
    public void LoadMaterial(ref Sr2ChunkPc chunk)
    {

    }

    struct Texture
    {
        public UInt16 TexIndex { get; set; }
        public String TexName { get; set; }
        public UInt16 TexFlags { get; set; }
    }
    struct Material
    {
        public byte[] Unk0 { get; set; }
        public UInt16[] Constants { get; set; } // ("constants" in SRIV)
        public Texture[] Textures { get; set; }
        public UInt32 Flags { get; set; } // (probably)
        public Int32 Unk1 { get; set; } // "name_checksum" in SRIV
        public UInt32[] Unk2s { get; set; }
    }
    struct MatUnknown3
    {
        public byte[] Unk1 { get; set; }
        public UInt32[] Unk2s { get; set; }
        public UInt16 Unk3 { get; set; }
    }

    struct MaterialSerialize
    {
        public String[] Textures { get; set; }
        public UInt32 MatUnknown1 { get; set; }
        public Material[] Materials { get; set; }
        public Single[] MatShaderParams { get; set; }
        public MatUnknown3[] MatUnknown3s { get; set; }
    }

    public void Unpack(ref Sr2ChunkPc chunk, string dir)
    {
        string subdir = Path.Combine(dir, "materials");

        Directory.CreateDirectory(dir);
        string filename_txt = Path.Combine(dir, "materials.txt");
        using (StreamWriter sw = File.CreateText(filename_txt))
        {
            sw.WriteLine("Materials");
            sw.WriteLine();
            sw.WriteLine("The editor can now spit out material data in JSON. It cannot put the data back in yet, but I'm working on it.");
            sw.WriteLine("The JSON file should contain everything, though not necessarily arranged in the best way. If you know more about SR2 materials, please let me know.");
            sw.WriteLine();
            sw.WriteLine("Editing JSON:");
            sw.WriteLine("Don't edit texture names and expect changes: they are only for help and will be ignored. Edit Texture Index instead.");
            sw.WriteLine("Don't change the length of any entry. Changing filesize will break the chunk.");
        }

        string filename_texlist = Path.Combine(dir, "materials.json");
        using (StreamWriter sw = File.CreateText(filename_texlist))
        {
            Material[] materials = new Material[chunk.NumMaterials];

            var matSer = new MaterialSerialize
            {
                Textures = new String[chunk.NumTextureNames],
                MatUnknown1 = chunk.MatUnknown1,
                Materials = new Material[chunk.NumMaterials],
                MatShaderParams = new Single[chunk.NumMatShaderParams],
                MatUnknown3s = new MatUnknown3[chunk.NumMatUnknown3s]
            };

            for (int i = 0; i < chunk.NumTextureNames; i++)
            { matSer.Textures[i] = chunk.TextureNames[i]; }

            for (int i = 0; i < chunk.NumMaterials; i++)
            {
                Sr2ChunkPc.Material mat = chunk.Materials[i];
                Sr2ChunkPc.MatUnknown2 unk2 = chunk.MatUnknown2s[i];
                Sr2ChunkPc.MatTexCont texs = chunk.MatTextures[i];

                matSer.Materials[i] = new Material
                {
                    Unk0 = mat.Unk,
                    Constants = new UInt16[mat.NumConstants * 6],
                    Textures = new Texture[mat.NumTextures],
                    Flags = mat.MatFlags,
                    Unk1 = mat.Unk2,
                    Unk2s = new UInt32[4]
                };
                for (int ii = 0; ii < mat.NumConstants * 6; ii++)
                {
                    matSer.Materials[i].Constants[ii] = chunk.MatConstants[i].Data[ii];
                }
                for (int ii = 0; ii < mat.NumTextures; ii++)
                {
                    matSer.Materials[i].Textures[ii] = new Texture
                    {
                        TexIndex = texs.TexData[ii].TexId,
                        TexName = chunk.TextureNames[texs.TexData[ii].TexId],
                        TexFlags = texs.TexData[ii].TexFlag
                    };
                }
                matSer.Materials[i].Unk2s[0] = unk2.Data[0];
                matSer.Materials[i].Unk2s[1] = unk2.Data[1];
                matSer.Materials[i].Unk2s[2] = unk2.Data[2];
                matSer.Materials[i].Unk2s[3] = unk2.Data[3];
            }

            for (int i = 0; i < chunk.NumMatShaderParams; i++)
            { matSer.MatShaderParams[i] = chunk.MatShaderParams[i]; }

            for (int i = 0; i < chunk.NumMatUnknown3s; i++)
            {
                matSer.MatUnknown3s[i] = new MatUnknown3
                {
                    Unk1 = chunk.MatUnknown3s[i].Unk,
                    Unk2s = new UInt32[chunk.MatUnknown3s[i].Unk2Count],
                    Unk3 = chunk.MatUnknown3s[i].Unk3
                };
                for (int ii = 0; ii < chunk.MatUnknown3s[i].Unk2Count; ii++)
                { matSer.MatUnknown3s[i].Unk2s[ii] = chunk.MatUnknown3Unk2[i][ii]; }
            }

            string jsonString = JsonSerializer.Serialize(matSer);
            sw.Write(jsonString);
        }


    }

}
