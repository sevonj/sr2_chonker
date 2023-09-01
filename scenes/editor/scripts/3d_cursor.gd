extends TextureRect

var target: Spatial
onready var camera: Camera = ChunkEditor.cam.get_node("pivot").get_node("cam")

onready var title_label = $title
onready var bg_rect = $rect

func _ready():
	ChunkEditor.threedee_cursor = self
	hide()

func _update_cursor(target_node: Spatial, title_text: String):
	target = target_node
	title_label.text = title_text

func _process(_delta):
	if !is_visible_in_tree():
		return
	if target == null or camera == null:
		return
	rect_global_position = camera.unproject_position(target.global_translation) - rect_size / 2
	bg_rect.rect_position = title_label.rect_position
	bg_rect.rect_size = title_label.rect_size
