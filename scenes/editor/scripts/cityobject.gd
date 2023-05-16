# gityobject.gd
# for chunk editor representation of a cityobject.

extends Spatial

var uid = ""
var is_rendermodel_bad = false

var rendermodel_id: int = -1
var cityobjpart_id: int
var is_highlighted = false

const MDL_PLACEHOLDER = preload("res://scenes/editor/mdl_model_placeholder.tres")
const TEX_PLACEHOLDER = preload("res://scenes/editor/tex_model_placeholder.png")

func _ready():
	load_model()

func _set_basis(basis: Basis):
	global_transform.basis = basis# Basis(Vector3(0,1,0),Vector3(0,0,1),Vector3(1,0,0))


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
		var model = MeshInstance.new()
		var mat = SpatialMaterial.new()
		
		# Rendermodel OK
		if rendermodel.get_surface_count() > 0:
			model.mesh = rendermodel
			mat.albedo_color = Color(
				fmod(randf(), .2) +.5,
				fmod(randf(), .2) +.5,
				fmod(randf(), .2) +.5
				)
		# Rendemodel BAD, load placeholder
		else:
			is_rendermodel_bad = true
			#push_warning(name + ": empty mesh")
			model.mesh = MDL_PLACEHOLDER
			mat.albedo_texture = TEX_PLACEHOLDER
		
		# Material
		mat.params_cull_mode = SpatialMaterial.CULL_DISABLED
		model.material_override = mat
		
		# Colliders are used to select cityobj by click (Collider uses rendermodel, NOT physmodel)
		model.create_trimesh_collision()
		model.get_child(0).connect("input_event", self, "_input_event")
		add_child(model)
		

func _input_event(_camera, event, _click_position, _click_normal, _shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.is_pressed():
			print("clicked ", name)
			ChunkEditor._select(uid)

func _set_highlight(highlight: bool):
	if get_child_count() == 0:
		return
		
	if highlight and !is_highlighted:
		get_child(0).material_override.albedo_color *= 5
	elif !highlight and is_highlighted:
		get_child(0).material_override.albedo_color *= .2
	is_highlighted = highlight
