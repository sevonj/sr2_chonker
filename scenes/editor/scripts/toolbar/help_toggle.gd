extends Button

func _ready():
	connect("toggled", self, "_toggled")

func _toggled(togg):
	if togg: get_child(0).show()
	else: get_child(0).hide()
