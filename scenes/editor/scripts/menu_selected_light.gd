extends Control

var target

var panel_flags
var panel_color
var panel_transform
var panel_properties

var flag_names = [
	"flag00",
	"flag01",
	"flag02",
	"flag03",
	"flag04",
	"flag05",
	"flag06",
	"flag07",
	"flag08",
	"flag09",
	"flag0a",
	"flag0b",
	"flag0c",
	"flag0d",
	"flag0e",
	"flag0f",
	"flag10",
	"flag11",
	"shadows_world",
	"shadows_people",
	"flag14",
	"flag15",
	"flag16",
	"flag17",
	"flag18",
	"flag19",
	"flag1a",
	"flag1b",
	"flag1c",
	"flag1d",
	"flag1e",
	"flag1f"
]

func _ready():
	ChunkEditor.menu_selected_light = self
	hide()
	#var vbox = VBoxContainer.new()
	#vbox.size_flags_horizontal = 3 # expand
	#add_child(vbox)
	
	panel_flags = PanelContainer.new()
	panel_flags.set_script(load("res://scenes/editor/scripts/inspector_panel_flags.gd"))
	panel_flags.name = "Flags"
	add_child(panel_flags)
	panel_flags._create_menu(flag_names, self, "_update_flag")
	
	panel_color = PanelContainer.new()
	panel_color.set_script(load("res://scenes/editor/scripts/inspector_panel_color.gd"))
	panel_color.name = "Color"
	add_child(panel_color)
	panel_color._create_menu(self, "_update_color")
	
	panel_transform = PanelContainer.new()
	panel_transform.set_script(load("res://scenes/editor/scripts/inspector_panel_transform.gd"))
	panel_transform.name = "Transform"
	add_child(panel_transform)
	panel_transform._create_menu(self, "_update_transform")
	
	panel_properties = PanelContainer.new()
	panel_properties.set_script(load("res://scenes/editor/scripts/inspector_panel_properties.gd"))
	panel_properties.name = "Properties"
	add_child(panel_properties)
	panel_properties._create_menu(self, "_update_properties")

func _select(cityobj):
	target = cityobj
	
	panel_flags._update_flags(target.flags)
	panel_color._update_color(target.color)
	panel_transform._update_transform(target)
	panel_properties._update_properties(target.radius_inner, target.radius_outer, target.render_dist)

func _update_flag(set, id):
	target.flags[id] = set

func _update_transform(translation, rotation, scale):
	target.set_global_translation(translation)
	target.set_rotation_degrees(rotation)
	target.set_scale(scale)

func _update_color(color:  Color):
	target._change_color(color)

func _update_properties(radius_inner, radius_outer, render_dist):
	target.radius_inner = radius_inner
	target.radius_outer = radius_outer
	target.render_dist = render_dist

