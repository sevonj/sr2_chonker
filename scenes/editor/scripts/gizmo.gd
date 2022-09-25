extends Spatial

var target: Spatial

onready var body_x = get_node("body_x")
onready var body_y = get_node("body_y")
onready var body_z = get_node("body_z")

onready var rod_x = get_node("rod_x")
onready var rod_y = get_node("rod_y")
onready var rod_z = get_node("rod_z")

var dragged = false
var start_ray = "test"
var drag_start_position = Vector2(0,0)
var original_transform = null

func _ready():
	ChunkEditor.gizmo = self
