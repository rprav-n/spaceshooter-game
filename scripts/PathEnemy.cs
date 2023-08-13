using Godot;
using System;

public class PathEnemy : Path2D
{
	private PathFollow2D pathFollow;
	private Enemy enemy;

	public override void _Ready()
	{
		pathFollow = GetNode<PathFollow2D>("PathFollow2D");
		enemy = GetNode<Enemy>("PathFollow2D/AlanEnemy");
	}

	public override void _Process(float delta)
	{
		pathFollow.UnitOffset += delta * 0.1f;
		if (pathFollow.UnitOffset >= 1) 
		{
			QueueFree();
		}	
	}
}
