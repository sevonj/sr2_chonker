extends PanelContainer


const TEX_BG = preload("res://ui/tex_alpha.png")

onready var editor = get_parent()
onready var split: VSplitContainer = VSplitContainer.new()
onready var texture_display: Panel = Panel.new()
onready var texture_display_tex: TextureRect = TextureRect.new()
onready var texture_display_bg: TextureRect = TextureRect.new()
onready var property_display: GridContainer = GridContainer.new()
onready var property_filename: Label = Label.new()
onready var property_res: Label = Label.new()

var dragging: bool = false
var drag_start_pos: Vector2

# Called when the node enters the scene tree for the first time.
func _ready():
	size_flags_horizontal = 3
	
	# Setup Texture display
	texture_display.rect_clip_content = true
	texture_display_bg.expand = true # Allows bg tex to shrink if tex is smaller than bg tex
	texture_display_bg.texture = TEX_BG
	texture_display_bg.stretch_mode = TextureRect.STRETCH_TILE
	texture_display_tex.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT
	
	# Setup property display
	property_display.columns = 2
	
	add_child(split)
	split.add_child(texture_display)
	texture_display.add_child(texture_display_bg)
	texture_display.add_child(texture_display_tex)
	split.add_child(property_display)
	property_display.add_child(MenuHelper.input_title("Filename:"))
	property_display.add_child(property_filename)
	property_display.add_child(MenuHelper.input_title("Res:"))
	property_display.add_child(property_res)
	split.split_offset = get_viewport_rect().size.y - 200
	_reset_size()

func _process(_delta):
	texture_display_bg.rect_size = texture_display_tex.rect_size
	texture_display_bg.rect_global_position = texture_display_tex.rect_global_position

func _select(idx: int):
	texture_display_tex.texture = editor._get_texture(idx)
	property_filename.text = editor._get_texture_name(idx)
	var size = texture_display_tex.texture.get_size()
	property_res.text = "%sx%spx" % [size.x, size.y]
	_reset_size()

func _reset_size():
	if texture_display_tex.texture == null:
		return
	texture_display_tex.rect_size = texture_display_tex.texture.get_size()
	texture_display_tex.rect_position = Vector2(10,10) # Gave up trying to center it

func _input(event):
	# Visibility Guard: Disable input this menu is not active
	if !is_visible_in_tree():
		return
	# Make dragging work, even when captured mouse is outside rect
	if dragging:
		# Exit
		if event is InputEventMouseButton and event.button_index == BUTTON_MIDDLE and !event.pressed:
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
			dragging = false
			Input.warp_mouse_position(drag_start_pos)
			return
		# Drag
		if event is InputEventMouseMotion:
			texture_display_tex.rect_position += event.relative
			var size = texture_display_tex.rect_size
			var pos = texture_display_tex.rect_position
			texture_display_tex.rect_position.x = clamp(pos.x, -size.x, texture_display.rect_size.x)
			texture_display_tex.rect_position.y = clamp(pos.y, -size.y, texture_display.rect_size.y)
			return
	
	# Rect Guard: only capture events when mouse is within rect
	var mousepos = get_viewport().get_mouse_position()
	var minpos = texture_display.rect_global_position
	var maxpos = texture_display.rect_global_position + texture_display.rect_size
	if minpos.x > mousepos.x or mousepos.x > maxpos.x or minpos.y > mousepos.y or mousepos.y > maxpos.y:
		return
		
	if event is InputEventMouseButton:
		match event.button_index:
			
			# Texture Zoom
			BUTTON_WHEEL_UP: texture_display_tex.rect_size /= .9
			BUTTON_WHEEL_DOWN: texture_display_tex.rect_size *= .9
			
			# Texture Move MouseMode
			BUTTON_MIDDLE:
				if event.pressed:
					Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
					dragging = true
					drag_start_pos = get_viewport().get_mouse_position()
	
	
