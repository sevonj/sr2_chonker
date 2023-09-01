extends "res://scenes/editor/scripts/selector/selector.gd"

onready var editor = get_parent()
onready var selector_texlist
onready var selector_matlist


func _ready():
	selector_texlist = create_selector_list("Textures")
	selector_matlist = create_selector_list("Materials")

func refresh():
	# Delete old menu items
	for child in selector_texlist.get_children():
		child.queue_free()
	for child in selector_matlist.get_children():
		child.queue_free()
	
	if Globals.chunk == null:
		return
	
	# Create menu items
	for i in Globals.chunk.texture_names.size():
		var tex = Globals.chunk.texture_names[i]
		var but = create_selector_item(tex)
		but.icon = editor._get_texture(i)
		but.connect("pressed", editor, "_select_texture", [i])
		selector_texlist.add_child(create_lineno(i))
		selector_texlist.add_child(but)
		
		
	for i in Globals.chunk.materials.size():
		var mat = Globals.chunk.materials[i]
		var but = create_selector_item(mat.Name)
		
		but.connect("pressed", editor, "_select_material", [i])
		selector_matlist.add_child(create_lineno(i))
		selector_matlist.add_child(but)

