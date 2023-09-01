extends "res://scenes/editor/scripts/inspector/panel.gd"
"""
Inspector Panel: Transform
Common panel for anything with a position or a transform.

Inputs:
	- Position
	- Rotation
	- Scale

disable_basis hides rotation & scale inputs

"""
signal changed(origin, rotation, scale)

var input_position
var input_rotation
var input_scale


func _create_menu(disable_basis:bool=false):
	set_title("Transform")
	var hbox = GridContainer.new()
	hbox.columns = 2
	vbox_contents.add_child(hbox)
	
	input_position = create_option_vec3()
	input_rotation = create_option_vec3()
	input_scale = create_option_vec3()

	hbox.add_child(create_option_title("Position"))
	hbox.add_child(input_position)
	if not disable_basis:
		hbox.add_child(create_option_title("Rotation"))
		hbox.add_child(input_rotation)
		hbox.add_child(create_option_title("Scale"))
		hbox.add_child(input_scale)
	
	input_position.get_node("editx").connect("text_changed", self, "_set_transform")
	input_position.get_node("edity").connect("text_changed", self, "_set_transform")
	input_position.get_node("editz").connect("text_changed", self, "_set_transform")
	input_rotation.get_node("editx").connect("text_changed", self, "_set_transform")
	input_rotation.get_node("edity").connect("text_changed", self, "_set_transform")
	input_rotation.get_node("editz").connect("text_changed", self, "_set_transform")
	input_scale.get_node("editx").connect("text_changed", self, "_set_transform")
	input_scale.get_node("edity").connect("text_changed", self, "_set_transform")
	input_scale.get_node("editz").connect("text_changed", self, "_set_transform")
	
	vbox_contents.add_child(create_option_tooltip("I haven't bothered to make the rotation/scale gizmos yet. You have to type them manually."))

func _set_transform(_t):
	var origin = Vector3(
		float(input_position.get_node("editx").text),
		float(input_position.get_node("edity").text),
		float(input_position.get_node("editz").text))
	var rotation = Vector3(
		float(input_rotation.get_node("editx").text),
		float(input_rotation.get_node("edity").text),
		float(input_rotation.get_node("editz").text))
	var scale = Vector3(
		float(input_scale.get_node("editx").text),
		float(input_scale.get_node("edity").text),
		float(input_scale.get_node("editz").text))
	emit_signal("changed", origin, rotation, scale)

func _update_transform(origin, rotation, scale):
	input_position.get_node("editx").text = str(origin.x)
	input_position.get_node("edity").text = str(origin.y)
	input_position.get_node("editz").text = str(origin.z)
	input_rotation.get_node("editx").text = str(rotation.x)
	input_rotation.get_node("edity").text = str(rotation.y)
	input_rotation.get_node("editz").text = str(rotation.z)
	input_scale.get_node("editx").text = str(scale.x)
	input_scale.get_node("edity").text = str(scale.y)
	input_scale.get_node("editz").text = str(scale.z)
	
	
	
	
	
	
	
