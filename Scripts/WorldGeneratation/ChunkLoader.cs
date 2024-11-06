using Godot;
using System;

public sealed class ChunkLoader {
	readonly int x, z;
	public delegate void Callback(int x, int z);
	Callback callback;
	public ChunkLoader(int _x, int _z, Callback _callback) {
		x = _x;
		z = _z;
		callback = _callback;
	}
	public void Start() {
		callback(x,z);
	}
}
