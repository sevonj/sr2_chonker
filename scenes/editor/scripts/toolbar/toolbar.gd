extends PanelContainer

const icon_bakedcollision = preload("res://ui/icon_bakedcollision.png")
const icon_collisionmodels = preload("res://ui/icon_physmodels.png")
const icon_rendemodels = preload("res://ui/icon_rendermodels.png")
const icon_lights = preload("res://ui/icon_lightbulb.png")

var selected_editor
onready var editor_world = get_parent().get_node("editor_world")
onready var editor_materials = get_parent().get_node("editor_materials")


const THEME_SEGBUTTON_L = preload("res://ui/theme_2008_segbutton_l.tres")
const THEME_SEGBUTTON_M = preload("res://ui/theme_2008_segbutton_m.tres")
const THEME_SEGBUTTON_R = preload("res://ui/theme_2008_segbutton_r.tres")

var toolbar_editor_changer


func _ready():
	var hbox = get_node("hbox")
	_change_editor(editor_world)
	toolbar_editor_changer = create_editor_changer()
	hbox.add_child(toolbar_editor_changer)
	ProjectMan.connect("loaded", self, "refresh")

func refresh():
	_change_editor(editor_world)

func create_editor_changer():
	var hbox = HBoxContainer.new()
	hbox.add_constant_override("separation", 0)
	var button_w = 100
	
	var input_editor_world = create_toolbar_button(null, "World", "", true, true)
	input_editor_world.connect("pressed", self, "_change_editor", [editor_world])
	input_editor_world.connect("pressed", self, "_update_editor_changer_buttons", [input_editor_world])
	input_editor_world.rect_min_size.x = button_w
	hbox.add_child(input_editor_world)
	
	var input_editor_materials = create_toolbar_button(null, "Materials", "", true, false)
	input_editor_materials.connect("pressed", self, "_change_editor", [editor_materials])
	input_editor_materials.connect("pressed", self, "_update_editor_changer_buttons", [input_editor_materials])
	input_editor_materials.rect_min_size.x = button_w
	hbox.add_child(input_editor_materials)
	
	for i in range(hbox.get_child_count()):
		var child: Button = hbox.get_child(i)
		if i == 0:
			child.theme = THEME_SEGBUTTON_L
		elif i == hbox.get_child_count() -1:
			child.theme = THEME_SEGBUTTON_R
		else:
			child.theme = THEME_SEGBUTTON_M
		child.focus_mode = Control.FOCUS_NONE
	
	return hbox

func _change_editor(selected: Control):
	selected_editor = selected
	disable_menu(editor_world)
	disable_menu(editor_materials)
	enable_menu(selected_editor)

func _update_editor_changer_buttons(selected: Button):
	for child in toolbar_editor_changer.get_children():
		child.pressed = false
	selected.pressed = true

func enable_menu(node: Control):
	node.show()
	node.set_process(true)
	node.set_physics_process(true)
	node.set_process_unhandled_input(true)
	node.set_process_input(true)
	if node.has_method("refresh"):
		node.refresh()
func disable_menu(node: Control):
	node.hide()
	node.set_process(false)
	node.set_physics_process(false)
	node.set_process_unhandled_input(false)
	node.set_process_input(false)


func create_worldview_toolbar():
	var hbox = HBoxContainer.new()
	var input_visibility_baked_collision = create_toolbar_button(icon_bakedcollision, "", "Baked collision model", true, false)
	input_visibility_baked_collision.connect("toggled", ChunkEditor, "_baked_collision_visible")
	hbox.add_child(input_visibility_baked_collision)
	#var input_visibility_rendermodels = create_toolbar_button(icon_rendemodels, "", "Collision models", true, false)
	#input_visibility_rendermodels.connect("toggled", ChunkEditor, "_lights_visible")
	#hbox.add_child(input_visibility_rendermodels)
	var input_visibility_rendermodels = create_toolbar_button(icon_rendemodels, "", "Rendermodels", true, true)
	input_visibility_rendermodels.connect("toggled", ChunkEditor, "_rendermodels_visible")
	hbox.add_child(input_visibility_rendermodels)
	var input_visibility_lights = create_toolbar_button(icon_lights, "", "Light sources", true, true)
	input_visibility_lights.connect("toggled", ChunkEditor, "_lights_visible")
	hbox.add_child(input_visibility_lights)
	return hbox
	

func create_toolbar_button(icon, text: String = "", tooltip: String = "", togglemode: bool = false, pressed: bool = false):
	var button = Button.new()
	button.icon = icon
	button.text = text
	button.set_tooltip(tooltip)
	button.toggle_mode = togglemode
	button.pressed = pressed
	return button
