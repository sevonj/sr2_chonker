extends PanelContainer

var ui_icon_expanded = preload("res://ui/ui_icon_expanded.png")
var ui_icon_contracted = preload("res://ui/ui_icon_contracted.png")
var ui_stylebox = preload("res://ui/stylebox_inspector_panel.tres")

var input_togglehide
var vbox_contents

func _ready():
	var top_vbox = VBoxContainer.new()
	add_child(top_vbox)
	
	input_togglehide = Button.new()
	input_togglehide.toggle_mode = true
	input_togglehide.text = name
	input_togglehide.flat = true
	input_togglehide.expand_icon = true
	input_togglehide.focus_mode = Control.FOCUS_NONE
	top_vbox.add_child(input_togglehide)
	input_togglehide.connect("toggled", self, "toggle_expand")
	
	vbox_contents = VBoxContainer.new()
	top_vbox.add_child(vbox_contents)
	
	var toggled = Globals.UI_INSPECTOR_PANELS_STARTOPEN
	input_togglehide.pressed = toggled
	toggle_expand(toggled)
	
	add_stylebox_override("panel", ui_stylebox)

func toggle_expand(yes):
	vbox_contents.visible = yes
	if yes:
		input_togglehide.icon = ui_icon_expanded
	else:
		input_togglehide.icon = ui_icon_contracted
