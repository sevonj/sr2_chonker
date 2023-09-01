extends Node


func input_checkbox(title: String, pressed: bool = false, disabled: bool = false):
	var checkbox = CheckBox.new()
	checkbox.text = title
	checkbox.pressed = pressed
	checkbox.disabled = disabled
	return checkbox


func fix_input_size(input: Control):
	input.size_flags_horizontal = 3
	input.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT

func input_title(title: String):
	var label = Label.new()
	label.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
	label.text = title
	label.valign = Label.VALIGN_TOP
	return label

#func create_option_tooltip(text: String):
#	var label = Label.new()
#	label.rect_min_size.y = Globals.UI_INSPECTOR_TEXT_MINHEIGHT
#	label.autowrap = true
#	label.text = text
#	return label

func input_lineedit():
	var ledit = LineEdit.new()
	fix_input_size(ledit)
	return ledit

func input_selectorbutton(text: String):
	var button = Button.new()
	button.text = text
	button.align =Button.ALIGN_LEFT
	button.flat = true
	button.toggle_mode = true
	button.add_color_override("font_color_pressed", Color.lightsteelblue)
	return button

func input_optionbutton():
	var optio = OptionButton.new()
	fix_input_size(optio)
	return optio

func input_vec2():
	var hbox = HBoxContainer.new()
	var editx = input_lineedit()
	var edity = input_lineedit()
	editx.name = "editx"
	edity.name = "edity"
	hbox.add_child(input_title("X"))
	hbox.add_child(editx)
	hbox.add_child(input_title("Y"))
	hbox.add_child(edity)
	fix_input_size(hbox)
	return hbox

func input_vec3():
	var hbox = HBoxContainer.new()
	var editx = input_lineedit()
	var edity = input_lineedit()
	var editz = input_lineedit()
	editx.name = "editx"
	edity.name = "edity"
	editz.name = "editz"
	hbox.add_child(input_title("X"))
	hbox.add_child(editx)
	hbox.add_child(input_title("Y"))
	hbox.add_child(edity)
	hbox.add_child(input_title("Z"))
	hbox.add_child(editz)
	fix_input_size(hbox)
	return hbox

func input_vec4():
	var hbox = HBoxContainer.new()
	var editx = input_lineedit()
	var edity = input_lineedit()
	var editz = input_lineedit()
	var editw = input_lineedit()
	editx.name = "editx"
	edity.name = "edity"
	editz.name = "editz"
	editw.name = "editw"
	hbox.add_child(input_title("X"))
	hbox.add_child(editx)
	hbox.add_child(input_title("Y"))
	hbox.add_child(edity)
	hbox.add_child(input_title("Z"))
	hbox.add_child(editz)
	hbox.add_child(input_title("W"))
	hbox.add_child(editw)
	fix_input_size(hbox)
	return hbox

func label(text: String) -> Label:
	var label = Label.new()
	label.text = text
	return label

func label_status(text: String, status: String) -> Label:
	var label = Label.new()
	label.text = text
	set_label_status(label, status)
	return label

func set_label_status(label: Label, status: String):
	match status:
		"active":
			label.add_color_override("font_color", Color.white)
		"inactive":
			label.add_color_override("font_color", Color.gray)
		"ok":
			label.add_color_override("font_color", Color.limegreen)
		"warning":
			label.add_color_override("font_color", Color.gold)
		"error":
			label.add_color_override("font_color", Color.maroon)
		_: 
			label.add_color_override("font_color", Color.magenta)
