# --- ChunkEditor.gd --- #
# Handles editor functions and contains data used by nodes.
#
#
extends Node

var is_chunk_loaded = false
var opt_rendermodels = true
var opt_unpack = true

var cam
var gizmo
var gizmo_cam
var threedee_cursor
var inspector_title
var inspector_cityobj
var inspector_light
var selector


var currently_selected: Spatial

var ui


func _ready():
	OS.window_borderless = false
	OS.window_size = Globals.UI_WINDOWSIZE
	OS.center_window()
	get_tree().connect("files_dropped", ProjectMan, "_on_files_dropped")
	ProjectMan.connect("loaded", self, "_on_load")


# On Clear / Reload
func _on_main_ready():
	ui = get_tree().root.get_node("main").get_node("ui")
	if Globals.on_clear_chunkfile_to_load:
		ProjectMan.import_chunk(Globals.on_clear_chunkfile_to_load)
		Globals.on_clear_chunkfile_to_load = null

func _on_load():
	cam.transform.origin = Globals.chunk.cityobjects[0].transform.origin

# Select object.
func _select(uid: String):
	var target = Globals.chunk.objects_by_uid[uid]
	_unselect()
	gizmo.target = target
	gizmo.show()
	
	if target.uid.begins_with("cobj_"):
		if inspector_title:
			inspector_title.text = "Selected type: cityobject"
		if inspector_cityobj:
			inspector_cityobj._select(target)
			inspector_cityobj.show()
	elif target.name.begins_with("light_"):
		if inspector_title:
			inspector_title.text = "Selected type: lightsource"
		if inspector_light:
			inspector_light._select(target)
			inspector_light.show()
			inspector_light._update_color(
				Color(
					target.data.Color.R,
					target.data.Color.G,
					target.data.Color.B
				))
	else:
		push_error("unknown selected")
		return
	currently_selected = target
	currently_selected._set_highlight(true)
	selector._select(target.uid)

func _unselect():
	gizmo.hide()
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
	if !is_chunk_loaded:
		return
	if !is_instance_valid(Globals.chunk):
		print("Save error 1")
		return
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

#func __update():
#	if selector: selector._update()

func _baked_collision_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._baked_collision_visible(vis)
func _rendermodels_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._rendermodel_visible(vis)
func _lights_visible(vis: bool):
	if Globals.chunk != null:
		Globals.chunk._light_visible(vis)
