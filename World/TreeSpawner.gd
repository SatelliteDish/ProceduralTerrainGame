extends Node3D

class_name Tree_Spawner

var world_position: Vector3

const vertices = [
	Vector3(0,0,0),
	Vector3(1,0,0),
	Vector3(0,1,0),
	Vector3(1,1,0),
	Vector3(0,0,1),
	Vector3(1,0,1),
	Vector3(0,1,1),
	Vector3(1,1,1),
]

const TOP = [2,3,7,6]
const BOTTOM = [0,4,5,1]
const LEFT = [6,4,0,2]
const RIGHT = [3,1,5,7]
const FRONT = [7,5,4,6]
const BACK = [2,0,1,3]

var blocks: Array = []
var mesh: Mesh
var mesh_instance: MeshInstance3D
var st = SurfaceTool.new()

func _init(pos: Vector3):
	self.world_position = pos

func update():
	mesh = Mesh.new()
	mesh_instance = MeshInstance3D.new()
	
func create_block(x,y,z):
	blocks[x][z] = y	
	create_face(TOP,x,y,z)
	create_face(BOTTOM,x,y,z)
	create_face(LEFT,x,y,z)
	create_face(RIGHT,x,y,z)
	create_face(FRONT,x,y,z)
	create_face(BACK,x,y,z)

func create_face(face,x,y,z):
	var offset = Vector3(x,y,z)
	var corner1 = vertices[face[0]] + offset
	var corner2 = vertices[face[1]] + offset
	var corner3 = vertices[face[2]] + offset
	var corner4 = vertices[face[3]] + offset
	st.add_triangle_fan([corner1,corner2,corner3])
	st.add_triangle_fan([corner1,corner3,corner4])
