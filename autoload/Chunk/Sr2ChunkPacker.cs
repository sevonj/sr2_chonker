///
/// Sr2ChunkUnpacker.cs - Saints Row 2 Chunk Unpacker
/// 
/// Will unpack the following sections into new files:
/// 
/// - baked collision
/// - matlib
/// - lights
/// 

using System;
using System.IO;
using Kaitai;
using System.Text.Json;

struct Offsets
{
    public int off_texlist;
    public int off_bakedcoll;
    public int off_matlib;
    public int off_lights;
}

public class Sr2ChunkPacker
{
    public void Unpack(string filepath, string dir)
    {
        if (Path.GetExtension(filepath) != ".chunk_pc") throw new InvalidOperationException(filepath + " - File extension is not .chunk_pc");
        Sr2ChunkPc chunk = Sr2ChunkPc.FromFile(filepath);

        string basename = Path.GetFileNameWithoutExtension(filepath);

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        // Create Offset File
        using (StreamWriter sw = File.CreateText(Path.Combine(dir, basename + "_offsets.json")))
        {
            sw.Write(JsonSerializer.Serialize(new Offsets
            {
                off_texlist = 256,
                off_bakedcoll = (int)chunk.OffBakedcoll.Input,
                off_matlib = (int)chunk.OffMatlib.Input,
                off_lights = (int)chunk.OffLights.Input
            }));
        }

        // Copy parts to new files
        using (FileStream fsr = File.OpenRead(filepath))
        {
            BinaryReader br = new BinaryReader(fsr);

            // Header
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".header")))
            {
                int start = 0;
                int len = 256;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }

            // Texture list
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".texlist")))
            {
                int start = 256;
                int len = (int)chunk.OffModelinfo.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }

            // Object Data 0
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".objects0")))
            {
                int start = (int)chunk.OffModelinfo.Input;
                int len = (int)chunk.OffBakedcoll.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }

            // Baked collision
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".bakedcoll")))
            {
                int start = (int)chunk.OffBakedcoll.Input;
                int len = (int)chunk.OffModelbuffers.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }

            // ... //

            // Material Library
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".matlib")))
            {
                int start = (int)chunk.OffMatlib.Input;
                int len = (int)chunk.OffRendermodels.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }

            // Object Data 1
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".objects1")))
            {
                int start = (int)chunk.OffCityobjects.Input;
                int len = (int)chunk.OffUnknownNames.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }
            // ... //

            // Light sources
            using (FileStream fs = File.OpenWrite(Path.Combine(dir, basename + ".lights")))
            {
                int start = (int)chunk.OffLights.Input;
                int len = (int)chunk.OffUnknown33.Input - start;
                BinaryWriter bw = new BinaryWriter(fs);
                fsr.Seek(start, SeekOrigin.Begin);
                bw.Write(br.ReadBytes(len));
            }
        }
        chunk.M_Io.Close();
    }

    public void Pack(string filepath, string dir)
    {
        if (Path.GetExtension(filepath) != ".chunk_pc") throw new InvalidOperationException(filepath + " - File extension is not .chunk_pc");

        string basename = Path.GetFileNameWithoutExtension(filepath);

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        // Read Offset File
        Offsets off = JsonSerializer.Deserialize<Offsets>(File.ReadAllText(Path.Combine(dir, basename + "_offsets.json")));

        // Patch files parts to chunk
        using (FileStream fs = File.OpenWrite(filepath))
        {
            BinaryWriter bw = new BinaryWriter(fs);

            // Texture list
            fs.Seek(off.off_texlist, SeekOrigin.Begin);
            bw.Write(File.ReadAllBytes(Path.Combine(dir, basename + ".texlist")));


            // ... //

            // Baked collision
            fs.Seek(off.off_bakedcoll, SeekOrigin.Begin);
            bw.Write(File.ReadAllBytes(Path.Combine(dir, basename + ".bakedcoll")));

            // ... //

            // Material Library
            fs.Seek(off.off_matlib, SeekOrigin.Begin);
            bw.Write(File.ReadAllBytes(Path.Combine(dir, basename + ".matlib")));

            // ... //

            // Light sources
            fs.Seek(off.off_lights, SeekOrigin.Begin);
            bw.Write(File.ReadAllBytes(Path.Combine(dir, basename + ".lights")));

        }
    }
}
