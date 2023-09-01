extends Control

var target

var panel_flags
var panel_color
var panel_transform
var panel_properties

var flag_names = [
	"flag_0",
	"flag_1",
	"flag_2",
	"flag_3",
	"flag_4",
	"flag_8",
	"flag_10",
	"shadow_character",
	"shadow_level",
	"light_character",
	"light_level",
	"flag_22",
]

func _ready():
	ChunkEditor.inspector_light = self
	hide()
	#var vbox = VBoxContainer.new()
	#vbox.size_flags_horizontal = 3 # expand
	#add_child(vbox)
	
	panel_flags = PanelContainer.new()
	panel_flags.set_script(load("res://scenes/editor/scripts/inspector/panel_light_flags.gd"))
	add_child(panel_flags)
	#panel_flags._create_menu(flag_names)
	panel_flags._create_menu()
	panel_flags.connect("changed", self, "_update_flag")
	
	panel_color = PanelContainer.new()
	panel_color.set_script(load("res://scenes/editor/scripts/inspector/panel_light_color.gd"))
	add_child(panel_color)
	panel_color._create_menu()
	panel_color.connect("changed", self, "_update_color")
	
	panel_transform = PanelContainer.new()
	panel_transform.set_script(load("res://scenes/editor/scripts/inspector/panel_transform.gd"))
	add_child(panel_transform)
	panel_transform._create_menu()
	panel_transform.connect("changed", self, "_update_transform")
	
	panel_properties = PanelContainer.new()
	panel_properties.set_script(load("res://scenes/editor/scripts/inspector/panel_light_properties.gd"))
	add_child(panel_properties)
	panel_properties._create_menu()
	panel_properties.connect("changed", self, "_update_properties")
	

func _select(cityobj):
	target = cityobj
	
	panel_flags._update_flags(target.data.Flags)
	panel_color._update_color(
		Color(
			target.data.Color.R,
			target.data.Color.G,
			target.data.Color.B
		))
	panel_transform._update_transform(target.translation, target.rotation_degrees, target.scale)
	panel_properties._update_properties(
		target.data.Unknown0x28,
		target.data.RadiusInner,
		target.data.RadiusOuter,
		target.data.RenderDistance,
		target.data.Parent,
		target.data.Type)
	#panel_properties._update_properties(target.unk10, target.radius_inner, target.radius_outer, target.render_dist, target.parent, target.type)

func _update_flag(set:bool, flag:int):
	if set:
		target.data.Flags |= flag
	else:
		target.data.Flags &= ~flag
	#target.flags[id] = set

func _update_transform(origin:Vector3, rotation:Vector3, scale:Vector3):
	target.translation = origin
	target.rotation_degrees = rotation
	target.scale = scale
	target.refresh_data()
	#target.transform.basis = Basis(rotation).scaled(scale)

func _update_color(color:  Color):
	target._change_color(color)

func _update_properties(unk0x28:int, radius_inner:float, radius_outer:float, render_dist:float, parent:int, type:int):
	target.data.Unknown0x28 = unk0x28
	target.data.RadiusInner = radius_inner
	target.data.RadiusOuter = radius_outer
	target.data.RenderDistance = render_dist
	target.data.Parent = parent
	target.data.Type = type
	target.refresh()


