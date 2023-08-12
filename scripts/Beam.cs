using Godot;
using System;

public class Beam : Area2D
{
	[Export]
	public int direction = -1;

	[Export]
	public int speed = 100;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{
		this.GlobalPosition += new Vector2(0, speed * direction * delta);
	}
	
	private void _on_BeamVisibilityNotifier2D_screen_exited() 
	{
		QueueFree();
	}
}
