extends "res://scenes/editor/scripts/inspector/panel.gd"
"""
Inspector Panel: Lightsource Color

"""
signal changed(color)

var input_color

func _create_menu():
	set_title("Color")
	input_color = ColorPicker.new()
	vbox_contents.add_child(input_color)
	input_color.connect("color_changed", self, "_color_changed")

func _update_color(col: Color):
	input_color.color = col

func _color_changed(col: Color):
	emit_signal("changed", col)
