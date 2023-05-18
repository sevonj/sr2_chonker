extends PanelContainer
"""
Base class for Inspector Panels

Panel structure:

PanelContainer
	top_vbox
		title / togglehide (Button)
		Panel contents (Panel creates its own container)

"""
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

func set_title(title: String):
	input_togglehide.text = title

func toggle_expand(yes: bool):
	vbox_contents.visible = yes
	if yes:
		input_togglehide.icon = ui_icon_expanded
	else:
		input_togglehide.icon = ui_icon_contracted

func create_option_title(title: String):
	var label = Label.new()
	label.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	label.text = title
	return label

func create_option_tooltip(text: String):
	var label = Label.new()
	label.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	label.autowrap = true
	label.text = text
	return label

func create_option_lineedit():
	var ledit = LineEdit.new()
	ledit.size_flags_horizontal = 3
	ledit.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	return ledit

func create_option_optionbut():
	var optio = OptionButton.new()
	optio.size_flags_horizontal = 3
	optio.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	return optio

func create_option_vec3():
	var hbox = HBoxContainer.new()
	var editx = create_option_lineedit()
	var edity = create_option_lineedit()
	var editz = create_option_lineedit()
	editx.name = "editx"
	edity.name = "edity"
	editz.name = "editz"
	hbox.add_child(create_option_title("X"))
	hbox.add_child(editx)
	hbox.add_child(create_option_title("Y"))
	hbox.add_child(edity)
	hbox.add_child(create_option_title("Z"))
	hbox.add_child(editz)
	
	hbox.size_flags_horizontal = 3
	hbox.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	return hbox
