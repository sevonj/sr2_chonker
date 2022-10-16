extends PanelContainer


onready var input_toggle_cityobjs = $hbox/input_toggle_cityobjs
onready var input_toggle_lights = $hbox/input_toggle_lights

func _ready():
	input_toggle_cityobjs.connect("toggled", self, "_set_cityobjs")
	input_toggle_lights.connect("toggled", self, "_set_lights")

func _set_cityobjs(yes):
	get_node("/root/main/chunk/cityobjects").visible = yes
func _set_lights(yes):
	get_node("/root/main/chunk/lights").visible = yes
