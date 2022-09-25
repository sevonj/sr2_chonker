extends Node

var cam
var gizmo
var menu_cityobj_selected

var selected_cityobj: Spatial

func _ready():
	get_tree().connect("files_dropped", self, "_on_files_dropped")

func _unhandled_input(event):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			_unselect_cityobj()

func _on_files_dropped(files, _screen):
	if len(files) != 1:
		print("One file at a time!")
		return
	var filepath = files[0]
	ChunkHandler.LoadChunk(filepath)

func _select_cityobj(obj: Spatial):
	if menu_cityobj_selected:
		_unselect_cityobj()
		selected_cityobj = obj
		selected_cityobj._set_highlight(true)
		gizmo.translation = selected_cityobj.translation
		
		menu_cityobj_selected._select(obj)
		menu_cityobj_selected.show()
	
func _unselect_cityobj():
	if menu_cityobj_selected:
		if selected_cityobj:
			selected_cityobj._set_highlight(false)
			selected_cityobj = null
		menu_cityobj_selected.hide()

func _save():
	ChunkHandler.SaveChunk()
	
func _clear():
	ChunkHandler.ClearChunk()
	get_tree().reload_current_scene()

func _process(_delta):
	if gizmo:
		if selected_cityobj:
			gizmo.show()
			gizmo.scale = Vector3.ONE * cam.get_node("pivot").get_node("cam").transform.origin.z * .2
			
			gizmo.translation = selected_cityobj.translation
			
		else:
			gizmo.hide()
		
		# Input
		# Focus on selected
		if Input.is_key_pressed(KEY_F):
			if selected_cityobj:
				cam.translation = selected_cityobj.translation
