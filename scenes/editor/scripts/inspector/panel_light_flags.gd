extends "res://scenes/editor/scripts/inspector/panel.gd"
"""
Inspector Panel: Lightsource Bitflags

"""
signal changed(idx, val)

enum flags {
	flag0x0 = 1 << 31,
	flag0x1 = 1 << 30,
	flag0x2 = 1 << 29,
	flag0x3 = 1 << 28,
	flag0x4 = 1 << 27,
	flag0x8 = 1 << 23,
	flag0xa = 1 << 21,
	ShadowCharacter = 1 << 19,
	ShadowLevel = 1 << 18,
	LightCharacter = 1 << 17,
	LightLevel = 1 << 16,
	flag0x16 = 1 << 9
};

onready var input_flag0x0
onready var input_flag0x1
onready var input_flag0x2
onready var input_flag0x3
onready var input_flag0x4
onready var input_flag0x8
onready var input_flag0xa
onready var input_shadowcharacter
onready var input_shadowlevel
onready var input_lightcharacter
onready var input_lightlevel
onready var input_flag0x16

func _create_menu():
	set_title("Flags")
	input_flag0x0 = create_flag(flags.flag0x0, "Flag 0x0")
	input_flag0x1 = create_flag(flags.flag0x1, "Flag 0x1")
	input_flag0x2 = create_flag(flags.flag0x2, "Flag 0x2")
	input_flag0x3 = create_flag(flags.flag0x3, "Flag 0x3")
	input_flag0x4 = create_flag(flags.flag0x4, "Flag 0x4")
	input_flag0x8 = create_flag(flags.flag0x8, "Flag 0x8")
	input_flag0xa = create_flag(flags.flag0xa, "Flag 0xa")
	input_shadowcharacter = create_flag(flags.ShadowCharacter, "ShadowCharacter")
	input_shadowlevel = create_flag(flags.ShadowLevel, "ShadowLevel")
	input_lightcharacter = create_flag(flags.LightCharacter, "LightCharacter")
	input_lightlevel = create_flag(flags.LightLevel, "LightLevel")
	input_flag0x16 = create_flag(flags.flag0x16, "Flag 0x16")
	#for i in flagnames.size():
	#	var flagname = flagnames[i]
	#	var flag_input = CheckBox.new()
	#	flag_input.text = flagname
	#	flag_input.flat = true
	#	flag_input.focus_mode = Control.FOCUS_NONE
	#	vbox_contents.add_child(flag_input)
	#	flag_input.connect("toggled", self, "_flag_toggled", [i])

func create_flag(flag: int, flagname: String):
	var flag_input = CheckBox.new()
	flag_input.text = flagname
	flag_input.flat = true
	flag_input.focus_mode = Control.FOCUS_NONE
	vbox_contents.add_child(flag_input)
	flag_input.connect("toggled", self, "_flag_toggled", [flag])
	return flag_input

# Param 1: Flag values, array of bools.
func _update_flags(values: int):
	print("FLAGS",values)
	input_flag0x0.pressed = values & flags.flag0x0 != 0
	input_flag0x1.pressed = values & flags.flag0x1 != 0
	input_flag0x2.pressed = values & flags.flag0x2 != 0
	input_flag0x3.pressed = values & flags.flag0x3 != 0
	input_flag0x4.pressed = values & flags.flag0x4 != 0
	input_flag0x8.pressed = values & flags.flag0x8 != 0
	input_flag0xa.pressed = values & flags.flag0xa != 0
	input_shadowcharacter.pressed = (values & flags.ShadowCharacter) != 0
	input_shadowlevel.pressed = (values & flags.ShadowLevel) != 0
	input_lightcharacter.pressed = (values & flags.LightCharacter) != 0
	input_lightlevel.pressed = (values & flags.LightLevel) != 0
	input_flag0x16.pressed = (values & flags.flag0x16) != 0

func _flag_toggled(flag: int, val: bool):
	emit_signal("changed", flag, val)
	
