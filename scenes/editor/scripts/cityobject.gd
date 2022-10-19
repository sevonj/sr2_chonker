# gityobject.gd
# for chunk editor representation of a cityobject.

extends Spatial

var is_rendermodel_bad = false

var rendermodel_id: int = -1
var cityobjpart_id: int
var is_highlighted = false

func _ready():
	load_model()

func change_model(id: int):
	rendermodel_id = id
	is_highlighted = false
	if get_child(0):
		get_child(0).queue_free()
	load_model()

func load_model():
	if rendermodel_id:
		is_rendermodel_bad = false
		
		if rendermodel_id < 0:
			var box = CSGBox.new()
			box.translation = translation
			add_child(box)
			return
		
		var rendermodel = ChunkEditor.chunk_rendermodels[rendermodel_id] # get_node("/root/main/chunk/rendermodels/RenderModel" + str(rendermodel_id))
		
		# Abort on empty mesh
		if rendermodel.get_surface_count() == 0:
			is_rendermodel_bad = true
			#push_warning(name + ": empty mesh")
			return
		
		var meshinstance = MeshInstance.new()
		meshinstance.mesh = rendermodel
		
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

func _input_event(_camera, event, _click_position, _click_normal, _shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			print("clicked ", name)
			ChunkEditor._select(self)

func _set_highlight(highlight: bool):
	if get_child_count() == 0:
		return
		
	if highlight and !is_highlighted:
		get_child(0).material_override.albedo_color *= 5
	elif !highlight and is_highlighted:
		get_child(0).material_override.albedo_color *= .2
	is_highlighted = highlight
