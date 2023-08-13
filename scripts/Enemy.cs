using Godot;
using System;
using System.Runtime.InteropServices;

public class Enemy : Area2D
{
	[Export]
	private int speed = 100;

	[Signal]
	private delegate void shootProjectile(Vector2 location);
	
	[Signal]
	private delegate void enemyDied(Vector2 location);

	public override void _Ready()
	{

	}

	public override void _Process(float delta)
	{
		var newVelocity = Vector2.Zero;
		newVelocity.y = speed * delta;
		
		GlobalPosition += newVelocity;
	}
	
	private void _on_Enemy_area_entered(Area2D area) 
	{
		if (area is Beam beam) 
		{
			EmitSignal("enemyDied", GlobalPosition);
			
			beam.QueueFree();
			QueueFree();
		}
	}
	
	private void _on_ShootProjectileTimer_timeout() 
	{
		EmitSignal("shootProjectile", this, GlobalPosition);
	}
}
