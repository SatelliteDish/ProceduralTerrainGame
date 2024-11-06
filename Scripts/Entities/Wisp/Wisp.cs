using Godot;
using System;

public partial class Wisp : Node3D {
	private bool active = false;
	[Export]MeshInstance3D mesh;
	public bool Active { 
		get => active;
		set {
			if(value == active && mesh == null) return;

			if(!value) {
				RemoveChild(mesh);
			} else {
				AddChild(mesh);
			}
			active = value;
		}
	}
}
