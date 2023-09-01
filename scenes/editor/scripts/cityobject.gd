# gityobject.gd
# for chunk editor representation of a cityobject.

extends Spatial

var uid = ""
var is_rendermodel_bad = false

onready var rendermodel_meshinstance = MeshInstance.new()

#var rendermodel_id: int = -1
var cityobjpart_id: int
var is_highlighted = false

const MDL_PLACEHOLDER = preload("res://scenes/editor/mdl_model_placeholder.tres")
const TEX_PLACEHOLDER = preload("res://scenes/editor/tex_model_placeholder.png")

var data: Dictionary # Deserialized JSON
var tdata: Dictionary # Deserialized JSON

var displayname: String

func _init():
	set_meta("chunk_object", true)

func _ready():
	add_child(rendermodel_meshinstance)
	#load_model()
	refresh()

# This had to be done with a delay
func _set_basis(basis: Basis):
	global_transform.basis = basis# Basis(Vector3(0,1,0),Vector3(0,0,1),Vector3(1,0,0))

func set_origin(origin: Vector3):
	transform.origin = origin
	tdata.Origin.X = -origin.x
	tdata.Origin.Y = origin.y
	tdata.Origin.Z = origin.z

func refresh_data():
	tdata.Origin.X = -transform.origin.x
	tdata.Origin.Y = transform.origin.y
	tdata.Origin.Z = transform.origin.z
	tdata.BasisX.X = transform.basis.x.x
	tdata.BasisX.Y = -transform.basis.x.y
	tdata.BasisX.Z = -transform.basis.x.z
	tdata.BasisY.X = -transform.basis.y.x
	tdata.BasisY.Y = transform.basis.y.y
	tdata.BasisY.Z = transform.basis.y.z
	tdata.BasisZ.X = -transform.basis.z.x
	tdata.BasisZ.Y = transform.basis.z.y
	tdata.BasisZ.Z = transform.basis.z.z
	

func refresh():
	displayname = data.Name
	translation = Vector3(-tdata.Origin.X ,tdata.Origin.Y, tdata.Origin.Z)
	transform.basis = Basis(
		Vector3(tdata.BasisX.X ,-tdata.BasisX.Y, -tdata.BasisX.Z),
		Vector3(-tdata.BasisY.X ,tdata.BasisY.Y, tdata.BasisY.Z),
		Vector3(-tdata.BasisZ.X ,tdata.BasisZ.Y, tdata.BasisZ.Z)
		)
	change_model(tdata.ModelIdx)

func change_model(id: int):
	tdata.ModelIdx = id
	#rendermodel_id = id
	is_highlighted = false
	if is_instance_valid(rendermodel_meshinstance):
		if rendermodel_meshinstance.get_child_count() > 0 and is_instance_valid(rendermodel_meshinstance.get_child(0)):
			rendermodel_meshinstance.get_child(0).queue_free() # collider
	load_model()

func load_model():
	if tdata.ModelIdx:
		is_rendermodel_bad = false
		if tdata.ModelIdx < 0:
			var box = CSGBox.new()
			box.translation = translation
			add_child(box)
			return
		
		var rendermodel = Globals.chunk.rendermodels[tdata.ModelIdx] # get_node("/root/main/chunk/rendermodels/RenderModel" + str(rendermodel_id))
		var mat = SpatialMaterial.new()
		
		# Rendermodel OK
		if rendermodel.get_surface_count() > 0:
			rendermodel_meshinstance.mesh = rendermodel
			mat.albedo_color = Color(
				fmod(randf(), .2) +.5,
				fmod(randf(), .2) +.5,
				fmod(randf(), .2) +.5
				)
		# Rendemodel BAD, load placeholder
		else:
			is_rendermodel_bad = true
			#push_warning(name + ": empty mesh")
			rendermodel_meshinstance.mesh = MDL_PLACEHOLDER
			mat.albedo_texture = TEX_PLACEHOLDER
		
		# Material
		mat.params_cull_mode = SpatialMaterial.CULL_DISABLED
		rendermodel_meshinstance.material_override = mat
		
		# Colliders are used to select cityobj by click (Collider uses rendermodel, NOT physmodel)
		rendermodel_meshinstance.create_trimesh_collision()
		#rendermodel_meshinstance.get_child(0).connect("input_event", self, "_input_event")
		

func _set_highlight(highlight: bool):
	if get_child_count() == 0:
		return
		
	if highlight and !is_highlighted:
		get_child(0).material_override.albedo_color *= 5
	elif !highlight and is_highlighted:
		get_child(0).material_override.albedo_color *= .2
	is_highlighted = highlight

func _set_rendermodels_visible(vis: bool):
	rendermodel_meshinstance.visible = vis
