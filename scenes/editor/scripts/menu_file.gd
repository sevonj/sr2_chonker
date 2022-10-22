extends Control

var target

onready var input_filemenu = $menu_file

onready var input_clear = $input_clear
onready var input_save = $input_save



func _ready():
	input_clear.connect("pressed", ChunkEditor, "_clear")
	input_save.connect("pressed", ChunkEditor, "_save")
	
	input_filemenu.get_popup().add_item("test")
