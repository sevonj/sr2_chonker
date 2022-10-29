extends Control

var target

onready var namelabel = $vbox/name
onready var input_mdl = $vbox/mdl_inputs/input_mdl
onready var input_mdl_ok = $vbox/mdl_inputs/input_mdl_ok

var panel_transform


func _ready():
	ChunkEditor.menu_selected_cityobj = self
	hide()
	input_mdl_ok.connect("pressed", self, "_update_mdl")
	
	panel_transform = PanelContainer.new()
	panel_transform.set_script(load("res://scenes/editor/scripts/inspector_panel_transform.gd"))
	panel_transform.name = "Transform"
	add_child(panel_transform)
	panel_transform._create_menu(self, "_update_transform")

func _select(cityobj):
	target = cityobj
	
	namelabel.text = target.name
	input_mdl.text = str(target.rendermodel_id)

	panel_transform._update_transform(target)

func _update_transform(translation, rotation, scale):
	target.set_global_translation(translation)
	target.set_rotation_degrees(rotation)
	target.set_scale(scale)

func _update_mdl():
	var id = int(input_mdl.text)
	target.change_model(id)
	ChunkEditor._update()
	
