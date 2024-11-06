using System;
using System.Threading;
using Godot;
public partial class Layer : Node3D {
	int length, width;
	Random rng = new Random();
	GodotThread thread = new GodotThread();
	Chunk[][] chunks;
	int[][] unreadyChunks;
	Godot.Vector3 playerPosition = new Godot.Vector3(-999,-999,-999);
	CharacterBody3D player;
	System.Threading.Mutex mutex = new System.Threading.Mutex(false);
	FastNoiseLite _noise;
	FastNoiseLite noise { 
		get {
			if(_noise == null) {
				FastNoiseLite newNoise = new FastNoiseLite();
				newNoise.CellularJitter = 1;
				newNoise.Seed = rng.Next();
				newNoise.FractalType = FastNoiseLite.FractalTypeEnum.None;
				newNoise.DomainWarpType = FastNoiseLite.DomainWarpTypeEnum.Simplex;
				newNoise.FractalOctaves = 4;
				newNoise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
				_noise = newNoise;
			}
			return _noise;
		}
	}

	public Layer(int _length, int _width, CharacterBody3D _player) {
		length = _length;
		width = _width;
		player = _player;
		playerPosition = Common.WorldToChunkCoords(player.Position);
		chunks = new Chunk[width][];
		unreadyChunks = new int[width][];
		for(int row = 0; row < width; row++) {
			Chunk[] rowArr = new Chunk[length];
			int[] unreadyArr = new int[length];
			for(int col = 0; col < length; col++) {
				rowArr[col] = null;
				unreadyArr[col] = 0;
			}
			chunks[row] = rowArr;
			unreadyChunks[row] = unreadyArr;
		}
		LoadChunks(playerPosition);
	}
	void LoadChunks(Godot.Vector3 pos) {
		for(int row = 0; row < length; row++) {
			for(int col = 0; col < width; col++) {
				ChunkLoader loader = new ChunkLoader(row, col, AddChunk);
				mutex.WaitOne();
				Thread thread = new Thread(new ThreadStart(loader.Start));
				thread.Start();
			}
		}
	}
	void AddChunk(int x, int z) {
		if(chunks[x][z] != null | unreadyChunks[x][z] != 0) {
			return;
		}
		unreadyChunks[x][z] = 1;
		LoadChunk(x,z);
	}
	void LoadChunk(int x, int z) {
		Chunk chunk = new Chunk(noise, x * Common.CHUNKSIZE, z * Common.CHUNKSIZE, noise.GetNoise2D(x,z));
		chunk.Name = $"Chunk {chunk.X} {chunk.Z}";
		chunk.Position = new Godot.Vector3(x * Common.CHUNKSIZE, 0, z * Common.CHUNKSIZE);
		chunks[x][z] = chunk;
		unreadyChunks[x][z] = 0;
		CallDeferred("PlaceChunk", chunk);
	}
	void PlaceChunk(Chunk chunk) {
		AddChild(chunk);
		chunk.Active = true;
		mutex.ReleaseMutex();
	}
	public override void _Process(double delta) {
		Godot.Vector3 currentPosition = Common.WorldToChunkCoords(player.Position);
		if(playerPosition != currentPosition) {
			playerPosition = currentPosition;
			PlayerPositionUpdated();
		}
	}
	void PlayerPositionUpdated() {
		foreach(Chunk[] row in chunks) {
				foreach(Chunk chunk in row) {
					Godot.Vector3 chunkPos = Common.WorldToChunkCoords(chunk.Position);
					float absX = Math.Abs(chunkPos.X - playerPosition.X);
					float absZ = Math.Abs(chunkPos.Z - playerPosition.Z);
					if(absX + absZ <= 5) {
						chunk.Active = true;
					} else if(chunk.Active == true) {
						chunk.Active = false;
					}
				}
			}
	}
}
