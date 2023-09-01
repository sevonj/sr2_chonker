extends Control

var target

onready var input_mdl = $mdl_inputs/input_mdl
onready var input_mdl_ok = $mdl_inputs/input_mdl_ok

var panel_transform
var namelabel

func _ready():
	ChunkEditor.inspector_cityobj = self
	hide()
	input_mdl_ok.connect("pressed", self, "_update_mdl")
	namelabel = Label.new()
	add_child(namelabel)
	
	panel_transform = PanelContainer.new()
	panel_transform.set_script(load("res://scenes/editor/scripts/inspector/panel_transform.gd"))
	add_child(panel_transform)
	panel_transform._create_menu()
	panel_transform.connect("changed", self, "_update_transform")

func _select(cityobj):
	target = cityobj
	
	namelabel.text = target.name
	input_mdl.text = str(target.tdata.ModelIdx)
	
	panel_transform._update_transform(target.transform.origin, target.rotation_degrees, target.scale)


func _update_transform(origin:Vector3, rotation:Vector3, scale:Vector3):
	target.transform.origin = origin
	target.rotation_degrees = rotation
	target.scale = scale
	target.refresh_data()
	#target.transform.basis = Basis(rotation).scaled(scale)

func _update_mdl():
	var id = int(input_mdl.text)
	target.change_model(id)
	#ChunkEditor._update()
	
