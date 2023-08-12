using Godot;
using System;

public class Projectile : Area2D
{
	[Export]
	private int speed = 100;

	private Sprite sprite;

	public override void _Ready()
	{
		sprite = GetNode<Sprite>("Sprite");
		GD.Print("sprite ", sprite);
	}

	public override void _Process(float delta)
	{
		var newVelocity = Vector2.Zero;
		newVelocity.y = speed * delta;
		
		GlobalPosition += newVelocity;
	}
	
	public void SetSpriteFrame(int frame) 
	{
		sprite.Frame = frame;
	}
}
