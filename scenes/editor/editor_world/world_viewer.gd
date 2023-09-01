extends Control


onready var scene_cam: Camera = ChunkEditor.cam.get_node("pivot").get_node("cam")
onready var gizmo_cam: Camera = ChunkEditor.gizmo_cam
onready var main: Spatial = get_tree().root.get_node("main")
onready var gizmo = ChunkEditor.gizmo
onready var panel_layers

const PANEL_MARGIN = 4
const PANEL_STYLE = preload("res://ui/stylebox_viewport_panel.tres")
const ICON_DROPDOWN = preload("res://ui/icon_dropdown.png")

func _ready():
	rect_clip_content = true
	
	# Ignore mouse, otherwise events are marked handled, even for this node itself!
	mouse_filter = Control.MOUSE_FILTER_IGNORE
	create_panel_layers()

# TODO: turn this into a reusable gui element
func create_panel_layers():
	var panel_layers_toggle = Button.new()
	var w = 20
	var h = 20
	var m = PANEL_MARGIN
	panel_layers_toggle.add_stylebox_override("normal", PANEL_STYLE)
	panel_layers_toggle.anchor_left = 1
	panel_layers_toggle.rect_size = Vector2.ZERO
	panel_layers_toggle.margin_right = -m
	panel_layers_toggle.margin_top = m
	panel_layers_toggle.toggle_mode = true
	
	panel_layers_toggle.grow_horizontal = Control.GROW_DIRECTION_BEGIN
	panel_layers_toggle.text = "Layers"
	panel_layers_toggle.icon = ICON_DROPDOWN
	panel_layers_toggle.icon_align  = Button.ALIGN_RIGHT
	panel_layers_toggle.focus_mode = Control.FOCUS_NONE
	add_child(panel_layers_toggle)
	
	panel_layers = PanelContainer.new()
	var vbox = VBoxContainer.new()
	
	panel_layers.add_child(vbox)
	vbox.add_constant_override("separation", 4)
	vbox.add_child(Control.new()) # Add some margin
	var input_layer_bakedcol = MenuHelper.input_checkbox("Baked Collision")
	vbox.add_child(input_layer_bakedcol)
	var input_layer_rendermodels = MenuHelper.input_checkbox("Viewmodels")
	vbox.add_child(input_layer_rendermodels)
	var input_layer_lights = MenuHelper.input_checkbox("Lights")
	vbox.add_child(input_layer_lights)
	vbox.add_child(Control.new()) # Add some margin
	
	input_layer_rendermodels.pressed = true
	input_layer_lights.pressed = true
	input_layer_bakedcol.connect("toggled", ChunkEditor, "_baked_collision_visible")
	input_layer_rendermodels.connect("toggled", ChunkEditor, "_rendermodels_visible")
	input_layer_lights.connect("toggled", ChunkEditor, "_lights_visible")
	
	panel_layers.anchor_left = 1
	panel_layers.anchor_top = 1
	panel_layers.grow_horizontal = Control.GROW_DIRECTION_BEGIN
	panel_layers.margin_top = m
	panel_layers_toggle.add_child(panel_layers)
	panel_layers_toggle.connect("toggled", self, "_set_visible", [panel_layers])
	panel_layers.hide()
	panel_layers.add_stylebox_override("panel", PANEL_STYLE)

func _set_visible(vis: bool, target):
	target.visible = vis

func _unhandled_input(event):
	# Visibility Guard: Disable input this menu is not active
	if !is_visible_in_tree():
		return
	# Drag Guard: When dragging transform gizmo 
	if gizmo.dragging:
		return
	# Rect Guard: only capture events when mouse is within rect
	var mousepos = get_viewport().get_mouse_position()
	var minpos = rect_global_position
	var maxpos = rect_global_position + rect_size
	if minpos.x > mousepos.x or mousepos.x > maxpos.x or minpos.y > mousepos.y or mousepos.y > maxpos.y:
		return
	
	
	if event is InputEventMouseButton:
		match event.button_index:
			
			# Camera Zoom
			BUTTON_WHEEL_UP: ChunkEditor.cam._zoom_in()
			BUTTON_WHEEL_DOWN: ChunkEditor.cam._zoom_out()
			
			# Camera Control
			BUTTON_MIDDLE:
				if event.pressed:
					Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
				else:
					Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
			
			BUTTON_LEFT:
				# Clicked Transform Gizmo
				if event.pressed:
					var pos = event.position
					var gview_pos = pos - gizmo.get_parent().get_parent().rect_global_position
					var from = gizmo_cam.project_ray_origin(gview_pos)
					var to = from + gizmo_cam.project_ray_normal(gview_pos) * Globals.UI_WORLD_CLICK_DISTANCE
					var result = gizmo_cam.get_world().direct_space_state.intersect_ray(from, from + to)
					if !result.empty():
						gizmo._initiate_drag(result.collider.name)
						return
				
				# Clicked object in 3d space
				if event.pressed:
					var pos = event.position
					var from = scene_cam.project_ray_origin(pos)
					var to = from + scene_cam.project_ray_normal(pos) * Globals.UI_WORLD_CLICK_DISTANCE - scene_cam.global_transform.origin
					var result = scene_cam.get_world().direct_space_state.intersect_ray(from, from + to)
					if !result.empty():
						var result_node = result.collider.get_parent()
						if result_node.has_meta("chunk_object"):
							ChunkEditor._select(result_node.uid)
							return
						# Cityobject colliders are nested one step deeper
						result_node = result_node.get_parent()
						if result_node.has_meta("chunk_object"):
							ChunkEditor._select(result_node.uid)
							return
					ChunkEditor._unselect()
	
	# Camera Control
	if event is InputEventMouseMotion and Input.is_mouse_button_pressed(BUTTON_MIDDLE):
		ChunkEditor.cam._mouseinput(event)
		return
	
	# Transform Gizmo hover
	if event is InputEventMouseMotion:
		var pos = event.position
		var gview_pos = pos - gizmo.get_parent().get_parent().rect_global_position
		var from = gizmo_cam.project_ray_origin(gview_pos)
		var to = from + gizmo_cam.project_ray_normal(gview_pos) * Globals.UI_WORLD_CLICK_DISTANCE
		var result = gizmo_cam.get_world().direct_space_state.intersect_ray(from, from + to)
		if !result.empty():
			gizmo._highlight(result.collider.name)
		else:
			gizmo._highlight("null")
	
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_ESCAPE:
			ChunkEditor._unselect()
			return
		if Input.is_key_pressed(KEY_F):
			ChunkEditor._focus()
			return
