using Godot;
using System;
using System.IO;

public class ChunkUnloader : Node
{
    public void PatchChunk(CPUChunk chunk, string filepath)
    {
        GD.Print(filepath);

        if (!System.IO.File.Exists(filepath))
        {
            GD.PushWarning("ChunkLoader.LoadChunk(): File doesn't exist! " + filepath);
            return;
        }

        using (FileStream fs = System.IO.File.OpenWrite(filepath))
        {
            BinaryWriter bw = new BinaryWriter(fs);


            // --- City Object Part --- //
            fs.Seek(chunk.cityObjectPartOff, 0);

            // 96B
            for (int i = 0; i < chunk.cityObjectPartCount; i++)
            {
                CityObjectPart part = chunk.cityObjectParts[i];

                bw.Write(-(Single)part.pos.x);
                bw.Write((Single)part.pos.y);
                bw.Write((Single)part.pos.z);
                fs.Seek(76, SeekOrigin.Current);
                bw.Write((UInt32)part.model);
                fs.Seek(4, SeekOrigin.Current);
            }

            // City Objects
            fs.Seek(chunk.cityObjectOff, 0);

            // 80B
            for (int i = 0; i < chunk.cityObjectCount; i++)
            {
                CityObject cobj = chunk.cityObjects[i];
                bw.Write((Single)cobj.cull_max.x);
                bw.Write((Single)cobj.cull_max.y);
                bw.Write((Single)cobj.cull_max.z);
                fs.Seek(4, SeekOrigin.Current);
                bw.Write((Single)cobj.cull_min.x);
                bw.Write((Single)cobj.cull_min.y);
                bw.Write((Single)cobj.cull_min.z);
                bw.Write((Single)cobj.cull_distance);
                fs.Seek(16, SeekOrigin.Current);
                bw.Write((UInt32)cobj.flags);
                bw.Write((UInt32)cobj.unk1);
                fs.Seek(8, SeekOrigin.Current);
                bw.Write((UInt32)cobj.cityObjectPart);
                fs.Seek(12, SeekOrigin.Current);
            }

            return;
        }
    }
}
