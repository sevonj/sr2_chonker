extends HBoxContainer

onready var selector: PanelContainer = PanelContainer.new()
onready var texviewer: PanelContainer = PanelContainer.new()
onready var matviewer: PanelContainer = PanelContainer.new()
const SCRIPT_SELECTOR = preload("res://scenes/editor/scripts/selector/selector_material.gd")
const TEX_FALLBACK = preload("res://ui/icon_molari0.png")

var texture_display 

func _ready():
	# Setup self
	add_constant_override("separation", 0)
	
	# Setup selector
	selector.set_script(SCRIPT_SELECTOR)
	add_child(selector)
	
	
	
	setup_texviewer()
	
func _update():
	selector._update()

func setup_texviewer():
	add_child(texviewer)
	texviewer.size_flags_horizontal = 3
	
	var centercont = CenterContainer.new()
	
	texture_display = TextureRect.new()
	texture_display.texture = TEX_FALLBACK
	texviewer.add_child(texture_display)
