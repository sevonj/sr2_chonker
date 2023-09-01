extends Spatial
class_name Chunk
"""
chunk.gd

This script holds manages objects and data of a loaded chunk.

"""

var working_dir: String
var path_project_file: String
var project: Dictionary

# Contains a reference to every object in arrays below
# Keys are the UID's of the object, such as "light_123" or "cobj_456"
var objects_by_uid: Dictionary

var physmodels: Array
var texture_names: Array 
#var material_names: Array
var materials: Array
var rendermodels: Array

# Extra data
var object_rendermodel_unknowns: Array
var object_num_models: int
var object_unknown3s: Array
var object_unknown4s: Array
var object_transforms: Array
var lights_unknown0x04: int

# Nodes
var baked_collision: MeshInstance
var cityobjects: Array
var lights: Array

const Sr2Cobj = preload("res://scenes/editor/scripts/cityobject.gd")
const Sr2Light = preload("res://scenes/editor/scripts/lightsource.gd")


func _load():
	objects_by_uid = {}
	
	var project_file = File.new()
	if not project_file.file_exists(path_project_file):
		return
	project_file.open(path_project_file, File.READ)
	project = parse_json(project_file.get_as_text())
	working_dir = path_project_file.get_base_dir()
	
	_load_objects()
	_load_materials()
	_load_lights()
	
	#objects_by_uid["baked_collision"] = baked_collision
	#baked_collision.hide()
	
	for obj in cityobjects:
		objects_by_uid[obj.uid] = obj
	for obj in lights:
		objects_by_uid[obj.uid] = obj
		
	swap_endian("aabbccdd")
	swap_endian("aabbccd")
	swap_endian("abbccdd")
func _load_objects():
	# JSON File
	var path = working_dir + "/" + project.path_unpacked_objects_json
	if path.length() <= 0:
		return
	var file = File.new()
	if not file.file_exists(path):
		return
	file.open(path, File.READ)
	var data = parse_json(file.get_as_text())
	object_rendermodel_unknowns = data.RendermodelUnknowns
	object_num_models = data.NumModels
	object_unknown3s = data.Unknown3s
	object_unknown4s = data.Unknown4s
	object_transforms = data.ObjectTransforms
	# Cityobject Nodes
	for i in range(data.Cityobjects.size()):
		var cobj = data.Cityobjects[i]
		var node = Sr2Cobj.new()
		node.data = cobj
		node.tdata = object_transforms[cobj.TransformIdx]
		node.name = "cobj_" + str(i)
		node.uid = node.name
		cityobjects.append(node)
		add_child(node)
		node.refresh()

func _load_materials():
	var path = working_dir + "/" + project.path_unpacked_mat_json
	if path.length() > 0:
		var file = File.new()
		if not file.file_exists(path):
			return
		file.open(path, File.READ)
		var data = parse_json(file.get_as_text())
		
		for texname in data.Textures:
			texture_names.append(texname)
		for material in data.Materials:
			materials.append(material)

func _load_lights():
	var path = working_dir + "/" + project.path_unpacked_lights_json
	if path.length() > 0:
		var file = File.new()
		if not file.file_exists(path):
			return
		file.open(path, File.READ)
		var data = parse_json(file.get_as_text())
		lights_unknown0x04 = data.Unknown0x04
		for i in range(data.Lights.size()):
			var lightdata = data.Lights[i]
			# Some genius decided that in JSON, every number is a float. Excellent for bit flags.
			# TODO: replace this workaround with something safer
			lightdata.Flags = ("0x" + swap_endian(lightdata.Flags)).hex_to_int()
			var node = Sr2Light.new()
			node.data = lightdata
			node.name = "light_" + str(i)
			node.uid = node.name
			objects_by_uid[node.name] = node
			lights.append(node)
			add_child(node)
			node.refresh()

# Hex strings of 32 bit values
func swap_endian(value: String) -> String:
	for i in range(8 - value.length()):
		value = "0" + value
	return value[6] + value[7] + value[4] + value[5] + value[2] + value[3] + value[0] + value[1]

func _save():
	_save_objects()
	#_save_materials()
	_save_lights()

func _save_objects():
	var data: Dictionary = {}
	data.NumRendermodels = rendermodels.size()
	data.RendermodelUnknowns = object_rendermodel_unknowns
	data.ObjectTransforms = object_transforms
	data.NumModels = object_num_models
	data.Unknown3s = object_unknown3s
	data.Unknown4s = object_unknown4s
	data.Cityobjects = []
	# Cityobject Nodes
	for i in range(cityobjects.size()):
		var node = cityobjects[i]
		data.ObjectTransforms[node.data.TransformIdx] = node.tdata
		data.Cityobjects.append(node.data)
	
	# JSON File
	var path = working_dir + "/" + project.path_unpacked_objects_json
	if path.length() > 0:
		var file = File.new()
		file.open(path, File.WRITE)
		file.store_string(to_json(data))

func _save_materials():
	pass

func _save_lights():
	var data = {}
	data.Unknown0x04 = lights_unknown0x04
	data.Lights = []
	for node in lights:
		data.Lights.append(node.data)
	
	# JSON File
	var path = working_dir + "/" + project.path_unpacked_lights_json
	if path.length() > 0:
		var file = File.new()
		file.open(path, File.WRITE)
		file.store_string(to_json(data))


func _baked_collision_visible(vis: bool):
	baked_collision.visible = vis

func _rendermodel_visible(vis: bool):
	for obj in cityobjects:
		obj.visible = vis
		
func _light_visible(vis: bool):
	for obj in lights:
		obj.visible = vis
