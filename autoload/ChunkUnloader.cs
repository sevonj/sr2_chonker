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
			fs.Seek(chunk.LightsOffset.Off + 4, 0);
			for (int i = 0; i < chunk.LightCount; i++)
			{
				Sr2CpuChunkPc.Light light = chunk.LightSections.Lights[i];
				// Get Cityobject Node data
				Spatial lightNode = (Spatial)GetNode("/root/main/chunk/lights").GetChild(i);

				// Construct a bit flag int from array of bools.
				// I am sure this could've been done with much less effort.
				int flags = 0;
				Godot.Collections.Array flags_arr = (Godot.Collections.Array)lightNode.Get("flags");

				for (int j = 0; j < 8; j++)
					if ((bool)flags_arr[7 - j])
						flags |= 1 << 7 - j;
				flags <<= 8;

				for (int j = 0; j < 8; j++)
					if ((bool)flags_arr[15 - j])
						flags |= 1 << 7 - j;
				flags <<= 8;

				for (int j = 0; j < 8; j++)
					if ((bool)flags_arr[23 - j])
						flags |= 1 << 7 - j;
				flags <<= 8;

				for (int j = 0; j < 8; j++)
					if ((bool)flags_arr[31 - j])
						flags |= 1 << 7 - j;


				Color col = (Color)lightNode.Get("color");

				uint unk10 = (uint)(int)lightNode.Get("unk10");

				float x = lightNode.Transform.origin.x;
				float y = lightNode.Transform.origin.y;
				float z = lightNode.Transform.origin.z;

				float radius_inner = (float)lightNode.Get("radius_inner");
				float radius_outer = (float)lightNode.Get("radius_outer");
				float render_dist = (float)lightNode.Get("render_dist");

				bw.Write(flags);

				fs.Seek(4, SeekOrigin.Current);

				bw.Write(col.r);
				bw.Write(col.g);
				bw.Write(col.b);

				fs.Seek(20, SeekOrigin.Current);

				bw.Write(unk10);

				fs.Seek(16, SeekOrigin.Current);

				bw.Write(-x);
				bw.Write(y);
				bw.Write(z);

				fs.Seek(44, SeekOrigin.Current);

				bw.Write(radius_inner);
				bw.Write(radius_outer);
				bw.Write(render_dist);

				fs.Seek(20, SeekOrigin.Current);

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
