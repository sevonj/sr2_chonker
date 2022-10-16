extends PanelContainer

func _ready():
	ChunkEditor.menu_selected_title = get_child(0)
	ChunkEditor._unselect()
