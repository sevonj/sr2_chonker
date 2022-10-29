extends Control

var target

onready var input_filemenu = $menu_file

onready var input_clear = $input_clear
onready var input_save = $input_save


func _ready():
	input_clear.connect("pressed", ChunkEditor, "_clear")
	input_save.connect("pressed", ChunkEditor, "_save")
	
	input_filemenu.get_popup().add_item("Open")
	input_filemenu.get_popup().connect("id_pressed", self, "_create_fileopen_dialog")

func _create_fileopen_dialog(id):
	match id:
		0: # Load file
			var filedialog = FileDialog.new()
			filedialog.anchor_top = .2
			filedialog.anchor_left = .2
			filedialog.anchor_right = .8
			filedialog.anchor_bottom = .8
			
			filedialog.mode = FileDialog.MODE_OPEN_FILE
			filedialog.access = FileDialog.ACCESS_FILESYSTEM
			filedialog.set_filters(PoolStringArray(["*.chunk_pc ; World Chunk file (CPU)"]))
			filedialog.resizable = true
			filedialog.window_title = "Open chunkfile"
			
			filedialog.connect("file_selected", self, "_fileopen_done")
			# Delete dialog on close
			filedialog.get_close_button().connect("pressed", filedialog, "queue_free")
			# Delete dialog on cancel
			filedialog.get_child(2).get_child(1).connect("pressed", filedialog, "queue_free")
			
			get_node("/root/main").add_child(filedialog)
			filedialog.show()

func _fileopen_done(fname):
	ChunkEditor._load_file(fname)
