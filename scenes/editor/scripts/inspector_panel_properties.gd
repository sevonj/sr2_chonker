extends "res://scenes/editor/scripts/inspector_panel.gd"

var call_target
var call_method

var input_radius_inner
var input_radius_outer
var input_render_dist

# Param 1: Target node of method call.
# Param 2: Name of method.
# Called when a value is changed: target.method(color: Color)
func _create_menu(target, method):
	
	var hbox_r0 = HBoxContainer.new()
	var hbox_r1 = HBoxContainer.new()
	var hbox_dist = HBoxContainer.new()
	vbox_contents.add_child(hbox_r0)
	vbox_contents.add_child(hbox_r1)
	vbox_contents.add_child(hbox_dist)
	var title_radius_inner = Label.new()
	var title_radius_outer = Label.new()
	var title_render_dist = Label.new()
	input_radius_inner = TextEdit.new()
	input_radius_outer = TextEdit.new()
	input_render_dist = TextEdit.new()
	
	input_radius_inner.size_flags_horizontal = 3 # Expand
	input_radius_outer.size_flags_horizontal = 3
	input_render_dist.size_flags_horizontal = 3
	title_radius_inner.size_flags_horizontal = 3
	title_radius_outer.size_flags_horizontal = 3
	title_render_dist.size_flags_horizontal = 3
	input_radius_inner.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_radius_outer.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_render_dist.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	title_radius_inner.text = "radius_inner"
	title_radius_outer.text = "radius_outer"
	title_render_dist.text = "render_dist"
	
	hbox_r0.add_child(title_radius_inner)
	hbox_r0.add_child(input_radius_inner)
	hbox_r1.add_child(title_radius_outer)
	hbox_r1.add_child(input_radius_outer)
	hbox_dist.add_child(title_render_dist)
	hbox_dist.add_child(input_render_dist)
	
	input_radius_inner.connect("text_changed", self, "_set_properties")
	input_radius_outer.connect("text_changed", self, "_set_properties")
	input_render_dist.connect("text_changed", self, "_set_properties")
	
	call_target = target
	call_method = method

func _update_properties(radius_inner, radius_outer, render_dist):
	input_radius_inner.text = str(radius_inner)
	input_radius_outer.text = str(radius_outer)
	input_render_dist.text = str(render_dist)
	
func _set_properties(col):
	var radius_inner = float(input_radius_inner.text)
	var radius_outer = float(input_radius_outer.text)
	var render_dist = float(input_render_dist.text)
	call_target.call(call_method, [radius_inner, radius_outer, render_dist])
