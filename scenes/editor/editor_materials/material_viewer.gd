extends PanelContainer


onready var editor = get_parent()
onready var split: VSplitContainer = VSplitContainer.new()
onready var material_display: Panel = Panel.new()
onready var property_display: HBoxContainer = HBoxContainer.new()
onready var property_list: GridContainer = GridContainer.new()
onready var constant_list: VBoxContainer = VBoxContainer.new()
onready var texture_list: VBoxContainer = VBoxContainer.new()
var texture_names: Array = []
var texture_flags: Array = []
var texture_thumbs: Array = []

onready var property_name: Label = Label.new()
onready var property_unknown0: Label = Label.new()
onready var property_shader: Label = Label.new()
onready var property_unknown1: Label = Label.new()
onready var property_unknown2s:VBoxContainer = VBoxContainer.new()
onready var property_flags: Label = Label.new()
onready var property_unknown3: Label = Label.new()


var dragging: bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	size_flags_horizontal = 3
	
	# Setup Material display
	material_display.rect_clip_content = true
	
	# Setup property display
	property_list.columns = 3
	
	add_child(split)
	split.add_child(material_display)
	split.add_child(property_display)
	var property_panel = PanelContainer.new()
	var unknown2_panel = PanelContainer.new()
	var constant_panel = ScrollContainer.new()
	var texture_panel = ScrollContainer.new()
	property_display.add_child(property_panel)
	property_display.add_child(unknown2_panel)
	property_display.add_child(constant_panel)
	property_display.add_child(texture_panel)
	property_panel.size_flags_horizontal = 3
	unknown2_panel.size_flags_horizontal = 3
	constant_panel.size_flags_horizontal = 3
	texture_panel.size_flags_horizontal = 3
	constant_list.size_flags_horizontal = 3
	texture_list.size_flags_horizontal = 3
	
	property_panel.add_child(property_list)
	
	
	#texture_list.columns = 3
	property_list.size_flags_horizontal = 3
	
	texture_panel.add_child(texture_list)
	constant_panel.add_child(constant_list)
	
	#split.split_offset = get_viewport_rect().size.y - 200 # disabled because no material preview
	split.dragger_visibility = SplitContainer.DRAGGER_HIDDEN
	
	create_property_list()
	
	create_constlist()
	create_texturelist()

func create_property_list():
	property_list.add_child(MenuHelper.input_title("(editor only!)"))
	property_list.add_child(MenuHelper.input_title("Name:"))
	property_list.add_child(property_name)
	property_list.add_child(MenuHelper.input_title("u32"))
	property_list.add_child(MenuHelper.input_title("Unknown 0:"))
	property_list.add_child(property_unknown0)
	property_list.add_child(MenuHelper.input_title("u32"))
	property_list.add_child(MenuHelper.input_title("Shader:"))
	property_list.add_child(property_shader)
	property_list.add_child(MenuHelper.input_title("u32"))
	property_list.add_child(MenuHelper.input_title("Unknown1:"))
	property_list.add_child(property_unknown1)
	property_list.add_child(MenuHelper.input_title(""))
	property_list.add_child(MenuHelper.input_title("Unknown 2s:"))
	property_list.add_child(property_unknown2s)
	
	property_list.add_child(MenuHelper.input_title("u32"))
	property_list.add_child(MenuHelper.input_title("Flags:"))
	property_list.add_child(property_flags)
	property_list.add_child(MenuHelper.input_title("i32"))
	property_list.add_child(MenuHelper.input_title("Unknown 3:"))
	property_list.add_child(property_unknown3)

func create_constlist():
	constant_list.add_child(MenuHelper.input_title("Constants"))
	constant_list.add_child(create_const_entry())

func create_const_entry():
	var panel = PanelContainer.new()
	var hbox = HBoxContainer.new()
	var grid = VBoxContainer.new()
	var color_hbox = HBoxContainer.new()
	hbox.add_constant_override("separation", 32)
	#grid.columns = 2
	
	panel.add_child(hbox)
	hbox.add_child(MenuHelper.input_title("0:"))
	hbox.add_child(grid)
	grid.add_child(MenuHelper.input_vec4())
	grid.add_child(color_hbox)
	color_hbox.add_child(MenuHelper.input_title("Edit as color:"))
	color_hbox.add_child(MenuHelper.input_lineedit())
	
	return panel

func refresh_const_entry(idx: int, vec):
	constant_list.get_child(idx).get_node("panel")

func create_texturelist():
	texture_list.add_child(MenuHelper.input_title("Textures"))
	for i in range(16):
		# These are the controls we keep track of
		texture_names.append(Label.new())
		texture_flags.append(Label.new())
		texture_thumbs.append(TextureRect.new())
		texture_names[i].size_flags_horizontal = 3
		texture_thumbs[i].expand = true
		texture_thumbs[i].rect_min_size = Vector2(48,48)
		texture_thumbs[i].stretch_mode = TextureRect.STRETCH_KEEP_ASPECT
		
		# Create the rest
		var panel = PanelContainer.new()
		var hbox = HBoxContainer.new()
		var grid = GridContainer.new()
		
		panel.size_flags_horizontal = 3
		grid.columns = 2
		var tex_no = MenuHelper.input_title(str(i))
		tex_no.rect_min_size.x = 16
		
		texture_list.add_child(panel)
		panel.add_child(hbox)
		hbox.add_child(tex_no)
		hbox.add_child(texture_thumbs[i])
		hbox.add_child(grid)
		grid.add_child(MenuHelper.input_title("Name:"))
		grid.add_child(texture_names[i])
		grid.add_child(MenuHelper.input_title("Flags:"))
		grid.add_child(texture_flags[i])

func _select(idx: int):
	var mat = editor._get_material(idx)
	property_name.text = mat.Name
	property_unknown0.text = String(mat.Unknown0x00)
	property_shader.text = String(mat.Shader)
	property_unknown1.text = String(mat.Unknown0x08)
	for u2 in mat.Unknown2s:
		property_unknown2s.add_child(MenuHelper.input_title(String(u2)))
	for i in range(16):
		var texname = "none"
		var texflag = -1
		var thumb = editor._get_texture(9999)
		if i < mat.Textures.size():
			texname = mat.Textures[i].Name
			texflag = mat.Textures[i].Flags
		texture_names[i].text = texname
		texture_flags[i].text = String(texflag)
		texture_thumbs[i].texture = thumb
	property_flags.text = String(mat.Flags)
	property_unknown3.text = String(mat.Unknown0x14)
