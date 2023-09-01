extends Spatial
var uid = ""
var icon = preload("res://ui/icon_lightbulb.png")
var icon_scale = 16

var highlight_model
var parent_node: Spatial

const MDL_ARROW = preload("res://scenes/editor/mdl_arrow.tscn")

var child_light: OmniLight
var child_icon: Sprite3D
var child_text: Label3D


var displayname: String
var unknown_0x14: int
var unknown_0x18: int
var unknown_0x1c: int
var unknown_floats: Array
var unknown_0x28: int
var unknown_0x30: int
var unknown_0x34: float
var unknown_0x38: float

var unknown_0x6c: float
var unknown_0x70: float
var radius_inner: float
var radius_outer: float
var render_distance: float
var parent: int
var unknown0x88: int
var unknown0x8c: int


var data

func _init():
	set_meta("chunk_object", true)

func _setup(lightdata):
	"""
	string name
		UInt32 Flags
		Sr2RGBJSON Color
		UInt32 Unknown0x14
		UInt32 Unknown0x18
		UInt32 Unknown0x1c
		Single[] UnknownFloats
		Int32 Unknown0x28
		UInt32 Unknown0x30
		Single Unknown0x34
		Single Unknown0x38
		Sr2Vector3JSON TransformPosition
		Sr2Vector3JSON TransformBasisX
		Sr2Vector3JSON TransformBasisY
		Sr2Vector3JSON TransformBasisZ
		Single Unknown0x6c
		Single Unknown0x70
		Single RadiusInner
		Single RadiusOuter
		Single RenderDistance
		Int32 Parent
		UInt32 Unknown0x88
		UInt32 Unknown0x8c
		UInt32 Type"""

func _ready():
	#child_light = OmniLight.new()
	#child_light.translation = translation
	#add_child(child_light)
	
	child_icon = Sprite3D.new()
	child_icon.scale *= icon_scale
	child_icon.billboard = SpatialMaterial.BILLBOARD_ENABLED
	child_icon.texture = icon
	child_icon.translation.y -= .52
	add_child(child_icon)
	
	child_text = Label3D.new()
	child_text.scale *= icon_scale
	child_text.billboard = SpatialMaterial.BILLBOARD_ENABLED
	child_text.translation.y -= .52
	#add_child(child_text)
	
	# Collider grabs clicks
	var collider = StaticBody.new()
	var colshape = CollisionShape.new()
	colshape.shape = SphereShape.new()
	colshape.shape.radius = 1.3
	collider.add_child(colshape)
	add_child(collider)
	#collider.connect("input_event", self, "_input_event")
	
	refresh()
	
	highlight_model = Spatial.new()
	add_child(highlight_model)
	highlight_model.add_child(MDL_ARROW.instance())
	
func _set_basis(basis: Basis):
	global_transform.basis = basis# Basis(Vector3(0,1,0),Vector3(0,0,1),Vector3(1,0,0))

func _set_highlight(yes):
	var cursor = ChunkEditor.threedee_cursor
	if cursor != null:
		if yes and data.Parent >= 0:
			cursor.show()
			cursor._update_cursor(parent_node, "parent: " + parent_node.displayname)
		else:
			cursor.hide()


func _change_color(col):
	data.Color.R = col.r
	data.Color.G = col.g
	data.Color.B = col.b
	print("new color: ", data.Color)
	refresh()

func refresh_data():
	data.Origin.X = -transform.origin.x
	data.Origin.Y = transform.origin.y
	data.Origin.Z = transform.origin.z
	data.BasisX.X = transform.basis.x.x
	data.BasisX.Y = -transform.basis.x.y
	data.BasisX.Z = -transform.basis.x.z
	data.BasisY.X = -transform.basis.y.x
	data.BasisY.Y = transform.basis.y.y
	data.BasisY.Z = transform.basis.y.z
	data.BasisZ.X = -transform.basis.z.x
	data.BasisZ.Y = transform.basis.z.y
	data.BasisZ.Z = transform.basis.z.z

func refresh():
	displayname = data.Name
	child_icon.modulate = Color(data.Color.R, data.Color.G, data.Color.B)
	child_text.text = "\n\n\n" + data.Name
	parent_node = Globals.chunk
	if data.Parent > -1:
		parent_node = Globals.chunk.cityobjects[data.Parent]
	get_parent().remove_child(self)
	parent_node.add_child(self)
	translation = Vector3(-data.Origin.X ,data.Origin.Y, data.Origin.Z)
	transform.basis = Basis(
		Vector3(data.BasisX.X ,-data.BasisX.Y, -data.BasisX.Z),
		Vector3(-data.BasisY.X ,data.BasisY.Y, data.BasisY.Z),
		Vector3(-data.BasisZ.X ,data.BasisZ.Y, data.BasisZ.Z)
		)
