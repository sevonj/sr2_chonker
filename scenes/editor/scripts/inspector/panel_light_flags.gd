extends "res://scenes/editor/scripts/inspector/panel.gd"
"""
Inspector Panel: Lightsource Bitflags

"""
signal changed(idx, val)


func _create_menu(flagnames):
	set_title("Flags")
	for i in flagnames.size():
		var flagname = flagnames[i]
		var flag_input = CheckBox.new()
		flag_input.text = flagname
		flag_input.flat = true
		flag_input.focus_mode = Control.FOCUS_NONE
		vbox_contents.add_child(flag_input)
		flag_input.connect("toggled", self, "_flag_toggled", [i])

# Param 1: Flag values, array of bools.
func _update_flags(values: Array):
	for i in range(vbox_contents.get_child_count()):
		var input = vbox_contents.get_child(i)
		input.pressed = values[i]

func _flag_toggled(idx: int, val: bool):
	emit_signal("changed", idx, val)
	
