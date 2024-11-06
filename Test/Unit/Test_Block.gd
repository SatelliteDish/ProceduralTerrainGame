extends GutTest

var pos: Vector3
var block: Block
func before_all():
	pos = Vector3(
				randi_range(0,Common.chunk_size),
				randi_range(0,Common.chunk_size),
				randi_range(0,Common.chunk_size))
	block = Block.new(pos,1,1,1)

func test_position():
	var x_same = block.position.x == pos.x
	var y_same = block.position.y == pos.y
	var z_same = block.position.z == pos.z
	assert_eq(x_same and y_same and z_same, true, "Is the position set correcty?")

func test_faces_set():
	var faces = [
			Block.FaceType.TOP,
			Block.FaceType.BOTTOM,
			Block.FaceType.LEFT,
			Block.FaceType.RIGHT,
			Block.FaceType.FRONT,
			Block.FaceType.BACK
		]
	var unchanged: bool = false
	for face in faces:
		block.set_face(face,false)
		if(block._faces[face] == true):
			unchanged = true
	assert_eq(unchanged, false, "Are all faces changed correctly?")
