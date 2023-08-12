using Godot;
using System;

public class Enemy : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
	}
	
	private void _on_Enemy_area_entered(Area2D area) 
	{
		if (area is Beam beam) 
		{
			beam.QueueFree();
			QueueFree();
		}
	}
}
