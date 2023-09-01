extends Node

const UI_WINDOWSIZE = Vector2(1920,1080)
const UI_WORLD_CLICK_DISTANCE = 10000
const UI_GIZMO_SIZE = .2
const UI_INSPECTOR_PANELS_STARTOPEN = false
const UI_INSPECTOR_TEXT_MINHEIGHT = 22

const PATH_CHUNK = "/root/main/chunk"
const PATH_CITYOBJECTS = "/root/main/chunk/cityobjects"
const PATH_LIGHTS = "/root/main/chunk/cityobjects"

var on_clear_chunkfile_to_load = null

var chunk: Chunk

func _clear():
	if chunk != null:
		chunk.queue_free()
		chunk = null
