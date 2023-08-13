using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{	
	
	[Export]
	private List<PackedScene> enemies;
	
	private Node projectileContainer;
	private PackedScene ProjectileScene;
	private Node2D spawnPositionContainer;
	Random random = new Random();
	private List<Position2D> spawPositionArr = new List<Position2D>();
	
	public override void _Ready()
	{
		projectileContainer = GetNode<Node>("ProjectileContainer");
		ProjectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
		
		spawnPositionContainer = GetNode<Node2D>("SpawnPositionContainer");
		var arr = spawnPositionContainer.GetChildren();
		
		foreach (var item in arr)
		{
			spawPositionArr.Add(item as Position2D);
		}
		
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
	
	private void _on_SpawnTimer_timeout() 
	{
		spawnEnemy();
	}
	
	private void spawnEnemy() 
	{
		var randomPosIndex = random.Next(0, spawPositionArr.Count);
		var randomEnemyIndex = random.Next(0, enemies.Count);
		
		var randomEnemyScene = enemies[randomEnemyIndex];
		var randomEnemy = randomEnemyScene.Instance<Enemy>();
		var randomPosition = spawPositionArr[randomPosIndex];
		
		AddChild(randomEnemy);
		
		randomEnemy.GlobalPosition = randomPosition.GlobalPosition;
		randomEnemy.Connect("shootProjectile", this, "_on_Enemy_shootProjectile");
	}
}
