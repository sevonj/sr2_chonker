extends PanelContainer

onready var tree = $Tree
onready var chunk_root = $"/root/main/chunk"

var chunk_object_dict = {}

func _ready():
	ChunkEditor.menu_selector = self
	_update()
	
	tree.connect("item_activated", self, "_on_tree_selected")
	tree.hide_root = true

# Called by ChunkEditor
func _on_select(target: Spatial):
	chunk_object_dict[target.get_path()].select(0)

func _on_tree_selected():
	var selected_name = tree.get_selected().get_text(0)
	var parent_name = tree.get_selected().get_parent().get_text(0)
	match parent_name:
		"chunk_root":
			pass
		"city objects":
			var target = get_node("/root/main/chunk/cityobjects/" + selected_name)
			ChunkEditor._select(target)
			ChunkEditor._focus()
		"light sources":
			var target = get_node("/root/main/chunk/lights/" + selected_name)
			ChunkEditor._select(target)
			ChunkEditor._focus()

func _update():
	tree.clear()
	chunk_object_dict.clear()
	var treeitem_root = tree.create_item()
	treeitem_root.set_text(0, "chunk_root")
	
	if !ChunkEditor.is_chunk_loaded:
		var treeitem_placeholder = tree.create_item()
		treeitem_placeholder.set_text(0, "No chunk loaded.")
		treeitem_placeholder.set_selectable(0, false)
		return
		
	var cobj_root = chunk_root.get_node("cityobjects")
	var light_root = chunk_root.get_node("lights")
	
	var treeitem_cityobjects = tree.create_item()
	treeitem_cityobjects.set_text(0, "city objects")
	var treeitem_lights = tree.create_item()
	treeitem_lights.set_text(0, "light sources")
	
	for i in range(cobj_root.get_child_count()):
		var cobj = cobj_root.get_child(i)
		var treeitem = tree.create_item(treeitem_cityobjects)
		treeitem.set_text(0, cobj.name)
		if cobj.is_rendermodel_bad:
			treeitem.set_custom_color(0, Color.maroon)
			
		chunk_object_dict[cobj.get_path()] = treeitem
		
	for i in range(light_root.get_child_count()):
		var light = light_root.get_child(i)
		var treeitem = tree.create_item(treeitem_lights)
		treeitem.set_text(0, light.name)
		
		chunk_object_dict[light.get_path()] = treeitem
