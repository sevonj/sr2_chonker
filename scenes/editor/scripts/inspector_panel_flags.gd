extends "res://scenes/editor/scripts/inspector_panel.gd"

# Param 1: String array of flag names. ["flagname0", "flagname1", "flagname2", ...]
# Param 2: Target node of method call.
# Param 3: Name of method.
# Called when a value is changed: target.method(set: bool, flag_id: int)
func _create_menu(flagnames, target, method):
	for i in flagnames.size():
		var flagname = flagnames[i]
		var flag_input = CheckBox.new()
		flag_input.text = flagname
		flag_input.flat = true
		flag_input.focus_mode = Control.FOCUS_NONE
		vbox_contents.add_child(flag_input)
		flag_input.connect("toggled", target, method, [i])

# Param 1: Flag values, array of bools.
func _update_flags(values):
	for i in range(vbox_contents.get_child_count()):
		var input = vbox_contents.get_child(i)
		input.pressed = values[i]
