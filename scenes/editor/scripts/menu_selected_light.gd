extends Control

var target

onready var namelabel = $vbox/name
onready var input_posx = $vbox/pos_inputs/input_x
onready var input_posy = $vbox/pos_inputs/input_y
onready var input_posz = $vbox/pos_inputs/input_z
onready var input_color = $vbox/color_input


func _ready():
	ChunkEditor.menu_selected_light = self
	hide()
	
	input_posx.connect("text_changed", self, "_update_pos")
	input_posy.connect("text_changed", self, "_update_pos")
	input_posz.connect("text_changed", self, "_update_pos")
	input_color.connect("color_changed", self, "_update_color")

func _select(cityobj):
	target = cityobj
	
	namelabel.text = target.name
	input_posx.text = str(target.translation.x)
	input_posy.text = str(target.translation.y)
	input_posz.text = str(target.translation.z)
	
func _update_pos():
	var deltapos = Vector3(
		float(input_posx.text),
		float(input_posy.text),
		float(input_posz.text)
		)
	target.translation = deltapos

func _update_color(color:  Color):
	input_color.color = color
	target._change_color(color)
	
