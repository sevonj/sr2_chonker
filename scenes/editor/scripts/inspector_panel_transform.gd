extends "res://scenes/editor/scripts/inspector_panel.gd"

var call_target
var call_method

var input_x
var input_y
var input_z

# Param 1: String array of flag names. ["flagname0", "flagname1", "flagname2", ...]
# Param 2: Target node of method call.
# Param 3: Name of method.
# Called when a value is changed: target.method(set: bool, flag_id: int)
func _create_menu(target, method):
	var pos_label = Label.new()
	pos_label.text = "Position XYZ:"
	vbox_contents.add_child(pos_label)
	
	var hbox = HBoxContainer.new()
	vbox_contents.add_child(hbox)
	
	input_x = TextEdit.new()
	input_y = TextEdit.new()
	input_z = TextEdit.new()
	input_x.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_y.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_z.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	input_x.size_flags_horizontal = 3 # Expand
	input_y.size_flags_horizontal = 3
	input_z.size_flags_horizontal = 3
	hbox.add_child(input_x)
	hbox.add_child(input_y)
	hbox.add_child(input_z)
	input_x.connect("text_changed", self, "_set_transform")
	input_y.connect("text_changed", self, "_set_transform")
	input_z.connect("text_changed", self, "_set_transform")
	call_target = target
	call_method = method

func _set_transform():
	var origin = Vector3(int(input_x.text),int(input_y.text),int(input_z.text))
	call_target.call(call_method, origin)

func _update_transform(origin):
	input_x.text = str(origin.x)
	input_y.text = str(origin.y)
	input_z.text = str(origin.z)
