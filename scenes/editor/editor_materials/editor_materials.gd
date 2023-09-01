extends HBoxContainer

onready var selector: PanelContainer = PanelContainer.new()
onready var texviewer: PanelContainer = PanelContainer.new()
onready var matviewer: PanelContainer = PanelContainer.new()
const SCRIPT_SELECTOR = preload("res://scenes/editor/scripts/selector/selector_material.gd")
const SCRIPT_TEXVIEW = preload("res://scenes/editor/editor_materials/texture_viewer.gd")
const SCRIPT_MATVIEW = preload("res://scenes/editor/editor_materials/material_viewer.gd")
const TEX_FALLBACK = preload("res://ui/icon_molari0.png")


func _ready():
	add_constant_override("separation", 0)
	
	selector.set_script(SCRIPT_SELECTOR)
	add_child(selector)
	
	texviewer.set_script(SCRIPT_TEXVIEW)
	add_child(texviewer)
	matviewer.set_script(SCRIPT_MATVIEW)
	add_child(matviewer)
	_select_texture(0)
	
func refresh():
	selector.refresh()

func _select_texture(idx: int):
	texviewer.show()
	matviewer.hide()
	texviewer._select(idx)

func _select_material(idx: int):
	matviewer.show()
	texviewer.hide()
	matviewer._select(idx)

func _get_texture(idx: int):
	if !is_instance_valid(Globals.chunk):
		return TEX_FALLBACK
	return TEX_FALLBACK
	var tex_path = Globals.chunk.path_unpack_dir + "/textures/wario.png"
	var img = Image.new()
	if img.load(tex_path):
	   return TEX_FALLBACK
	var tex = ImageTexture.new()
	tex.create_from_image(img)
	return tex

func _get_texture_name(idx: int):
	if Globals.chunk == null:
		return "Failed fetching texture name! (0)"
	if idx >= Globals.chunk.texture_names.size():
		return "Failed fetching texture name! (1)"
	return Globals.chunk.texture_names[idx]
	

func _get_material(idx: int):
	if Globals.chunk == null:
		return "Failed fetching material! (0)"
	if idx >= Globals.chunk.materials.size():
		return "Failed fetching material! (1)"
	return Globals.chunk.materials[idx]
