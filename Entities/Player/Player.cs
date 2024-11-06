using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody3D {
	[Export] float speed = 10.0f;
	[Export] float jumpStrength = 20.0f;
	[Export] float gravity = 0.25f;//1.25;
	InputController inputController;
	Node3D mesh;
	SpringArm3D camArm;
	Godot.Vector2 moveDirection = new Godot.Vector2(0,0);
	Godot.Vector2 lookDirection = new Godot.Vector2(0,0);

public override void _Ready() {
	inputController = GetNode<InputController>("Input Controller");
	mesh = GetNode<Node3D>("Voxel_Character");
	camArm = GetNode<SpringArm3D>("SpringArm3D");
	camArm.AddExcludedObject(GetRid());
	inputController.PlayerMoveEventHandler += SetMoveDirection;
	inputController.PlayerJumpEventHandler += Jump;
}


public override void _PhysicsProcess(double delta) {
		Godot.Vector3 vel = new Godot.Vector3(0,0,0);
		vel.X = moveDirection.X * speed;
		vel.Z = moveDirection.Y * speed;
		vel.Y -= gravity;
		this.Velocity = vel;
		
		this.Velocity = this.Velocity.Rotated(new Godot.Vector3(0,1,0), camArm.Rotation.Y);
		
		MoveAndSlide();
		
		camArm.Position = this.Position;
		
		if (this.Velocity.Length() > 0.2) {
			Godot.Vector2 lookDirection = new Godot.Vector2(this.Velocity.Z,this.Velocity.X);
			mesh.Rotation = new Godot.Vector3(mesh.Rotation.X,lookDirection.Angle(),mesh.Rotation.Z);
		}
}

public void SetMoveDirection(Godot.Vector2 vector) {
	moveDirection = vector;
}

public void Jump() {
	//if this.Is_On_Floor() {}
	Godot.Vector3 vel = new Godot.Vector3(this.Velocity.X,this.Velocity.Y + jumpStrength, this.Velocity.Z);	
	}
}
