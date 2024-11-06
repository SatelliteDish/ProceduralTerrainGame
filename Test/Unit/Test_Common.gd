extends GutTest

func before_all():
	randomize()

func test_rounds_to_nth_correctly():
	var n = randi_range(1,10)
	var rounded = Common.round_to_nth(randf(),n)
	assert_almost_eq(fposmod(rounded,(1.0/n)),0.0,.001,"is divisible by 1/n")

func test_rounds_to_nearest_nth():
	var val = randf()
	var n = randi_range(1,10)
	var rounded = Common.round_to_nth(val,n)
	var diff = val - rounded
	assert_lt(diff,1.0/n)

func test_vec3_converted_to_chunk_correctly():
	var ogVec = Vector3(randf(),randf(),randf())
	var rounded = Common.vec3_to_chunk_coords(ogVec)
	var whole_numbers_only = roundi(rounded.x) == rounded.x and roundi(rounded.z) == rounded.z
	var divided_correctly = rounded.x == int(ogVec.x/Common.chunk_size) and rounded.z == int(ogVec.z/Common.chunk_size)
	assert_true(whole_numbers_only and divided_correctly)
