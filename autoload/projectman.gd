extends Node

"""
	Project Manager
	
	Handles import/export/save/load
"""

var import_dialog

signal loaded()
signal saving()

func _on_files_dropped(files, _screen):
	# Prevent multiple import dialogs
	if is_instance_valid(import_dialog):
		import_dialog.queue_free()
	var f: String = files[0]
	match f.get_extension():
		"project": # Open project
			ProjectMan.load_project(f)
		"chunk_pc", "g_chunk_pc", "peg_pc", "g_peg_pc": # Import chunkfile
			import_chunk_dialog(f)
			return
		"cts": # Import cts
			return

func import_chunk_dialog(filepath: String = ""):
	if is_instance_valid(import_dialog):
		import_dialog.queue_free()
	var dialog = UiModalImportChunk.new()
	ChunkEditor.ui.add_child(dialog)
	dialog._set_file(filepath)
	import_dialog = dialog

# Load should always go through this
func import_chunk(filepath: String):
	# Load chunk
	Globals.chunk = Chunk.new()
	get_tree().root.get_node("main").add_child(Globals.chunk)
	ChunkHandler.LoadChunk(filepath)
	Globals.chunk._load()
	# Save g_chunk models
	var modelpath = Globals.chunk.working_dir + "/g_chunk"
	var dir = Directory.new();
	if !dir.dir_exists(modelpath):
		dir.make_dir(modelpath)
	for i in range(Globals.chunk.rendermodels.size()):
		ResourceSaver.save(modelpath + "/model_" + str(i).pad_zeros(4) + ".tres", Globals.chunk.rendermodels[i])
	emit_signal("loaded")

func load_project(filepath: String):
	if is_instance_valid(Globals.chunk):
		Globals.chunk.queue_free()
	Globals.chunk = Chunk.new()
	get_tree().root.get_node("main").add_child(Globals.chunk)
	load_saved_g_chunk_models(filepath.get_base_dir() + "/g_chunk")
	Globals.chunk.path_project_file = filepath
	Globals.chunk._load()
	emit_signal("loaded")

func export_chunk():
	emit_signal("saving")
	Globals.chunk._save()

func load_saved_g_chunk_models(modelpath: String):
	var dir = Directory.new()
	dir.open(modelpath)
	print(modelpath)
	dir.list_dir_begin()
	var files = []
	while true:
		var file = dir.get_next()
		if file == "":
			break
		if file.get_extension() == "tres":
			files.append(file)
	dir.list_dir_end()
	files.sort()
	for file in files:
		var res = load(modelpath + "/" + file) as ArrayMesh
		if res == null:
			continue
		Globals.chunk.rendermodels.append(res)
