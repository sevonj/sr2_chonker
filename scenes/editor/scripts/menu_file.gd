extends Control

var target
var file_import_dialog = preload("res://scenes/editor/scripts/import_dialog.gd")

onready var input_filemenu = $menu_file
onready var input_clear = $input_clear
onready var input_save = $input_save
onready var ui = get_tree().root.get_node("main").get_node("ui")

const ITEM_IMPORT = 0


func _ready():
	input_filemenu.get_popup().connect("id_pressed", self, "_file_pressed")
	input_clear.connect("pressed", ChunkEditor, "_clear")
	input_save.connect("pressed", ChunkEditor, "_save")
	
	input_filemenu.get_popup().add_item("Import chunk", ITEM_IMPORT)
	

func _file_pressed(id):
	match id:
		ITEM_IMPORT:
			var dialog = file_import_dialog.new()
			ui.add_child(dialog)
			
