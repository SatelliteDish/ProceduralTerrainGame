using Godot;
using System;

public partial class BlockSpawner : StaticBody3D {
	public SurfaceTool st = new SurfaceTool();

	public void CreateBlock(Block block) {
		foreach(Block.FaceType face in Enum.GetValues(typeof(Block.FaceType))) {
			if(block.GetFace(face)) {
				CreateFace(block, face);
			}
		}
	}
	private void CreateFace(Block block, Block.FaceType face) {
		Godot.Vector3 offset = new Godot.Vector3( block.Position.X, block.Position.Y, block.Position.Z );
		Godot.Vector3 corner1 = block.FaceVerts[face][0] + offset;
		Godot.Vector3 corner2 = block.FaceVerts[face][1] + offset;
		Godot.Vector3 corner3 = block.FaceVerts[face][2] + offset;
		Godot.Vector3 corner4 = block.FaceVerts[face][3] + offset;
		st.AddTriangleFan(new Godot.Vector3[] {corner1,corner2,corner3});
		st.AddTriangleFan(new Godot.Vector3[] {corner1,corner3,corner4});
	}
}
