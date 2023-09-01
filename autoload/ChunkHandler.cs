using Godot;
using System;
using System.IO;
using System.Text.Json;
using Kaitai;

public class ChunkHandler : Node
{
	Sr2ChunkPc loadedChunk;
	string loadedChunkName;
	string loadedChunkPath;

	string path_unpack_dir;
	string path_project_file;
	string path_unpacked_header;
	string path_unpacked_texlist;
	string path_unpacked_objects0;
	string path_upnacked_bakedcoll;
	string path_unpacked_objects1;
	string path_unpacked_matlib;
	string path_unpacked_lights;
	string path_unpacked_objects_json;
	string path_unpacked_mat_json;
	string path_unpacked_lights_json;

	string path_unpack_dir_peg;

	struct ProjectFileJSON
	{
		public int version { get; set; }
		public bool has_textures { get; set; }
		public string name { get; set; }
		public string path_original_chunkfile { get; set; }
		public string path_unpacked_mat_json { get; set; }
		public string path_unpacked_lights_json { get; set; }
		public string path_unpacked_objects_json { get; set; }
	}

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

		Sr2ChunkPc chunk = Sr2ChunkPc.FromFile(cpu_chunk_filepath);

		GD.Print("Chunk parsed");
		loadedChunk = chunk;
		loadedChunkName = System.IO.Path.GetFileNameWithoutExtension(cpu_chunk_filepath);
		loadedChunkPath = cpu_chunk_filepath;

		Node chunkEditor = GetNode("/root/ChunkEditor");
		if ((bool)chunkEditor.Get("opt_unpack"))
		{
			Unpack(cpu_chunk_filepath);
		}

