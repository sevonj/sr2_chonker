extends ViewportContainer

onready var gizmo_viewport: Viewport = $viewport
onready var gizmo_cam_rig: Spatial = $viewport/gizmocam
onready var scene_cam =  ChunkEditor.cam.get_node("pivot").get_node("cam")
var target

func _ready():
	pass
	
func _process(_delta):
	target = ChunkEditor.currently_selected
	if target == null:
		return
	
	var target_position = scene_cam.unproject_position(target.global_translation)
	target_position -= rect_size / 2

	
	rect_global_position = target_position
