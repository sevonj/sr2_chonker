extends Control

"""
	AppMenu - This is the top left corner file menu
	
"""
onready var but_toggle = $but_toggle

onready var panel = $panel

onready var but_open = $panel/vbox/but_open
onready var but_close = $panel/vbox/but_close
onready var but_save = $panel/vbox/but_save
onready var but_import = $panel/vbox/but_import
onready var but_export = $panel/vbox/but_export

func _ready():
	but_toggle.connect("toggled", self, "toggled")
	
	#but_open.connect("pressed", ProjectMan, "save_chunk")
	but_close.connect("pressed", ChunkEditor, "_clear")
	but_save.connect("pressed", ProjectMan, "save_chunk")
	but_import.connect("pressed", ProjectMan, "import_chunk_dialog")
	but_export.connect("pressed", ProjectMan, "export_chunk")
	
	but_open.connect("pressed", self, "hidemenu")
	but_close.connect("pressed", self, "hidemenu")
	but_save.connect("pressed", self, "hidemenu")
	but_import.connect("pressed", self, "hidemenu")
	but_export.connect("pressed", self, "hidemenu")
	
	panel.hide()

func hidemenu():
	but_toggle.pressed = false

func toggled(tog: bool):
	panel.visible = tog
