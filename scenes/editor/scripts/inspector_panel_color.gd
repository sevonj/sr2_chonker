extends "res://scenes/editor/scripts/inspector_panel.gd"

var input_color

# Param 1: Target node of method call.
# Param 2: Name of method.
# Called when a value is changed: target.method(color: Color)
func _create_menu(target, method):
	
	input_color = ColorPicker.new()
	vbox_contents.add_child(input_color)
	input_color.connect("color_changed", target, method)

func _update_color(col):
	input_color.color = col
