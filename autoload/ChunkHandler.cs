using Godot;
using System;
using System.IO;
using Kaitai;

public class ChunkHandler : Node
{
	Sr2CpuChunkPc loadedChunk;
	string loadedChunkName;
	string loadedChunkPath;

	/// Should only be called by ChunkEditor.
	/// Clears any variables in ChunkHandler.
	public void OnClearChunk()
	{
		loadedChunk = null;
		loadedChunkName = null;
		loadedChunkPath = null;
	}

	/// Should only be called by ChunkEditor.
	/// LoadChunk checks filepaths, sets a new loadedChunk, and then calls ImportChunkToScene.
	/// It recognizes any of the chunk file types (.chunk_pc, .g_chunk_pc, .g_peg_pc)
	public void LoadChunk(string input_filepath)
	{
		string cpu_chunk_filepath = "";
		string gpu_chunk_filepath = "";
		string peg_filepath = "";

		// Get all filepaths
		if (System.IO.Path.GetExtension(input_filepath) == ".chunk_pc" ||
			System.IO.Path.GetExtension(input_filepath) == ".g_chunk_pc" ||
			System.IO.Path.GetExtension(input_filepath) == ".g_peg_pc")
		{
			cpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".chunk_pc");
			gpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".g_chunk_pc");
			peg_filepath = System.IO.Path.ChangeExtension(input_filepath, ".g_peg_pc");
		}
		else
		{
			GD.Print("Error: Unknown file extension!");
			return;
		}

		// Sanity check for if files exist
		if (!System.IO.File.Exists(cpu_chunk_filepath))
		{
			GD.Print("Error: " + cpu_chunk_filepath + " doesn't exist!");
			return;
		}
		if (!System.IO.File.Exists(gpu_chunk_filepath))
		{
			GD.Print("Error: " + gpu_chunk_filepath + " doesn't exist!");
			return;
		}

		Sr2CpuChunkPc chunk = Sr2CpuChunkPc.FromFile(cpu_chunk_filepath);

		GD.Print("Chunk parsed");
		loadedChunk = chunk;
		loadedChunkName = System.IO.Path.GetFileNameWithoutExtension(cpu_chunk_filepath);
		loadedChunkPath = cpu_chunk_filepath;
		ImportChunkToScene(loadedChunk, gpu_chunk_filepath);
	}

	void ImportChunkToScene(Sr2CpuChunkPc chunk, string gpu_chunk_filepath)
	{
		Node chunkEditor = GetNode("/root/ChunkEditor");
		Node world = GetNode("/root/main/chunk/cityobjects");
		Node lights = GetNode("/root/main/chunk/lights");

		chunkEditor.Set("is_chunk_loaded", true);

		Sr2GpuChunkLoader gLoader = new Sr2GpuChunkLoader();
		Mesh[] chunk_rendermodels = gLoader.LoadMeshesFromChunk(chunk, gpu_chunk_filepath);
		GD.Print("meshes len: ", chunk_rendermodels.Length);
		for (int i = 0; i < chunk.RendermodelCount; i++)
		{
			Mesh mesh = chunk_rendermodels[i];
			chunkEditor.Call("_add_chunk_rendermodel", mesh);
		}

		for (int i = 0; i < chunk.CityobjectCount; i++)
		{
			Sr2CpuChunkPc.Cityobject cobj = chunk.Cityobjects[i];
			int partId = (int)cobj.CityobjectPartId;
			Sr2CpuChunkPc.CityobjectPart temp = chunk.CityobjectParts[partId];

			Spatial cityObjectNode = new Spatial();
			cityObjectNode.SetScript(ResourceLoader.Load("res://scenes/editor/scripts/cityobject.gd"));
			cityObjectNode.Translation = new Vector3(-temp.Pos.X, temp.Pos.Y, temp.Pos.Z);
			cityObjectNode.Name = chunk.CityobjectNames[i];
			cityObjectNode.Set("rendermodel_id", temp.RendermodelId);
			cityObjectNode.Set("cityobjpart_id", partId);

			world.AddChild(cityObjectNode);
		}

		for (int i = 0; i < chunk.LightCount; i++)
		{
			if (chunk.LightCount == 1212891981) break; // Duct tape solution for MCKH (this means chunk has no lights)

			Sr2CpuChunkPc.Light light = chunk.LightSections.Lights[i];
			string name = chunk.LightSections.LightNames[i];
			Vector3 pos = new Vector3(-light.Pos.X, light.Pos.Y, light.Pos.Z);
			Color col = new Color(light.R, light.G, light.B);

			Spatial lightNode = new Spatial();
			lightNode.Name = name;
			lightNode.SetScript(ResourceLoader.Load("res://scenes/editor/scripts/lightsource.gd"));

			Godot.Collections.Array flags = new Godot.Collections.Array();
			flags.Add(light.Flags.Bit00);
			flags.Add(light.Flags.Bit01);
			flags.Add(light.Flags.Bit02);
			flags.Add(light.Flags.Bit03);
			flags.Add(light.Flags.Bit04);
			flags.Add(light.Flags.Bit05);
			flags.Add(light.Flags.Bit06);
			flags.Add(light.Flags.Bit07);
			flags.Add(light.Flags.Bit08);
			flags.Add(light.Flags.Bit09);
			flags.Add(light.Flags.Bit0a);
			flags.Add(light.Flags.Bit0b);
			flags.Add(light.Flags.Bit0c);
			flags.Add(light.Flags.Bit0d);
			flags.Add(light.Flags.Bit0e);
			flags.Add(light.Flags.Bit0f);
			flags.Add(light.Flags.Bit10);
			flags.Add(light.Flags.Bit11);
			flags.Add(light.Flags.CastShadowsOnWorld);
			flags.Add(light.Flags.CastShadowsOnPeople);
			flags.Add(light.Flags.Bit14);
			flags.Add(light.Flags.Bit15);
			flags.Add(light.Flags.Bit16);
			flags.Add(light.Flags.Bit17);
			flags.Add(light.Flags.Bit18);
			flags.Add(light.Flags.Bit19);
			flags.Add(light.Flags.Bit1a);
			flags.Add(light.Flags.Bit1b);
			flags.Add(light.Flags.Bit1c);
			flags.Add(light.Flags.Bit1d);
			flags.Add(light.Flags.Bit1e);
			flags.Add(light.Flags.Bit1f);

			lightNode.Set("flags", flags);

			lightNode.Set("color", col);

			lightNode.Set("unk10", light.Unk10);
			lightNode.Translation = pos;
			lightNode.Set("radius_inner", light.RadiusInner);
			lightNode.Set("radius_outer", light.RadiusOuter);
			lightNode.Set("render_dist", light.RenderDist);


			lights.AddChild(lightNode);

		}

		Spatial camera = (Spatial)GetNode("/root/main/editor/cameraman");
		camera.Translation = new Vector3(
			-loadedChunk.CityobjectParts[0].Pos.X,
			loadedChunk.CityobjectParts[0].Pos.Y,
			loadedChunk.CityobjectParts[0].Pos.Z
		);

		chunkEditor.Call("_update");
	}

	MeshInstance rendermodel2Mesh()
	{
		MeshInstance meshInstance = new MeshInstance();
		return (meshInstance);
	}
	//*
	public void SaveChunk()
	{
		// Make a copy of the chunk
		string newChunkPath = System.IO.Path.ChangeExtension(loadedChunkPath, "_new.chunk_pc");
		if (System.IO.File.Exists(newChunkPath)) { System.IO.File.Delete(newChunkPath); }
		System.IO.File.Copy(loadedChunkPath, newChunkPath);

		// Patch the new chunk
		ChunkUnloader unloader = new ChunkUnloader();
		AddChild(unloader);
		unloader.PatchChunk(loadedChunk, newChunkPath);
		unloader.QueueFree();
	}
}
