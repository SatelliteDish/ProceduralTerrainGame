using Godot;
using System;

public partial class SpringArm : SpringArm3D
{
	InputController inputController;
	Node3D body;
	public override void _Ready() {
		inputController = GetNode<InputController>($"../Input Controller");
		body = GetNode<Node3D>($"../Voxel_Character");
		TopLevel = true;
		inputController.PlayerLookEventHandler += look;
	}
	private void look(Godot.Vector2 vec) {
		this.Rotation = new Godot.Vector3(vec.Y,vec.X,Rotation.Z);
	}

}
