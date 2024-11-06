using Godot;
using System;

public partial class World : Node3D {
    CharacterBody3D player;
    public override void _Ready() {
        player = GetNode<CharacterBody3D>("Player"); 
		Layer layer = new Layer(24,24,player);
        AddChild(layer);
	}
}
