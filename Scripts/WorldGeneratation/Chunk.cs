using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public partial class Chunk : BlockSpawner {
	float[][] blocks = new float[Common.CHUNKSIZE][];
	Mesh mesh;
	FastNoiseLite noise;
	MeshInstance3D meshInstance;
	public int X { get; set; }
	public int Z { get; set; }
	float wispChance;
	float wispHeight = 2.0f;
	List<Node3D> wisps = new List<Node3D>();
	Material material = GD.Load<Material>("res:/World/test.tres");
	PackedScene wispScene = GD.Load<PackedScene>("res:/World/Wisps/Wisp.tscn");
	bool active = false;
	public bool Active { 
		get => active;
		set {
			if(value == active && meshInstance == null) { return; } 
			if(value == true) {
				AddChild(meshInstance);
			} else {
				RemoveChild(meshInstance);
			}
			SetWisps(value);
			active = value;
		}
	}
	public Chunk(Godot.FastNoiseLite _noise, int x, int z, float _wispChance) {
		X = x;
		Z = z;
		noise = _noise;
		wispChance = _wispChance;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		for(int i = 0; i < Common.CHUNKSIZE; i++) {
			float[] row = new float[Common.CHUNKSIZE];
			blocks[i] = row;
		}
		Update();
	}
	void Update() {
		mesh = new Mesh();
		meshInstance = new MeshInstance3D();
		meshInstance.Name = $"{Name} Ground";
		st.Begin(Mesh.PrimitiveType.Triangles);
		st.SetMaterial(material);
		for(int i = 0; i < Common.CHUNKSIZE; i++) {
			for(int j = 0; j < Common.CHUNKSIZE; j++) {
					bool wispSpawn = (int)GD.RandRange(0,128) == 128;
					float y = Common.RoundToNth(noise.GetNoise2D( i + X, j + Z) * 5,3);
					blocks[i][j] = y;
					Block block = new Block(new Godot.Vector3(i,y,j),1,1,1);
					block.SetFace(Block.FaceType.TOP, true);
					block.SetFace(Block.FaceType.BOTTOM, false);
					block.SetFace(Block.FaceType.LEFT, !BlockIsSolid(new Godot.Vector3(X + 1, y, Z)));
					block.SetFace(Block.FaceType.RIGHT, !BlockIsSolid(new Godot.Vector3(X - 1, y, Z)));
					block.SetFace(Block.FaceType.FRONT, !BlockIsSolid(new Godot.Vector3(X, y, Z + 1)));
					block.SetFace(Block.FaceType.BACK, !BlockIsSolid(new Godot.Vector3(X, y, Z - 1)));
					CreateBlock(block);
					if(wispSpawn) {
						Wisp wisp = (Wisp)wispScene.Instantiate<Node3D>();
						wisp.Position = new Godot.Vector3(i,y + wispHeight, j);
						wisps.Add(wisp);
						AddChild(wisp);
						wisp.Active = false;
					}
			}
		}
		st.GenerateNormals(false);
		mesh = st.Commit();
		meshInstance.Mesh = mesh;
		meshInstance.CreateTrimeshCollision();
		if(Active) {
			AddChild(meshInstance);
		}
	}
	bool BlockIsSolid(Godot.Vector3 vec) {
		int X = (int)vec.X;
		int Z = (int)vec.Z;
		Func<int, bool> TooSmall = val =>  val < 0;
		Func<int, bool> TooBig = val =>  val >= Common.CHUNKSIZE;
		if(TooBig(X) | TooSmall(X) | TooBig(Z) | TooSmall(Z)) {
			return false;
		}
		return blocks[X][Z] == vec.Y;
	}
	void SetWisps(bool active) {
		foreach(Wisp wisp in wisps) {
			if(wisp.Active != active) {
				wisp.Active = active;
			}
		}
	}
}