		//ImportChunkToScene(ref loadedChunk, gpu_chunk_filepath);
		ImportRendermodels(ref loadedChunk, gpu_chunk_filepath);
	}

	void ImportRendermodels(ref Sr2ChunkPc chunk, string gpu_chunk_filepath)
	{

		Node chunkEditor = GetNode("/root/ChunkEditor");
		Node chunkNode = (Node)GetNode("/root/Globals").Get("chunk");
		if ((bool)chunkEditor.Get("opt_rendermodels"))
		{
			Sr2GpuChunkLoader gLoader = new Sr2GpuChunkLoader();
			Mesh[] chunk_rendermodels = gLoader.LoadMeshesFromChunk(ref chunk, gpu_chunk_filepath);
			//GD.Print("meshes len: ", chunk_rendermodels.Length);
			for (int i = 0; i < chunk.NumRendermodels; i++)
			{
				chunk_rendermodels[i] = chunk_rendermodels[i];
				//Mesh mesh = chunk_rendermodels[i];
				//chunkEditor.Call("_add_chunk_rendermodel", mesh);
			}
			chunkNode.Set("rendermodels", chunk_rendermodels);

		}
		// No Rendermodels, empty meshes instead.
		else
		{
			Mesh[] chunk_rendermodels = new Mesh[chunk.NumRendermodels];
			SurfaceTool st = new SurfaceTool();
			st.Begin(Mesh.PrimitiveType.Triangles);
			for (int i = 0; i < chunk.NumRendermodels; i++)
			{
				chunk_rendermodels[i] = st.Commit();
				//chunkEditor.Call("_add_chunk_rendermodel", st.Commit());
			}
			chunkNode.Set("rendermodels", chunk_rendermodels);
		}
	}

	/*
	void ImportChunkToScene(ref Sr2ChunkPc chunk, string gpu_chunk_filepath)
	{
		Node chunkEditor = GetNode("/root/ChunkEditor");
		Node chunkNode = (Node)GetNode("/root/Globals").Get("chunk");
		if ((bool)chunkEditor.Get("opt_unpack"))
		{
			chunkNode.Set("path_unpacked_mat_json", path_unpacked_mat_json);
			chunkNode.Set("path_unpack_dir", path_unpack_dir);
		}
		chunkEditor.Set("is_chunk_loaded", true);
		// Load Rendermodels
		if ((bool)chunkEditor.Get("opt_rendermodels"))
		{
			Sr2GpuChunkLoader gLoader = new Sr2GpuChunkLoader();
			Mesh[] chunk_rendermodels = gLoader.LoadMeshesFromChunk(ref chunk, gpu_chunk_filepath);
			//GD.Print("meshes len: ", chunk_rendermodels.Length);
			for (int i = 0; i < chunk.NumRendermodels; i++)
			{
				chunk_rendermodels[i] = chunk_rendermodels[i];
				//Mesh mesh = chunk_rendermodels[i];
				//chunkEditor.Call("_add_chunk_rendermodel", mesh);
			}
			chunkNode.Set("rendermodels", chunk_rendermodels);

		}
		// No Rendermodels, empty meshes instead.
		else
		{
			Mesh[] chunk_rendermodels = new Mesh[chunk.NumRendermodels];
			SurfaceTool st = new SurfaceTool();
			st.Begin(Mesh.PrimitiveType.Triangles);
			for (int i = 0; i < chunk.NumRendermodels; i++)
			{
				chunk_rendermodels[i] = st.Commit();
				//chunkEditor.Call("_add_chunk_rendermodel", st.Commit());
			}
			chunkNode.Set("rendermodels", chunk_rendermodels);
		}
		var loaded_cobjs = new Godot.Collections.Array<Node>();
		for (int i = 0; i < chunk.NumCityobjects; i++)
		{
			Sr2ChunkPc.Cityobject cobj = chunk.Cityobjects[i];
			int partId = (int)cobj.CityobjectPartId;
			Sr2ChunkPc.CityobjectPart temp = chunk.CityobjectParts[partId];
			Spatial cityObjectNode = new Spatial();
			cityObjectNode.SetScript(ResourceLoader.Load("res://scenes/editor/scripts/cityobject.gd"));
			cityObjectNode.Set("uid", "cobj_" + i);
			cityObjectNode.Translation = new Vector3(-temp.Pos.X, temp.Pos.Y, temp.Pos.Z);
			cityObjectNode.Name = chunk.CityobjectNames[i];
			cityObjectNode.Set("rendermodel_id", temp.RendermodelId);
			cityObjectNode.Set("cityobjpart_id", partId);
			chunkNode.AddChild(cityObjectNode);
			loaded_cobjs.Add(cityObjectNode);
			// Has to be done in this roundabout way, cannot set transform via csharp?
			cityObjectNode.Call("_set_basis", new Basis(
				new Vector3(temp.BasisX.X, -temp.BasisX.Y, -temp.BasisX.Z),
				new Vector3(-temp.BasisY.X, temp.BasisY.Y, temp.BasisY.Z),
				new Vector3(-temp.BasisZ.X, temp.BasisZ.Y, temp.BasisZ.Z)));
		}
		chunkNode.Set("cityobjects", loaded_cobjs);

		var loaded_lights = new Godot.Collections.Array<Node>();
		for (int i = 0; i < chunk.NumLights.Result; i++)
		{
			Sr2ChunkPc.LightDataType light = chunk.LightData[i];
			string name = chunk.LightNames[i];
			Vector3 pos = new Vector3(-light.Position.X, light.Position.Y, light.Position.Z);
			Color col = new Color(light.R, light.G, light.B);

			Spatial lightNode = new Spatial();
			lightNode.Name = name;
			lightNode.SetScript(ResourceLoader.Load("res://scenes/editor/scripts/lightsource.gd"));
			lightNode.Set("uid", "light_" + i);
			Godot.Collections.Array flags = new Godot.Collections.Array();
			flags.Add(light.Bitflag0);
			flags.Add(light.Bitflag1);
			flags.Add(light.Bitflag2);
			flags.Add(light.Bitflag3);
			flags.Add(light.Bitflag4);
			flags.Add(light.Bitflag8);
			flags.Add(light.Bitflag10);
			flags.Add(light.ShadowCharacter);
			flags.Add(light.ShadowLevel);
			flags.Add(light.LightCharacter);
			flags.Add(light.LightLevel);
			flags.Add(light.Bitflag22);
			lightNode.Set("flags", flags);
			lightNode.Set("color", col);
			lightNode.Set("unk10", light.Unk10);
			lightNode.Translation = pos;
			lightNode.Set("type", light.Type);
			lightNode.Set("radius_inner", light.RadiusInner);
			lightNode.Set("radius_outer", light.RadiusOuter);
			lightNode.Set("render_dist", light.RenderDist);
			lightNode.Set("parent", light.ParentCityobject);
			chunkNode.AddChild(lightNode);
			loaded_lights.Add(lightNode);
			// Has to be done in this roundabout way, cannot set transform via csharp?
			lightNode.Call("_set_basis", new Basis(
				new Vector3(light.BasisX.X, -light.BasisX.Y, -light.BasisX.Z),
				new Vector3(-light.BasisY.X, light.BasisY.Y, light.BasisY.Z),
				new Vector3(-light.BasisZ.X, light.BasisZ.Y, light.BasisZ.Z)));

		}
		chunkNode.Set("lights", loaded_lights);

		BakedCollision bakedColLoader = new BakedCollision();
		MeshInstance bakedColNode = bakedColLoader.LoadBakedCollision(ref chunk);
		chunkNode.AddChild(bakedColNode);
		chunkNode.Set("baked_collision", bakedColNode);

		chunk.M_Io.Close();

		Spatial camera = (Spatial)GetNode("/root/main/editor/cameraman");
		camera.Translation = new Vector3(
			-loadedChunk.CityobjectParts[0].Pos.X,
			loadedChunk.CityobjectParts[0].Pos.Y,
			loadedChunk.CityobjectParts[0].Pos.Z
		);
		chunkEditor.Call("_update");
	}
	*/
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

	// Unpack and create project file
	public void Unpack(string cpu_chunk_filepath)
	{
		string basename = System.IO.Path.GetFileNameWithoutExtension(cpu_chunk_filepath);
		string filename = System.IO.Path.GetFileName(cpu_chunk_filepath);
		path_unpack_dir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(cpu_chunk_filepath), basename + "_unpacked");

		// Unpack Chunk
		Sr2ChunkPacker unpacker = new Sr2ChunkPacker();
		unpacker.Unpack(cpu_chunk_filepath, path_unpack_dir);

		path_unpacked_header = System.IO.Path.Combine(path_unpack_dir, basename + ".header");
		path_unpacked_objects0 = System.IO.Path.Combine(path_unpack_dir, basename + ".objects0");
		path_unpacked_objects1 = System.IO.Path.Combine(path_unpack_dir, basename + ".objects1");
		path_unpacked_texlist = System.IO.Path.Combine(path_unpack_dir, basename + ".texlist");
		path_unpacked_matlib = System.IO.Path.Combine(path_unpack_dir, basename + ".matlib");
		path_unpacked_lights = System.IO.Path.Combine(path_unpack_dir, basename + ".lights");

		// Unpack Peg
		string path_pegfile = System.IO.Path.ChangeExtension(cpu_chunk_filepath, ".peg_pc");
		path_unpack_dir_peg = System.IO.Path.Combine(path_unpack_dir, "textures");
		Sr2PegPacker pegger = new Sr2PegPacker();
		pegger.Unpack(path_pegfile, path_unpack_dir_peg);

		// Convert Objects
		Sr2ObjectDataConv objsConv = new Sr2ObjectDataConv();
		path_unpacked_objects_json = System.IO.Path.Combine(path_unpack_dir, basename + "_objects.json");
		objsConv.BinToJSON(
			path_unpacked_header,
			path_unpacked_objects0,
			path_unpacked_objects1,
			path_unpacked_objects_json
			);

		// Convert Materials
		Sr2MatlibConv matconv = new Sr2MatlibConv();
		path_unpacked_mat_json = System.IO.Path.Combine(path_unpack_dir, basename + "_materials.json");
		matconv.BinToJSON(
			path_unpacked_matlib,
			path_unpacked_texlist,
			path_unpacked_mat_json
			);

		// Convert Lights
		Sr2LightsConv lightsConv = new Sr2LightsConv();
		path_unpacked_lights_json = System.IO.Path.Combine(path_unpack_dir, basename + "_lights.json");
		lightsConv.BinToJSON(
			path_unpacked_lights,
			path_unpacked_lights_json
			);

		CreateHelpFiles(path_unpack_dir);

		// Create Project File
		path_project_file = System.IO.Path.Combine(path_unpack_dir, basename + ".project");

		using (StreamWriter sw = System.IO.File.CreateText(path_project_file))
		{
			ProjectFileJSON projectFile = new ProjectFileJSON
			{
				version = 0,
				has_textures = false,
				name = basename,
				path_original_chunkfile = cpu_chunk_filepath,
				path_unpacked_objects_json = basename + "_objects.json",
				path_unpacked_mat_json = basename + "_materials.json",
				path_unpacked_lights_json = basename + "_lights.json"
			};

			sw.Write(JsonSerializer.Serialize(projectFile));
		}



		Node chunkNode = (Node)GetNode("/root/Globals").Get("chunk");
		chunkNode.Set("path_project_file", path_project_file);
	}

	public void Repack(string cpu_chunk_filepath)
	{
		string basename = System.IO.Path.GetFileNameWithoutExtension(cpu_chunk_filepath);
		string filename = System.IO.Path.GetFileName(cpu_chunk_filepath);

		// Convert Material
		Sr2MatlibConv matconv = new Sr2MatlibConv();
		matconv.JSONToBin(
			path_unpacked_matlib,
			path_unpacked_texlist,
			path_unpacked_mat_json
			);

		// Unpack
		//Sr2ChunkUnpacker unpacker = new Sr2ChunkUnpacker();
		//unpacker.Unpack(cpu_chunk_filepath, unpack_dir);
	}

	private static void CreateHelpFiles(string dir)
	{
		using (StreamWriter sw = System.IO.File.CreateText(System.IO.Path.Combine(dir, "materials.txt")))
		{
			sw.WriteLine("Materials");
			sw.WriteLine();
			sw.WriteLine("Materials JSON file contains all data in .texlist and .matlib files, and can be compiled back into them.");
			sw.WriteLine("https://www.saintsrowmods.com/forum/threads/sr2-material-doc.20337/");
			sw.WriteLine("JSON structure follows this with the exception that I nested any possible struct inside its parent for readability.");
		}
	}
}
