using Godot;
using System;
using System.IO;
using Kaitai;
using File = System.IO.File;

public class PhysModels : Node
{
    public MeshInstance LoadCollisionModels(ref Sr2ChunkPc chunk)
    {
        GD.Print("Importing collision model");

        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.LineStrip);

        //GD.Print("vertex count:", chunk.NumUnkWorldpositions);
        for (int i = 0; i < chunk.NumBakedCollisionVertices; i++)
        {
            st.AddVertex(new Vector3(
                -chunk.BakedCollisionVertices[i].X,
                chunk.BakedCollisionVertices[i].Y,
                chunk.BakedCollisionVertices[i].Z));
        }

        //for (int i = 0; i < chunk.NumUnknown6s - 2; i++)
        //{
        //    int i0 = index(chunk.Unknown6s[i]);
        //    int i1 = index(chunk.Unknown6s[i + 1]);
        //    int i2 = index(chunk.Unknown6s[i + 2]);
        //    if (i0 == -1 || i0 == -1 || i1 == -1)
        //    {
        //        continue;
        //    }
        //    st.AddIndex((int)i0);
        //    st.AddIndex((int)i1);
        //    st.AddIndex((int)i2);
        //}

        MeshInstance meshInstance = new MeshInstance();
        meshInstance.Mesh = st.Commit();
        meshInstance.Name = "baked_collision";
        return meshInstance;
    }

    int index(byte[] index)
    {
        byte[] bytes = new byte[4];
        index.CopyTo(bytes, 0);
        return BitConverter.ToInt32(bytes, 0);
    }

    public void Unpack(ref Sr2ChunkPc chunk, string dir)
    {
        string subdir = System.IO.Path.Combine(dir, "coll");

        System.IO.Directory.CreateDirectory(dir);
        string filename_txt = System.IO.Path.Combine(dir, "baked_collision.txt");
        using (StreamWriter sw = File.CreateText(filename_txt))
        {
            sw.WriteLine("Baked collision model.");
            sw.WriteLine();
            sw.WriteLine("Files explained in the order they appear in chunkfile:");
            sw.WriteLine("Vertex buffer: UInt32 vertexcount, floats XYZ per vertex. X coord is inverted like seemingly everywhere else in this game.");
            sw.WriteLine("Unk0: UInt32 count, 3-byte data");
            sw.WriteLine("Unk1: UInt32 count, 4-byte data");
            sw.WriteLine("Unk2: UInt32 count, 12-byte data");
            sw.WriteLine("MOPP: Look up Havok MOPP format. Seems common in 2000s games. The other files are probably also havok.");
        }
        string filename_unk0 = System.IO.Path.Combine(dir, "baked_collision.unk0");
        using (BinaryWriter bw = new BinaryWriter(File.Open(filename_unk0, FileMode.Create)))
        {
            bw.Write((UInt32)chunk.NumBakedCollisionUnk0);
            bw.Write(chunk.BakedCollisionUnk0);
        }
        
    }

}
