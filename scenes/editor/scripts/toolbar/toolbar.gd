extends PanelContainer


onready var input_toggle_baked_collision = $hbox/input_toggle_baked_collision
onready var input_toggle_rendermodels = $hbox/input_toggle_rendermodels
onready var input_toggle_lights = $hbox/input_toggle_lights

func _ready():
	input_toggle_baked_collision.connect("toggled", ChunkEditor, "_baked_collision_visible")
	input_toggle_baked_collision.set_tooltip("Baked collison model")
	input_toggle_rendermodels.connect("toggled", ChunkEditor, "_rendermodels_visible")
	input_toggle_rendermodels.set_tooltip("Rendermodels")
	input_toggle_lights.connect("toggled", ChunkEditor, "_lights_visible")
	input_toggle_lights.set_tooltip("Lightsources")
	
