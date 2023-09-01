extends HBoxContainer

onready var worldviewer = $viewport_area
onready var selector = $selector

# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	add_constant_override("separation", 0)

func refresh():
	if !is_instance_valid(selector):
		return
	selector.refresh()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
