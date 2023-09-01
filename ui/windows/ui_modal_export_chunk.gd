extends WindowDialog
class_name UiModalExportChunk

var file_input
onready var label_filestatus = RichTextLabel.new()

onready var input_graphics = MenuHelper.input_checkbox("Import graphics (can't disable yet)", true, true)
onready var input_textures = MenuHelper.input_checkbox("Import textures", false, true)

onready var input_ok = Button.new()

func _ready():
	show()
	window_title = "Import Chunk"
	#popup_exclusive = true
	rect_position.x = 200
	rect_position.y = 200
	rect_size.x = 300
	rect_size.y = 256
	
	var vbox = VBoxContainer.new()
	vbox.margin_top = 0
	vbox.margin_left = 8
	vbox.margin_bottom = 0
	vbox.margin_right = -8
	vbox.anchor_right = 1
	vbox.anchor_bottom = 1
	add_child(vbox)
	
	var file_title = Label.new()
	file_title.text = ""
	vbox.add_child(file_title)
	var file_open = Button.new()
	file_open.text = "Select file"
	file_open.connect("pressed", self, "_open_file")
	vbox.add_child(file_open)
	file_input = LineEdit.new()
	vbox.add_child(file_input)
	
	vbox.add_child(label_filestatus)
	label_filestatus.bbcode_enabled = true
	label_filestatus.fit_content_height = true
	vbox.add_child(input_graphics)
	vbox.add_child(input_textures)
		
	input_ok.text = "Ok"
	input_ok.disabled = true
	input_ok.connect("pressed", self, "_ok")
	vbox.add_child(input_ok)

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
	
func _set_file(f: String):
	if f.empty():
		return
	var fext = f.get_extension()
	if fext != "chunk_pc" and fext != "g_chunk_pc" and fext != "peg_pc" and fext != "g_peg_pc":
		label_filestatus.bbcode_text = "[color=red]ERR: File extension doesn't match any chunk format!"
		input_textures.pressed = false
		input_textures.disabled = true
		return
		
	f = f.get_basename()
	
	var path_chunk_pc = f + ".chunk_pc"
	var path_g_chunk_pc = f + ".g_chunk_pc"
	var path_peg_pc = f + ".peg_pc"
	var path_g_peg_pc = f + ".g_peg_pc"
	
	file_input.text = path_chunk_pc
	input_ok.disabled = false
	
	
	input_textures.pressed = true
	input_textures.disabled = false
	
	var file = File.new()
	if !file.file_exists(path_chunk_pc):
		label_filestatus.bbcode_text = "[color=red]ERR: .chunk_pc   file missing - Cannot continue"
	else:
		label_filestatus.bbcode_text = "[color=#00ff00]OK:  .chunk_pc"
	if !file.file_exists(path_g_chunk_pc):
		label_filestatus.bbcode_text += "\n[color=red]ERR: .g_chunk_pc file missing - Cannot continue"
	else:
		label_filestatus.bbcode_text += "\n[color=#00ff00]OK:  .g_chunk_pc"
	if !file.file_exists(path_peg_pc):
		label_filestatus.bbcode_text += "\n[color=yellow]ERR: .peg_pc     file missing - Textures disabled"
		input_textures.pressed = false
		input_textures.disabled = true
	else:
		label_filestatus.bbcode_text += "\n[color=#00ff00]OK:  .peg_pc"
	if !file.file_exists(path_g_peg_pc):
		label_filestatus.bbcode_text += "\n[color=yellow]ERR: .g_peg_pc   file missing - Textures disabled"
		input_textures.pressed = false
		input_textures.disabled = true
	else:
		label_filestatus.bbcode_text += "\n[color=#00ff00]OK:  .g_peg_pc"
	

func _ok():
	var fext = file_input.text.get_extension()
	if fext == "chunk_pc" or fext == "g_chunk_pc" or fext == "peg_pc" or fext == "g_peg_pc":
		ChunkEditor.opt_rendermodels = true
		ChunkEditor.opt_unpack = true
		ProjectMan.import_chunk(file_input.text)
		queue_free()
	else:
		label_filestatus.bbcode_text = "[color=red]ERR: Invalid file extension!"
