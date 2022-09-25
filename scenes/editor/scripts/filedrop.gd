extends Node

onready var chunkhandler = get_node("/root/ChunkHandler")

var filepath = ""

func _ready():
	get_tree().connect("files_dropped", self, "_on_files_dropped")

func _on_files_dropped(files, _screen):
	
	if len(files) != 1:
		print("One file at a time!")
		return
	filepath = files[0]
	chunkhandler.LoadChunk(filepath)
