using Godot;
using System.Collections.Generic;

public class Block {
	public enum FaceType {
		TOP,
		BOTTOM,
		LEFT,
		RIGHT,
		FRONT,
		BACK
	}
	Vector3[] _vertices;
	Vector3[] vertices {
		get {
			return _vertices ??= new Vector3[8] {
					new Vector3(0, 0, 0),
					new Vector3(width,0,0),
					new Vector3(0,height,0),
					new Vector3(width,height,0),
					new Vector3(0,0,length),
					new Vector3(width,0,length),
					new Vector3(0,height,length),
					new Vector3(width,height,length),
			};
		}
	}
	float height, width, length;
	public Godot.Vector3 Position { get; set; }
	Dictionary<FaceType,Godot.Vector3[]> _faceVerts;
	public Dictionary<FaceType,Godot.Vector3[]> FaceVerts {
		get {
			return _faceVerts ??= new Dictionary<FaceType, Godot.Vector3[]> {
					{ FaceType.TOP, FacesToVertices(new int[] { 2,3,7,6 }) },
					{ FaceType.BOTTOM, FacesToVertices(new int[] { 0,4,5,1 }) },
					{ FaceType.LEFT, FacesToVertices(new int[] { 6,4,0,2 }) },
					{ FaceType.RIGHT, FacesToVertices(new int[] { 3,1,5,7 }) },
					{ FaceType.FRONT, FacesToVertices(new int[] { 7,5,4,6 }) },
					{ FaceType.BACK, FacesToVertices(new int[] { 2,0,1,3 }) }
				};
		}
	}
	Dictionary<FaceType, bool> faces = new Dictionary<FaceType, bool> {
		{ FaceType.FRONT, true },
		{ FaceType.BACK, true },
		{ FaceType.LEFT, true },
		{ FaceType.RIGHT, true },
		{ FaceType.TOP, true },
		{ FaceType.BOTTOM, true },
	};

	public Block(Godot.Vector3 _position, int _width, int _height, int _length) {
		Position = _position;
		width = _width;
		height = _height;
		length = _length;
	}
	Vector3[] FacesToVertices(int[] faces) {
		Vector3[] results = new Godot.Vector3[faces.Length];
		for(int i = 0; i < faces.Length; i++) {
			results[i] = vertices[faces[i]];
		}
		return results;
	}
	public void SetFace(FaceType face, bool status) {
		faces[face] = status;
	}
	public bool GetFace(FaceType face) {
		return faces[face];
	}
}
