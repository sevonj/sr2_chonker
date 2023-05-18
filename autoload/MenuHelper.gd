extends Node


func create_checkbox(title: String, val):
	var checkbox = CheckBox.new()
	checkbox.text = title
	checkbox.pressed = val
