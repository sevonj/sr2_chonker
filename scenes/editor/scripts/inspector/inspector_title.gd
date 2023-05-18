extends PanelContainer

func _ready():
	ChunkEditor.inspector_title = get_child(0)
	ChunkEditor._unselect()
