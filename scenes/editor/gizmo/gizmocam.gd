extends Spatial

var gizmo_cam
var scene_cam

func _ready():
	gizmo_cam = get_node("cam")
	scene_cam = ChunkEditor.cam.get_node("pivot").get_node("cam")
	ChunkEditor.gizmo_cam = gizmo_cam

func _process(_delta):
	var target = get_parent().get_node("gizmo_translation").target
	if target == null:
		return
	
	var dir = scene_cam.to_local(target.global_transform.origin).normalized()
	var sheared_basis = Basis(
	Vector3(1.0, 0.0, 0.0),
	Vector3(0.0, 1.0, 0.0),
	Vector3(dir.x, dir.y, 1.0)
	) * scene_cam.global_transform.basis.inverse()
	get_parent().get_node("gizmo_translation").transform.basis = sheared_basis
	
