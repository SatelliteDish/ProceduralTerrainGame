using System;
using Godot;

public partial class InputController : Node {
	[Export] float mouseSensitivity = 0.01f;

	private bool mouseLocked = false;
	public bool MouseLocked {
		set {
			mouseLocked = value;
			Input.MouseMode = value == true? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
		} get {
			return mouseLocked;
		}
	}
	public delegate void VecEventHandler(Vector2 lookVector);
	public delegate void JumpHandler();
	public event VecEventHandler PlayerLookEventHandler;
	public event VecEventHandler PlayerMoveEventHandler;
	public event JumpHandler  PlayerJumpEventHandler;
	Vector2 lookRotation = Vector2.Zero;
	public override void _Ready() {
		MouseLocked = true;
	}
	public override void _UnhandledInput(InputEvent ev) {
		switch (ev.GetClass()) {
			case "InputEventMouseMotion":
				handleMouseMotion((InputEventMouseMotion)ev);
				break;
			case "InputEventKey":
				handleKeyPress((InputEventKey)ev);	
				break;
		}
	}

	private void handleMouseMotion(InputEventMouseMotion ev) {
		lookRotation.X -= ev.Relative.X * mouseSensitivity;
		lookRotation.X = (float)Mathf.Wrap(lookRotation.X, 0.0, 360.0);
		lookRotation.Y -= ev.Relative.Y * mouseSensitivity;
		lookRotation.Y = (float)Math.Clamp(lookRotation.Y, -1.25, -.2);
		PlayerLookEventHandler(lookRotation);
	}
	
	private void handleKeyPress(InputEventKey ev) {
		if(ev.IsActionPressed("esc")) {
			MouseLocked = !MouseLocked;
		}
		if(ev.IsActionPressed("space")) {
			PlayerJumpEventHandler();
		}
	}

	public override void _PhysicsProcess(double delta) {
		Godot.Vector2 input_direction = Input.GetVector("a", "d", "w", "s");
		PlayerMoveEventHandler(input_direction);
	}	
}
