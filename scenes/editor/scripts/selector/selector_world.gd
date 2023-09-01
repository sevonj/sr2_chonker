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
		- Globals.chunk (and chunk.gd)
		- uid in object scripts
		- ChunkEditor._select(uid)
"""

extends "res://scenes/editor/scripts/selector/selector.gd"

onready var chunk_root = $"/root/main/chunk"

var button_uid = {}
var cobj_container
var light_container

func _ready():
	ChunkEditor.selector = self
	cobj_container = create_selector_list("city objects")
	light_container = create_selector_list("light sources")
	refresh()

# Update selected menu item. Called by ChunkEditor.
func _select(uid):
	_set_selected(button_uid[uid])
func _unselect():
	_set_selected(null)

# Buttons call this
func _select_from_self(uid):
	ChunkEditor._select(uid)
	ChunkEditor._focus()

func refresh():
	button_uid.clear()
	for child in cobj_container.get_children():
		child.queue_free()
	for child in light_container.get_children():
		child.queue_free()
		
	var chunk = Globals.chunk
	if !is_instance_valid(chunk):
		return
	
	for i in chunk.cityobjects.size():
		var cobj = chunk.cityobjects[i]
		var button = create_objectbutton(cobj)
		if cobj.is_rendermodel_bad:
			button.add_color_override("font_color", Color.maroon)
		cobj_container.add_child(create_lineno(i))
		cobj_container.add_child(button)
	
	for i in chunk.lights.size():
		var light = chunk.lights[i]
		light_container.add_child(create_lineno(i))
		light_container.add_child(create_objectbutton(light))
	
	# Labels for if category is empty
	if chunk.cityobjects.size() == 0:
		cobj_container.add_child(create_nonelabel())
	if chunk.lights.size() == 0:
		light_container.add_child(create_nonelabel())

# Returns a Button that selects an an object
func create_objectbutton(obj):
	var button = create_selector_item(obj.displayname)
	button_uid[obj.name] = button
	button.connect("pressed", self, "_select_from_self", [obj.uid])
	return button

