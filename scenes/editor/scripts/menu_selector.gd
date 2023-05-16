"""
	Selector
	
	This creates the selector panel on the left.
	Menu structure for node selection:
	Selector
		TabContainer
			ScrollCont > GridCont
				Button
				Button
			ScrollCont > GridCont
				Button
				...
			...
	where each button represents an object loaded from chunk.
	
	Methods indended for external code:
		_select(uid)
		_unselect()
	
	How selection works:
		- 1: A button in selector is pressed, selector calls ChunkEditor._select() -OR- Something else calls ChunkEditor._select()
		- 2: ChunkEditor calls selector._select() to update status of this menu
	
	Dependencies:
		- Globals.loaded_cityobjectso
		- Globals.loaded_lights
		- uid in object scripts
		- ChunkEditor._select(uid)
"""

extends PanelContainer

onready var chunk_root = $"/root/main/chunk"

var button_uid = {}
var tabs
var cobj_container
var light_container
var selected

const STYLE_SELECTED = preload("res://ui/stylebox_selected_text.tres")

func _ready():
	ChunkEditor.menu_selector = self
	tabs = TabContainer.new()
	var cobj_scroll = ScrollContainer.new()
	cobj_container = GridContainer.new()
	var lights_scroll = ScrollContainer.new()
	light_container = GridContainer.new()
	
	add_child(tabs)
	tabs.add_child(cobj_scroll)
	tabs.add_child(lights_scroll)
	cobj_scroll.add_child(cobj_container)
	lights_scroll.add_child(light_container)
	
	cobj_scroll.name = "cityobjects"
	lights_scroll.name = "lights"
	cobj_container.columns = 2
	light_container.columns = 2
	
	_update()

# Update selected menu item. Called by ChunkEditor.
func _select(uid):
	# Set selected
	_unselect()
	selected = button_uid[uid]
	selected.pressed = true
	selected.add_stylebox_override("pressed", STYLE_SELECTED)
	
	# Switch tab, scroll position to selected button
	var scrollcont = selected.get_parent().get_parent()
	if selected.rect_global_position.y < 80 or selected.rect_global_position.y > get_viewport_rect().size.y-30:
		var scrollpos = selected.rect_position.y
		scrollcont.scroll_vertical = scrollpos
	var tab = scrollcont.get_index()
	tabs.current_tab = tab

func _unselect():
	if selected:
		selected.pressed = false
		selected.add_stylebox_override("pressed", null)
		
# Buttons call this
func _select_from_self(uid):
	ChunkEditor._select(uid)
	ChunkEditor._focus()

func _update():
	button_uid.clear()
	for child in cobj_container.get_children():
		child.queue_free()
	for child in light_container.get_children():
		child.queue_free()
	
	for i in Globals.loaded_cityobjects.size():
		var cobj = Globals.loaded_cityobjects[i]
		var button = create_objectbutton(cobj)
		if cobj.is_rendermodel_bad:
			button.add_color_override("font_color", Color.maroon)
		cobj_container.add_child(create_lineno(i))
		cobj_container.add_child(button)
	
	for i in Globals.loaded_lights.size():
		var light = Globals.loaded_lights[i]
		light_container.add_child(create_lineno(i))
		light_container.add_child(create_objectbutton(light))
	
	
	# Labels for if category is empty
	if Globals.loaded_cityobjects.size() == 0:
		cobj_container.add_child(create_nonelabel())
	if Globals.loaded_lights.size() == 0:
		light_container.add_child(create_nonelabel())

# Returns a Button that selects an an object
func create_objectbutton(obj):
	var button = Button.new()
	button.text = obj.name
	button.align =Button.ALIGN_LEFT
	button.flat = true
	button.toggle_mode = true
	button.add_color_override("font_color_pressed", Color.lightsteelblue)
	button_uid[obj.uid] = button
	button.connect("pressed", self, "_select_from_self", [obj.uid])
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
