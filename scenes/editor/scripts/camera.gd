extends Spatial

var look_sensitivity = Vector2(.005, .005)
var move_sensitivity = .5

onready var pivot = $pivot
onready var cam = $pivot/cam

func _init():
	ChunkEditor.cam = self

# Called by viewport script
func _zoom_in():
	cam.transform.origin.z *= .9 

# Called by viewport script
func _zoom_out():
	cam.transform.origin.z /= .9 

# Called by viewport script
func _mouseinput(event):
	var look_delta = event.relative * look_sensitivity
	
	# Mouse Drag Move
	if Input.is_key_pressed(KEY_SHIFT):
		var dir = Vector3.ZERO
		dir -= cam.global_transform.basis.x * look_delta.x
		dir += cam.global_transform.basis.y * look_delta.y
		dir *= cam.transform.origin.z
		translation += dir * move_sensitivity
	
	# Mouse Drag Rotate
	else:
		rotate_y(-look_delta.x)
		pivot.rotate_x(look_delta.y)
		if pivot.rotation.x > 1.5:
			pivot.rotation.x = 1.5
		if pivot.rotation.x < -1.5:
			pivot.rotation.x = -1.5
	
	# TODO: fix this. Mouse input code was originally in _process(). Current _mouseinput func is
	# only called when the mouse moves
	# WASD move
	var dir = Vector3.ZERO
	if Input.is_key_pressed(KEY_W): dir -= cam.global_transform.basis.z
	if Input.is_key_pressed(KEY_S): dir += cam.global_transform.basis.z
	if Input.is_key_pressed(KEY_A): dir -= cam.global_transform.basis.x
	if Input.is_key_pressed(KEY_D): dir += cam.global_transform.basis.x
	
	dir *= cam.transform.origin.z * .1
	translation += dir * move_sensitivity
