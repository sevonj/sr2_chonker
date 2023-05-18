extends Node
"""
chunk.gd

This script holds manages objects and data of a loaded chunk.

"""

# Contains a reference to every object in arrays below
# Keys are the UID's of the object, such as "light_123" or "cobj_456"
var objects_by_uid: Dictionary

var rendermodels: Array
var physmodels: Array

# Arrays of objects by type
var baked_collision: MeshInstance
var cityobjects: Array
var lights: Array

func _load():
	objects_by_uid = {}
	
	objects_by_uid["baked_collision"] = baked_collision
	baked_collision.hide()
	
	for obj in cityobjects:
		objects_by_uid[obj.uid] = obj
		
	for obj in lights:
		objects_by_uid[obj.uid] = obj


func _baked_collision_visible(vis: bool):
	baked_collision.visible = vis

func _rendermodel_visible(vis: bool):
	for obj in cityobjects:
		obj.visible = vis
		
func _light_visible(vis: bool):
	for obj in lights:
		obj.visible = vis
