extends "res://scenes/editor/scripts/inspector_panel.gd"

var call_target
var call_method

var input_posx
var input_posy
var input_posz
var input_rotx
var input_roty
var input_rotz
var input_sclx
var input_scly
var input_sclz

# Param 1: String array of flag names. ["flagname0", "flagname1", "flagname2", ...]
# Param 2: Target node of method call.
# Param 3: Name of method.
# Called when a value is changed: target.method(set: bool, flag_id: int)
func _create_menu(target, method):
	var temp_label = Label.new()
	temp_label.text = "I haven't bothered to make the gizmo work yet. You have to type the transform values manually."
	temp_label.autowrap = true
	vbox_contents.add_child(temp_label)
	var label_pos = Label.new()
	var hbox_pos = HBoxContainer.new()
	var label_rot = Label.new()
	var hbox_rot = HBoxContainer.new()
	var label_scl = Label.new()
	var hbox_scl = HBoxContainer.new()
	label_pos.text = "Position XYZ:"
	label_rot.text = "Rotation XYZ:"
	label_scl.text = "Scale XYZ:"
	vbox_contents.add_child(label_pos)
	vbox_contents.add_child(hbox_pos)
	vbox_contents.add_child(label_rot)
	vbox_contents.add_child(hbox_rot)
	vbox_contents.add_child(label_scl)
	vbox_contents.add_child(hbox_scl)
	
	input_posx = TextEdit.new()
	input_posy = TextEdit.new()
	input_posz = TextEdit.new()
	input_rotx = TextEdit.new()
	input_roty = TextEdit.new()
	input_rotz = TextEdit.new()
	input_sclx = TextEdit.new()
	input_scly = TextEdit.new()
	input_sclz = TextEdit.new()
	input_posx.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_posy.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_posz.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_rotx.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_roty.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_rotz.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_sclx.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_scly.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_sclz.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_posx.size_flags_horizontal = 3 # Expand
	input_posy.size_flags_horizontal = 3
	input_posz.size_flags_horizontal = 3
	input_rotx.size_flags_horizontal = 3
	input_roty.size_flags_horizontal = 3
	input_rotz.size_flags_horizontal = 3
	input_sclx.size_flags_horizontal = 3
	input_scly.size_flags_horizontal = 3
	input_sclz.size_flags_horizontal = 3
	hbox_pos.add_child(input_posx)
	hbox_pos.add_child(input_posy)
	hbox_pos.add_child(input_posz)
	hbox_rot.add_child(input_rotx)
	hbox_rot.add_child(input_roty)
	hbox_rot.add_child(input_rotz)
	hbox_scl.add_child(input_sclx)
	hbox_scl.add_child(input_scly)
	hbox_scl.add_child(input_sclz)
	input_posx.connect("text_changed", self, "_set_transform")
	input_posy.connect("text_changed", self, "_set_transform")
	input_posz.connect("text_changed", self, "_set_transform")
	input_rotx.connect("text_changed", self, "_set_transform")
	input_roty.connect("text_changed", self, "_set_transform")
	input_rotz.connect("text_changed", self, "_set_transform")
	input_sclx.connect("text_changed", self, "_set_transform")
	input_scly.connect("text_changed", self, "_set_transform")
	input_sclz.connect("text_changed", self, "_set_transform")
	call_target = target
	call_method = method

func _set_transform():
	var global_translation = Vector3(float(input_posx.text),float(input_posy.text),float(input_posz.text))
	var rotation = Vector3(float(input_rotx.text),float(input_roty.text),float(input_rotz.text))
	var scale = Vector3(float(input_sclx.text),float(input_scly.text),float(input_sclz.text))
	call_target.call(call_method, global_translation, rotation, scale)

func _update_transform(target: Node):
	input_posx.text = str(target.get_global_translation().x)
	input_posy.text = str(target.get_global_translation().y)
	input_posz.text = str(target.get_global_translation().z)
	input_rotx.text = str(target.get_rotation_degrees().x)
	input_roty.text = str(target.get_rotation_degrees().y)
	input_rotz.text = str(target.get_rotation_degrees().z)
	input_sclx.text = str(target.get_scale().x)
	input_scly.text = str(target.get_scale().y)
	input_sclz.text = str(target.get_scale().z)
