extends Spatial

var icon = preload("res://ui/icon_lightbulb.png")
var icon_scale = 16

var color: Color

var child_light: OmniLight
var child_icon: Sprite3D
var child_text: Label3D


func _ready():
	child_light = OmniLight.new()
	child_light.translation = translation
	add_child(child_light)
	
	child_icon = Sprite3D.new()
	child_icon.scale *= icon_scale
	child_icon.billboard = SpatialMaterial.BILLBOARD_ENABLED
	child_icon.texture = icon
	add_child(child_icon)
	
	child_text = Label3D.new()
	child_text.scale *= icon_scale
	child_text.billboard = SpatialMaterial.BILLBOARD_ENABLED
	add_child(child_text)
	
	# Collider grabs clicks
	var collider = StaticBody.new()
	var colshape = CollisionShape.new()
	colshape.shape = SphereShape.new()
	colshape.shape.radius = 1.3
	colshape.translation.y += .52
	collider.add_child(colshape)
	add_child(collider)
	collider.connect("input_event", self, "_input_event")
	
	_update()
	
func _set_highlight(_temp):
	pass

func _change_color(col):
	color = col
	_update()
	
func _update():
	child_light.light_color = color
	child_icon.modulate = color
	child_text.text = "\n\n\n" + name

func _input_event(_camera, event, _click_position, _click_normal, _shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			print("clicked ", name)
			ChunkEditor._select(self)
