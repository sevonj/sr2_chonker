extends "res://scenes/editor/scripts/inspector/panel.gd"
"""
Inspector Panel: Lightsource Properties

Inputs:
	- unk10
	- radius_inner
	- radius_outer
	- render_dist
	- parent
	- type

"""
signal changed(unk10, radius_inner, radius_outer, render_dist, parent, type)

var input_unk10
var input_radius_inner
var input_radius_outer
var input_render_dist
var input_parent
var input_type


func _create_menu():
	set_title("Properties")
	var container = GridContainer.new()
	container.columns = 2
	vbox_contents.add_child(container)
	
	input_unk10 = create_option_lineedit()
	input_unk10.connect("text_changed", self, "_set_properties")
	
	input_type = create_option_optionbut()
	input_type.add_item("0: unknown", 0)
	input_type.add_item("1: unknown", 1)
	input_type.add_item("2: unknown", 2)
	input_type.add_item("3: unknown", 3)
	input_type.connect("item_selected", self, "_on_type_set")
	
	input_radius_inner = create_option_lineedit()
	input_radius_inner.connect("text_changed", self, "_set_properties")
	
	input_radius_outer = create_option_lineedit()
	input_radius_outer.connect("text_changed", self, "_set_properties")
	
	input_render_dist = create_option_lineedit()
	input_render_dist.connect("text_changed", self, "_set_properties")
	
	input_parent = create_option_optionbut()
	input_parent.connect("item_selected", self, "_on_type_set")
	
	container.add_child(create_option_title("Unknown 10"))
	container.add_child(input_unk10)
	container.add_child(create_option_title("Inner radius"))
	container.add_child(input_radius_inner)
	container.add_child(create_option_title("Outer radius"))
	container.add_child(input_radius_outer)
	container.add_child(create_option_title("Render distance"))
	container.add_child(input_render_dist)
	container.add_child(create_option_title("Parent"))
	container.add_child(input_parent)
	container.add_child(create_option_title("Light type??"))
	container.add_child(input_type)

func _update_properties(unk10, radius_inner, radius_outer, render_dist, parent, type):
	input_unk10.text = str(unk10)
	input_radius_inner.text = str(radius_inner)
	input_radius_outer.text = str(radius_outer)
	input_render_dist.text = str(render_dist)
	input_parent.clear()
	input_parent.add_item("-1: None", 0)
	for i in Globals.chunk.cityobjects.size():
		var cobj = Globals.chunk.cityobjects[i]
		var itemname = str(i) + ": " + cobj.name
		input_parent.add_item(itemname)
	input_parent.selected = parent + 1
	input_type.selected = type

func _on_type_set(_n):
	_set_properties()

func _set_properties(_unused=null):
	var unk10 = int(input_unk10.text)
	var radius_inner = float(input_radius_inner.text)
	var radius_outer = float(input_radius_outer.text)
	var render_dist = float(input_render_dist.text)
	var parent = input_parent.selected -1
	var type = input_type.selected
	emit_signal("changed", unk10, radius_inner, radius_outer, render_dist, parent, type)
