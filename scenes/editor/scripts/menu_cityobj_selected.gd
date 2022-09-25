extends Control

var target

onready var namelabel = $vbox/obj_name
onready var input_posx = $vbox/pos_inputs/input_x
onready var input_posy = $vbox/pos_inputs/input_y
onready var input_posz = $vbox/pos_inputs/input_z
onready var input_mdl = $vbox/mdl_inputs/input_mdl
onready var input_mdl_ok = $vbox/mdl_inputs/input_mdl_ok


func _ready():
	ChunkEditor.menu_cityobj_selected = self
	
	input_posx.connect("text_changed", self, "_update_pos")
	input_posy.connect("text_changed", self, "_update_pos")
	input_posz.connect("text_changed", self, "_update_pos")
	input_mdl_ok.connect("pressed", self, "_update_mdl")

func _select(cityobj):
	target = cityobj
	
	namelabel.text = target.name
	input_posx.text = str(target.translation.x)
	input_posy.text = str(target.translation.y)
	input_posz.text = str(target.translation.z)
	input_mdl.text = str(target.rendermodel_id)
	
func _update_pos():
	var deltapos = Vector3(
		float(input_posx.text),
		float(input_posy.text),
		float(input_posz.text)
		)
	target.translation = deltapos

func _update_mdl():
	var id = int(input_mdl.text)
	target.change_model(id)
	
