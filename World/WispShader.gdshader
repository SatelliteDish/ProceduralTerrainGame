shader_type spatial;

void vertex() {
	VERTEX.y += sin(TIME) * .2;
}

void fragment() {
	ALPHA = .5;
}
