extends Spatial

const MAT_HIGHLIGHT = preload("res://scenes/editor/gizmo/mat_highlight.tres")

var target: Spatial

onready var mdl_rod_x = get_node("rod_x")
onready var mdl_rod_y = get_node("rod_y")
onready var mdl_rod_z = get_node("rod_z")
onready var mdl_tip_x = get_node("tip_x")
onready var mdl_tip_y = get_node("tip_y")
onready var mdl_tip_z = get_node("tip_z")
onready var mdl_plane_yz = get_node("plane_yz")
onready var mdl_plane_xz = get_node("plane_xz")
onready var mdl_plane_xy = get_node("plane_xy")

onready var gizmo_cam: Camera = get_parent().get_node("gizmocam/cam")
onready var scene_cam: Camera = ChunkEditor.cam.get_node("pivot").get_node("cam")

const MAT_DEBUG = preload("res://scenes/editor/gizmo/mat_debug.tres")

enum AXIS{
	X, Y, Z, YZ, XZ, XY
}

var dragging: bool = false
var drag_axis = AXIS.X
var original_transform = null
var mouse_delta: Vector2 = Vector2.ZERO
var og_pos: Vector3 = Vector3.ZERO
var drag_start_pos = null

func _ready():
	ChunkEditor.gizmo = self

func _initiate_drag(handle):
	if !is_visible_in_tree():
		return
	if target == null:
		return
	match handle:
		"body_rod_x": drag_axis = AXIS.X
		"body_rod_y": drag_axis = AXIS.Y
		"body_rod_z": drag_axis = AXIS.Z
		"body_plane_yz": drag_axis = AXIS.YZ
		"body_plane_xz": drag_axis = AXIS.XZ
		"body_plane_xy": drag_axis = AXIS.XY
		_: return
	dragging = true
	og_pos = target.global_translation
	print("drag started")
	drag_start_pos = null
	mouse_delta = Vector2.ZERO
	
func _input(event):
	if dragging == true:
		
		# Exit Drag
		if event is InputEventMouseButton:
			if event.button_index == BUTTON_LEFT and !event.pressed:
				dragging = false
				return
		
		# Cancel Drag
		if event is InputEventKey and event.pressed:
			if event.scancode == KEY_ESCAPE:
				dragging = false
				target.global_translation = og_pos
				return
		
		# Drag (pos)
		if event is InputEventMouseMotion:
			var mousepos = scene_cam.get_viewport().get_mouse_position()
			var relative_pos = target.global_translation - scene_cam.global_translation
			
			# Creade normal for the plane we're dragging along
			var n
			match drag_axis:
				AXIS.X:
					relative_pos.x = 0
					n = relative_pos.normalized()
				AXIS.Y:
					relative_pos.y = 0
					n = relative_pos.normalized()
				AXIS.Z:
					relative_pos.z = 0
					n = relative_pos.normalized()
				AXIS.YZ: n = Vector3.RIGHT
				AXIS.XZ: n = Vector3.UP
				AXIS.XY: n = Vector3.FORWARD
			
			# Create line
			var p = scene_cam.project_ray_origin(mousepos) - og_pos
			var v = scene_cam.project_ray_normal(mousepos)
			
			# Intersection point for the line and plane
			var t = - n.dot(p) / n.dot(v)
			var intersect_pos = p + t * v
			
			# Offset the starting position to where cursor was (projected 3d) at drag initiation.
			# Prevents abrupt jump when starting drag.
			if drag_start_pos == null:
				drag_start_pos = intersect_pos
			var new_origin =  intersect_pos - drag_start_pos
			
			# Eliminate the extra axis for X, Y, or Z
			match drag_axis:
				AXIS.X: new_origin *= Vector3.RIGHT
				AXIS.Y: new_origin *= Vector3.UP
				AXIS.Z: new_origin *= Vector3.BACK
			
			# TODO: local transform
			new_origin += og_pos
			
			# TODO: This guard doesn't work.
			if new_origin.length() == float('inf'):
				return
			
			target.global_translation = new_origin
			target.refresh_data()

func _highlight(handlename: String):
	_clear_mats()
	match handlename:
		"body_rod_x":
			mdl_rod_x.material_override = MAT_HIGHLIGHT
			mdl_tip_x.material_override = MAT_HIGHLIGHT
		"body_rod_y":
			mdl_rod_y.material_override = MAT_HIGHLIGHT
			mdl_tip_y.material_override = MAT_HIGHLIGHT
		"body_rod_z":
			mdl_rod_z.material_override = MAT_HIGHLIGHT
			mdl_tip_z.material_override = MAT_HIGHLIGHT
		"body_plane_yz":
			mdl_plane_yz.material_override = MAT_HIGHLIGHT
		"body_plane_xz":
			mdl_plane_xz.material_override = MAT_HIGHLIGHT
		"body_plane_xy":
			mdl_plane_xy.material_override = MAT_HIGHLIGHT

func _clear_mats():
	mdl_rod_x.material_override = null
	mdl_rod_y.material_override = null
	mdl_rod_z.material_override = null
	mdl_tip_x.material_override = null
	mdl_tip_y.material_override = null
	mdl_tip_z.material_override = null
	mdl_plane_yz.material_override = null
	mdl_plane_xz.material_override = null
	mdl_plane_xy.material_override = null
