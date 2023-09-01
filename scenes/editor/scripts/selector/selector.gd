extends Control

const STYLE_SELECTED = preload("res://ui/stylebox_selected_text.tres")

onready var tabs: TabContainer = TabContainer.new()
var selected

func _ready():
	add_child(tabs)
	rect_size.x = 300
	rect_min_size.x = 300

# Update menu.
func _set_selected(new_selected):
	# unselect old
	if is_instance_valid(selected):
		selected.pressed = false
		selected.add_stylebox_override("pressed", null)
	
	selected = new_selected
	if !is_instance_valid(selected):
		return
	selected.pressed = true
	selected.add_stylebox_override("pressed", STYLE_SELECTED)
	
	# Switch tab, scroll position to selected button
	var scrollcont = selected.get_parent().get_parent()
	if selected.rect_global_position.y < 80 or selected.rect_global_position.y > get_viewport_rect().size.y-30:
		var scrollpos = selected.rect_position.y
		scrollcont.scroll_vertical = scrollpos
	var tab = scrollcont.get_index()
	tabs.current_tab = tab

func create_selector_list(title: String):
	var scroll = ScrollContainer.new()
	var vbox = GridContainer.new()
	vbox.size_flags_horizontal = 3
	vbox.columns = 2
	scroll.name = title
	tabs.add_child(scroll)
	scroll.add_child(vbox)
	return vbox

func create_selector_item(title: String):
	var button = Button.new()
	button.size_flags_horizontal = 3
	button.text = title
	button.align = Button.ALIGN_LEFT
	button.flat = true
	button.toggle_mode = true
	button.add_color_override("font_color_pressed", Color.lightsteelblue)
	button.connect("pressed", self, "_set_selected", [button])
	button.expand_icon = true
	return button

# Returns a Label for line numbers
func create_lineno(i):
	var label = Label.new()
	label.text = str(i)
	label.align = Label.ALIGN_RIGHT
	label.add_color_override("font_color", Color.slategray)
	return label

# Returns a placeholder label
func create_nonelabel():
	var label = Label.new()
	label.text = "None loaded."
	return label
