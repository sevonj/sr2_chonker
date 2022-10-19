extends Spatial

var look_sensitivity = Vector2(.005, .005)
var move_sensitivity = .5

var look_delta: Vector2
var fly_active = false

onready var pivot = $pivot
onready var cam = $pivot/cam


func _ready():
	ChunkEditor.cam = self

func _unhandled_input(event):
	if event is InputEventMouseMotion:
		look_delta += event.relative * look_sensitivity
		get_viewport().set_input_as_handled()
	elif event is InputEventMouseButton:
		if event.button_index == BUTTON_WHEEL_UP: cam.transform.origin.z *= .9
		elif event.button_index == BUTTON_WHEEL_DOWN: cam.transform.origin.z /= .9
		
func _process(_delta):
	if Input.is_mouse_button_pressed(3):
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
		
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
			
			# WASD move
			var dir = Vector3.ZERO
			if Input.is_key_pressed(KEY_W): dir -= cam.global_transform.basis.z
			if Input.is_key_pressed(KEY_S): dir += cam.global_transform.basis.z
			if Input.is_key_pressed(KEY_A): dir -= cam.global_transform.basis.x
			if Input.is_key_pressed(KEY_D): dir += cam.global_transform.basis.x
			
			dir *= cam.transform.origin.z * .1
			translation += dir * move_sensitivity
	
	else:
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	look_delta = Vector2.ZERO
