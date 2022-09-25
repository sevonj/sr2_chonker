# gityobject.gd
# for chunk editor representation of a cityobject.
# Contains:
# rendermodel stored in a MeshInstance
# TODO: physmodel

extends Spatial

var rendermodel_id
var cityobjpart_id
var is_highlighted = false

func _ready():
	load_model()

func change_model(id):
	rendermodel_id = id
	is_highlighted = false
	if get_child(0):
		get_child(0).queue_free()
	load_model()

func load_model():
	if rendermodel_id:
		var rendermodel = get_node("/root/main/chunk/rendermodels/RenderModel" + str(rendermodel_id))
		
		# Abort on empty mesh
		if rendermodel.mesh.get_surface_count() == 0:
			return
		
		var meshinstance = MeshInstance.new()
		meshinstance.mesh = rendermodel.mesh
		
		# Colliders are used to select cityobj by click (Collider uses rendermodel, NOT physmodel)
		meshinstance.create_trimesh_collision()
		meshinstance.get_child(0).connect("input_event", self, "_input_event")
		
		add_child(meshinstance)
		
		# Material)
		var mat = SpatialMaterial.new()
		mat.albedo_color = Color(
			fmod(randf(), .2) +.5,
			fmod(randf(), .2) +.5,
			fmod(randf(), .2) +.5
			)
		mat.params_cull_mode = SpatialMaterial.CULL_DISABLED
		meshinstance.material_override = mat

func _input_event(camera, event, click_position, click_normal, shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			print("clicked ", name)
			get_node("/root/ChunkEditor")._select_cityobj(self)

func _set_highlight(highlight: bool):
	if highlight and !is_highlighted:
		get_child(0).material_override.albedo_color *= 5
	elif !highlight and is_highlighted:
		get_child(0).material_override.albedo_color *= .2
	is_highlighted = highlight
