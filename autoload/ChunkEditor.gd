# --- ChunkEditor.gd --- #
# Handles editor functions and contains data used by nodes.
#
#
extends Node

var is_chunk_loaded = false
var opt_rendermodels = true
var opt_unpack = false

var cam
var gizmo
var inspector_title
var inspector_cityobj
var inspector_light
var selector

var currently_selected: Spatial

const FILE_IMPORT_DIALOG = preload("res://scenes/editor/scripts/import_dialog.gd")
const CHUNK = preload("res://scenes/editor/scripts/chunk.gd")
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
	var dialog = FILE_IMPORT_DIALOG.new()
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
	Globals.chunk = CHUNK.new()
	get_tree().root.get_node("main").add_child(Globals.chunk)
	ChunkHandler.LoadChunk(file)
	Globals.chunk._load()
	
	
	#var obj_parent = get_tree().root.get_node("main").get_node("chunk") #.get_node("objects")
	#for pos in Globals.unkposses:
	#	var posnode = CSGBox.new()
	#	posnode.transform.origin = pos
	#	obj_parent.add_child(posnode)
	
	
# Select object.
func _select(uid: String):
	var target = Globals.chunk.objects_by_uid[uid]
	_unselect()
	gizmo.transform.origin = target.global_transform.origin
	
	if target.uid.begins_with("cobj_"):
		if inspector_title:
			inspector_title.text = "Selected type: cityobject"
		if inspector_cityobj:
			inspector_cityobj._select(target)
			inspector_cityobj.show()
	elif target.uid.begins_with("light_"):
		if inspector_title:
			inspector_title.text = "Selected type: lightsource"
		if inspector_light:
			inspector_light._select(target)
			inspector_light.show()
			inspector_light._update_color(target.color)
	else:
		push_error("unknown selected")
		return
	currently_selected = target
	currently_selected._set_highlight(true)
	selector._select(target.uid)

func _unselect():
	selector._unselect()
	if currently_selected:
		currently_selected._set_highlight(false)
		currently_selected = null
	if inspector_title:
		inspector_title.text = "Nothing selected."
	if inspector_cityobj:
		inspector_cityobj.hide()
	if inspector_light:
		inspector_light.hide()
	

func _focus():
	if currently_selected:
		cam.transform.origin = currently_selected.global_transform.origin

func _save():
	ChunkHandler.SaveChunk()
	
func _clear():
	Globals._clear()
	ChunkHandler.OnClearChunk()
	is_chunk_loaded = false
	cam = null
	gizmo = null
	selector = null
	inspector_title = null
	inspector_cityobj = null
	inspector_light = null
	currently_selected = null
	
	get_tree().reload_current_scene()

func _update():
	if selector: selector._update()

func _process(_delta):
	if gizmo:
		if currently_selected:
			gizmo.show()
			gizmo.scale = Vector3.ONE * cam.get_node("pivot").get_node("cam").transform.origin.z * .2
			gizmo.transform.origin = currently_selected.global_transform.origin
		else:
			gizmo.hide()
		# Focus on selected
		if Input.is_key_pressed(KEY_F):
			_focus()

func _baked_collision_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._baked_collision_visible(vis)
func _rendermodels_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._rendermodel_visible(vis)
func _lights_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._light_visible(vis)
