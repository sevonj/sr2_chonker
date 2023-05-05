extends WindowDialog


var file_input
onready var opt_cityobjects = ChunkEditor.opt_cityobjects
onready var opt_lights = ChunkEditor.opt_lights


func _ready():
	show()
	window_title = "Import options"
	popup_exclusive = true
	rect_position.x = 200
	rect_position.y = 200
	rect_size.x = 300
	rect_size.y = 200
	
	var vbox = VBoxContainer.new()
	vbox.margin_top = 0
	vbox.margin_left = 0
	vbox.margin_bottom = 0
	vbox.margin_right = 0
	vbox.anchor_right = 1
	vbox.anchor_bottom = 1
	add_child(vbox)
	
	var file_title = Label.new()
	file_title.text = "chunkfile:"
	vbox.add_child(file_title)
	var file_open = Button.new()
	file_open.text = "Select file"
	file_open.connect("pressed", self, "_open_file")
	vbox.add_child(file_open)
	file_input = LineEdit.new()
	vbox.add_child(file_input)
	
	var input_opt_cityobjects = CheckBox.new()
	input_opt_cityobjects.pressed = opt_cityobjects
	input_opt_cityobjects.connect("toggled", self, "_input_opt_cityobjects")
	input_opt_cityobjects.text = "import cityobjects"
	vbox.add_child(input_opt_cityobjects)
	var input_opt_lights = CheckBox.new()
	input_opt_lights.pressed = opt_lights
	input_opt_lights.connect("toggled", self, "_input_opt_lights")
	input_opt_lights.text = "import lights"
	vbox.add_child(input_opt_lights)
	var ok = Button.new()
	ok.text = "Ok"
	ok.connect("pressed", self, "_ok")
	vbox.add_child(ok)

func _process(_delta):
	if !visible:
		queue_free()

func _open_file():
	var dialog = FileDialog.new()
	get_parent().add_child(dialog)
	dialog.show()
	dialog.rect_position.x = 600
	dialog.rect_position.y = 400
	dialog.rect_size.x = 800
	dialog.rect_size.y = 600
	dialog.resizable = true
	dialog.mode = 0
	dialog.access = 2
	dialog.connect("file_selected", self, "_set_file")
	
func _set_file(f):
	file_input.text = f
	
func _input_opt_cityobjects(yes):
	opt_cityobjects = yes
func _input_opt_lights(yes):
	opt_lights = yes

func _ok():
	var fext = file_input.text.get_extension()
	if fext == "chunk_pc" or fext == "g_chunk_pc" or fext == "g_peg_pc":
		ChunkEditor.opt_cityobjects = opt_cityobjects
		ChunkEditor.opt_lights = opt_lights
		ChunkHandler.LoadChunk(file_input.text)
		queue_free()
