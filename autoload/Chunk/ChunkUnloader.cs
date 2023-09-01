using Godot;
using System;
using System.IO;
using Kaitai;

//*
public class ChunkUnloader : Node
{
	public void PatchChunk(Sr2ChunkPc chunk, string filepath)
	{
		GD.Print(filepath);

		if (!System.IO.File.Exists(filepath))
		{
			GD.PushWarning("ChunkLoader.LoadChunk(): File doesn't exist! " + filepath);
			return;
		}
		Node chunkEditor = GetNode("/root/ChunkEditor");
		using (FileStream fs = System.IO.File.OpenWrite(filepath))
		{
			BinaryWriter bw = new BinaryWriter(fs);
			// --- City Object Parts --- //
			// 96B
			Node chunkNode = (Node)GetNode("/root/Globals").Get("chunk");
			Godot.Collections.Array cobjs = (Godot.Collections.Array)chunkNode.Get("cityobjects");
			for (int i = 0; i < chunk.NumCityobjects; i++)
			{
				Spatial cobjNode = (Spatial)cobjs[i];//(Spatial)GetNode("/root/main/chunk/cityobjects").GetChild(i);

				// Find out which CityobjectPart belongs to this Cityobject and seek there.
				uint cobjPartId = chunk.Cityobjects[i].CityobjectPartId;
				fs.Seek(chunk.OffCityobjectparts.Input + cobjPartId * 96, 0);

				// Transform
				bw.Write(-(Single)cobjNode.Transform.origin.x);
				bw.Write((Single)cobjNode.Transform.origin.y);
				bw.Write((Single)cobjNode.Transform.origin.z);
				bw.Write((Single)cobjNode.Transform.basis.x.x);
				bw.Write(-(Single)cobjNode.Transform.basis.x.y);
				bw.Write(-(Single)cobjNode.Transform.basis.x.z);
				bw.Write(-(Single)cobjNode.Transform.basis.y.x);
				bw.Write((Single)cobjNode.Transform.basis.y.y);
				bw.Write((Single)cobjNode.Transform.basis.y.z);
				bw.Write(-(Single)cobjNode.Transform.basis.z.x);
				bw.Write((Single)cobjNode.Transform.basis.z.y);
				bw.Write((Single)cobjNode.Transform.basis.z.z);
				fs.Seek(40, SeekOrigin.Current);

				// Rendermodel
				bw.Write((UInt32)(int)cobjNode.Get("rendermodel_id"));
				fs.Seek(4, SeekOrigin.Current);
			}

			// --- Lights --- //
			fs.Seek(chunk.OffLights.Input + 4, 0);

			Godot.Collections.Array lights = (Godot.Collections.Array)chunkNode.Get("lights");
			for (int i = 0; i < chunk.NumLights.Result; i++)
			{
				Sr2ChunkPc.LightDataType light = chunk.LightData[i];
				Spatial lightNode = (Spatial)lights[i];//(Spatial)GetNode("/root/main/chunk/lights").GetChild(i);

				//GD.Print("Saving lightnode", lightNode.Name);

				int flags = 0;
				Godot.Collections.Array flags_arr = (Godot.Collections.Array)lightNode.Get("flags");
				flags = (bool)flags_arr[0x0] == true ? flags | (128) : flags; // bitflag0
				flags = (bool)flags_arr[0x1] == true ? flags | (64) : flags; // bitflag1
				flags = (bool)flags_arr[0x2] == true ? flags | (32) : flags; // bitflag2
				flags = (bool)flags_arr[0x3] == true ? flags | (16) : flags; // bitflag3
				flags = (bool)flags_arr[0x4] == true ? flags | (8) : flags; // bitflag4
				flags = (bool)flags_arr[0x5] == true ? flags | (32768) : flags; // bitflag8
				flags = (bool)flags_arr[0x6] == true ? flags | (8192) : flags; // bitflag10
				flags = (bool)flags_arr[0x7] == true ? flags | (2048) : flags; // shadow_character
				flags = (bool)flags_arr[0x8] == true ? flags | (1024) : flags; // shadow_level
				flags = (bool)flags_arr[0x9] == true ? flags | (512) : flags; // light_character
				flags = (bool)flags_arr[0xa] == true ? flags | (256) : flags; // light_level
				flags = (bool)flags_arr[0xb] == true ? flags | (131072) : flags; // bitflag22

				Color col = (Color)lightNode.Get("color");

				bw.Write(flags);
				bw.Write((UInt32)(0));
				bw.Write(col.r);
				bw.Write(col.g);
				bw.Write(col.b);
				fs.Seek(16, SeekOrigin.Current);
				bw.Write((Int32)(-1));
				bw.Write((uint)(int)lightNode.Get("unk10"));
				bw.Write((Int32)(-1));
				fs.Seek(12, SeekOrigin.Current);
				bw.Write(-lightNode.Transform.origin.x);
				bw.Write(lightNode.Transform.origin.y);
				bw.Write(lightNode.Transform.origin.z);
				bw.Write((Single)lightNode.Transform.basis.x.x);
				bw.Write(-(Single)lightNode.Transform.basis.x.y);
				bw.Write(-(Single)lightNode.Transform.basis.x.z);
				bw.Write(-(Single)lightNode.Transform.basis.y.x);
				bw.Write((Single)lightNode.Transform.basis.y.y);
				bw.Write((Single)lightNode.Transform.basis.y.z);
				bw.Write(-(Single)lightNode.Transform.basis.z.x);
				bw.Write((Single)lightNode.Transform.basis.z.y);
				bw.Write((Single)lightNode.Transform.basis.z.z);
				fs.Seek(8, SeekOrigin.Current);
				bw.Write((float)lightNode.Get("radius_inner"));
				bw.Write((float)lightNode.Get("radius_outer"));
				bw.Write((float)lightNode.Get("render_dist"));
				bw.Write((Int32)(-1));
				bw.Write((UInt32)(int)lightNode.Get("parent"));
				fs.Seek(8, SeekOrigin.Current);
				bw.Write((UInt32)(int)lightNode.Get("type"));
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
