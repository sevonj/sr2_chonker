# --- ChunkEditor.gd --- #
# Handles editor functions and contains data used by nodes.
#
#
extends Node

var is_chunk_loaded = false
var opt_cityobjects = true
var opt_rendermodels = true
var opt_lights = true

var cam
var gizmo
var menu_selected_title
var menu_selected_cityobj
var menu_selected_light
var menu_selector

var currently_selected: Spatial
var chunk_rendermodels = []

var file_import_dialog = preload("res://scenes/editor/scripts/import_dialog.gd")
var ui

func _ready():
	get_tree().connect("files_dropped", self, "_on_files_dropped")

# Click empty space unselects object.
func _unhandled_input(event):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			_unselect()

# Load a dropped file.
func _on_files_dropped(files, _screen):
	if len(files) != 1:
		print("One file at a time!")
		return
	var dialog = file_import_dialog.new()
	ui.add_child(dialog)
	dialog._set_file(files[0])
#	var fext = files[0].get_extension()
#	if fext == "chunk_pc" or fext == "g_chunk_pc" or fext == "g_peg_pc":
#		Globals.on_clear_chunkfile_to_load = files[0]
#		_clear()
	#else: print("Unknown file extension: " + fext)

# On Clear / Reload
func _on_main_ready():
	ui = get_tree().root.get_node("main").get_node("ui")
	if Globals.on_clear_chunkfile_to_load:
		_load_chunk(Globals.on_clear_chunkfile_to_load)
		Globals.on_clear_chunkfile_to_load = null

# Load should always go through this
func _load_chunk(file):
	ChunkHandler.LoadChunk(file)
	for obj in Globals.loaded_cityobjects:
		Globals.objects_by_uid[obj.uid] = obj
	for obj in Globals.loaded_lights:
		Globals.objects_by_uid[obj.uid] = obj
	
	
# Select object.
func _select(uid: String):
	var target = Globals.objects_by_uid[uid]
	_unselect()
	gizmo.translation = target.translation
	
	match target.get_parent().name:
		"lights":
			if menu_selected_title:
				menu_selected_title.text = "Selected type: lightsource"
			if menu_selected_light:
				menu_selected_light._select(target)
				menu_selected_light.show()
				menu_selected_light._update_color(target.color)
		"cityobjects":
			if menu_selected_title:
				menu_selected_title.text = "Selected type: cityobject"
			if menu_selected_cityobj:
				menu_selected_cityobj._select(target)
				menu_selected_cityobj.show()
		_:
			push_error("unknown selected")
			return
	currently_selected = target
	currently_selected._set_highlight(true)
	menu_selector._select(target.uid)

func _unselect():
	menu_selector._unselect()
	if currently_selected:
		currently_selected._set_highlight(false)
		currently_selected = null
	if menu_selected_title:
		menu_selected_title.text = "Nothing selected."
	if menu_selected_cityobj:
		menu_selected_cityobj.hide()
	if menu_selected_light:
		menu_selected_light.hide()
	

func _focus():
	if currently_selected:
		cam.translation = currently_selected.translation

func _save():
	ChunkHandler.SaveChunk()
	
func _clear():
	Globals._clear()
	ChunkHandler.OnClearChunk()
	is_chunk_loaded = false
	cam = null
	gizmo = null
	menu_selector = null
	menu_selected_title = null
	menu_selected_cityobj = null
	menu_selected_light = null
	currently_selected = null
	chunk_rendermodels.clear()
	
	get_tree().reload_current_scene()

func _update():
	if menu_selector: menu_selector._update()

func _add_chunk_rendermodel(mesh: Mesh):
	chunk_rendermodels.append(mesh)

func _process(_delta):
	if gizmo:
		if currently_selected:
			gizmo.show()
			gizmo.scale = Vector3.ONE * cam.get_node("pivot").get_node("cam").transform.origin.z * .2
			gizmo.translation = currently_selected.translation
		else:
			gizmo.hide()
		# Focus on selected
		if Input.is_key_pressed(KEY_F):
			_focus()
