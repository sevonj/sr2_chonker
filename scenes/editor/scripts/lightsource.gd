extends Spatial
var uid = ""
var icon = preload("res://ui/icon_lightbulb.png")
var icon_scale = 16

var highlight_model

const MDL_ARROW = preload("res://scenes/editor/mdl_arrow.tscn")

var flags = [
	true, #flag_0
	true, #flag_1
	true, #flag_2
	true, #flag_3
	true, #flag_4
	true, #flag_8
	true, #flag_10
	true, #shadow_character
	true, #shadow_level
	true, #light_character
	true, #light_level
	true, #flag_22
]
var color: Color
var unk10: int
var type: int
var radius_inner: float
var radius_outer: float
var render_dist: float
var parent: int

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
	child_icon.translation.y -= .52
	add_child(child_icon)
	
	child_text = Label3D.new()
	child_text.scale *= icon_scale
	child_text.billboard = SpatialMaterial.BILLBOARD_ENABLED
	child_text.translation.y -= .52
	add_child(child_text)
	
	# Collider grabs clicks
	var collider = StaticBody.new()
	var colshape = CollisionShape.new()
	colshape.shape = SphereShape.new()
	colshape.shape.radius = 1.3
	collider.add_child(colshape)
	add_child(collider)
	collider.connect("input_event", self, "_input_event")
	
	_update()
	
	highlight_model = Spatial.new()
	add_child(highlight_model)
	highlight_model.add_child(MDL_ARROW.instance())
	
func _set_basis(basis: Basis):
	global_transform.basis = basis# Basis(Vector3(0,1,0),Vector3(0,0,1),Vector3(1,0,0))

func _set_highlight(_temp):
	pass


func _change_color(col):
	color = col
	_update()
	
func _update():
	child_light.light_color = color
	child_icon.modulate = color
	child_text.text = "\n\n\n" + name
	var p = Globals.chunk
	if parent > -1:
		p = Globals.chunk.cityobjects[parent]
	var t = translation
	get_parent().remove_child(self)
	p.add_child(self)
	translation = t

func _input_event(_camera, event, _click_position, _click_normal, _shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			print("clicked ", name)
			ChunkEditor._select(uid)
