using Godot;
using System;
using System.IO;
using Kaitai;

//*
public class ChunkUnloader : Node
{
	public void PatchChunk(Sr2CpuChunkPc chunk, string filepath)
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


			// --- City Object Parts --- //
			// 96B
			for (int i = 0; i < chunk.CityobjectCount; i++)
			{
				// Get Cityobject Node data
				Spatial cobjNode = (Spatial)GetNode("/root/main/chunk/cityobjects").GetChild(i);

				float x = cobjNode.Transform.origin.x;
				float y = cobjNode.Transform.origin.y;
				float z = cobjNode.Transform.origin.z;
				uint modelId = (uint)(int)cobjNode.Get("rendermodel_id");

				// Find out which CityobjectPart belongs to this Cityobject and seek there.
				uint cobjPartId = chunk.Cityobjects[i].CityobjectPartId;
				fs.Seek(chunk.CityobjectPartsOffset.Off + cobjPartId * 96, 0);

				bw.Write(-(Single)x);
				bw.Write((Single)y);
				bw.Write((Single)z);
				fs.Seek(76, SeekOrigin.Current);
				bw.Write((UInt32)modelId);
				fs.Seek(4, SeekOrigin.Current);
			}

			// --- Lights --- //
			fs.Seek(chunk.LightsOffset.Off, 0);
			for (int i = 0; i < chunk.LightCount; i++)
			{
				// Get Cityobject Node data
				Spatial lightNode = (Spatial)GetNode("/root/main/chunk/lights").GetChild(i);

				Color col = (Color)lightNode.Get("color");

				col.r = 0;
				col.g = 1;
				col.b = 1;

				float x = lightNode.Transform.origin.x;
				float y = lightNode.Transform.origin.y;
				float z = lightNode.Transform.origin.z;

				fs.Seek(8, SeekOrigin.Current);

				bw.Write((Single)col.r);
				bw.Write((Single)col.g);
				bw.Write((Single)col.b);

				fs.Seek(40, SeekOrigin.Current);

				bw.Write(-(Single)x);
				bw.Write((Single)y);
				bw.Write((Single)z);
				
				fs.Seek(76, SeekOrigin.Current);
			}
			// City Objects
			/*
			fs.Seek(chunk.CityobjectPartsOffset.Off, 0);

			// 80B
			for (int i = 0; i < chunk.CityobjectCount; i++)
			{
				Chunk_ cobj = chunk.Cityobjects[i];
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
			}//*/

			return;
		}
	}
}
//*/
