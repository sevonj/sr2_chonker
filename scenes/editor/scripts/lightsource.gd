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
