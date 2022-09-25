using Godot;
using System;
using System.IO;

public class ChunkHandler : Node
{
	ChunkLoader chunkLoader;
	ChunkUnloader chunkUnloader;

	CPUChunk loadedChunk;
	string loadedChunkPath;

	public override void _Ready()
	{
		chunkLoader = GetNode("/root/ChunkLoader") as ChunkLoader;
		chunkUnloader = new ChunkUnloader();
	}

	public void ClearChunk()
	{
		loadedChunk = null;
	}

	public void LoadChunk(string input_filepath)
	{
		string cpu_chunk_filepath = "";
		string gpu_chunk_filepath = "";
		// string peg_filepath;

		GD.Print(input_filepath);

		// File Exist
		if (!System.IO.File.Exists(input_filepath))
		{
			GD.Print("Error: Input File Doesn't Exist! " + input_filepath);
			return;
		}

		// File Extension
		if (System.IO.Path.GetExtension(input_filepath) == ".chunk_pc")
		{
			cpu_chunk_filepath = input_filepath;
			gpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".g_chunk_pc");
		}
		else if (System.IO.Path.GetExtension(input_filepath) == ".g_chunk_pc")
		{
			cpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".chunk_pc");
			gpu_chunk_filepath = input_filepath;
		}
		else if (System.IO.Path.GetExtension(input_filepath) == ".g_peg_pc")
		{
			cpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".chunk_pc");
			gpu_chunk_filepath = System.IO.Path.ChangeExtension(input_filepath, ".g_chunk_pc");
		}
		else
		{
			GD.Print("Error: Unknown extension!");
			return;
		}

		// Chunkfile Exist
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

		loadedChunk = chunkLoader.LoadChunk(cpu_chunk_filepath);
		loadedChunkPath = cpu_chunk_filepath;
		if (loadedChunk != null)
		{
			chunkLoader.LoadGPUChunk(loadedChunk, gpu_chunk_filepath);
			ImportChunkToScene(loadedChunk);
		}
		else
		{
			GD.PushWarning("ChunkLoader returned null.");
		}
	}

	public void ImportChunkToScene(CPUChunk chunk)
	{
		Node world = GetNode("/root/main/chunk/cityobjects");
		for (int i = 0; i < chunk.cityObjectCount; i++)
		{
			CityObject cityObject = chunk.cityObjects[i];
			uint partId = cityObject.cityObjectPart;
			CityObjectPart temp = chunk.cityObjectParts[partId];

			Spatial cityObjectNode = new Spatial();
			cityObjectNode.SetScript(ResourceLoader.Load("res://scenes/editor/scripts/cityobject.gd"));
			cityObjectNode.Translation = temp.pos;
			cityObjectNode.Name = cityObject.name;
			cityObjectNode.Set("rendermodel_id", temp.model);
			cityObjectNode.Set("cityobjpart_id", partId);

			world.AddChild(cityObjectNode);
		}
		Spatial camera = (Spatial)GetNode("/root/main/editor/cameraman");
		camera.Translation = chunk.cityObjectParts[0].pos;
	}

	public void SaveChunk()
	{
		// Get cityobject data from nodes
		Node cityObjContainer = GetNode("/root/main/chunk/cityobjects");
		for (int i = 0; i < cityObjContainer.GetChildCount(); i++)
		{
			Spatial cityObjNode = cityObjContainer.GetChild(i) as Spatial;
			uint model = (uint)(int)cityObjNode.Get("rendermodel_id");
			Vector3 pos = cityObjNode.Translation;

			uint partid = loadedChunk.cityObjects[i].cityObjectPart;
			loadedChunk.cityObjectParts[partid].pos = pos;
			loadedChunk.cityObjectParts[partid].model = model;
		}

		string newChunkPath = System.IO.Path.ChangeExtension(loadedChunkPath, "_new.chunk_pc");
		if (System.IO.File.Exists(newChunkPath))
		{
			System.IO.File.Delete(newChunkPath);
		}
		System.IO.File.Copy(loadedChunkPath, newChunkPath);
		chunkUnloader.PatchChunk(loadedChunk, newChunkPath);

	}
}
