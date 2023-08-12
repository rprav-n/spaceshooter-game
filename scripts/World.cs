using Godot;
using System;

public class World : Node2D
{	
	
	private Node projectileContainer;
	private PackedScene ProjectileScene;

	public override void _Ready()
	{
		projectileContainer = GetNode<Node>("ProjectileContainer");
		ProjectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
	}

	public override void _Process(float delta)
	{
		
	}
	
	private void _on_DeadZone_area_entered(Area2D area) 
	{
		if (area is Enemy enemy) 
		{
			enemy.QueueFree();
		} else if (area is Projectile projectile) 
		{
			projectile.QueueFree();
		}
	}
	
	private void _on_Enemy_shootProjectile(Enemy enemy, Vector2 location) 
	{
		var projectile = ProjectileScene.Instance<Projectile>();
		projectile.GlobalPosition = location;
		projectileContainer.AddChild(projectile);
		if (enemy.Name == "BonEnemy") 
		{
			projectile.SetSpriteFrame(2);
		}
	
	}
}
