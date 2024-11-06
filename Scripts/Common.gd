extends Node

const chunk_size: int = 16

var render_distance = 5

func round_to_nth(val: float, n: int):
	#assert(n >= 1,"N must be 1 or greater")
	val = val * n
	val =  int(val)
	val = val/n
	return val

func vec3_to_chunk_coords(vec: Vector3):
	vec.x = int(vec.x/chunk_size)
	vec.y = 0
	vec.z = int(vec.z/chunk_size)
	return vec
