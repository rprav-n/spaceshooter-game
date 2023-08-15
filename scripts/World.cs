using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{	
	
	[Export]
	private List<PackedScene> enemies;
	
	private PackedScene EnemyExplosion;
	private Node2D effects;
	private Timer spawnTimer;
	private Control gameOver;
	
	private Node projectileContainer;
	private PackedScene ProjectileScene;
	private Node2D spawnPositionContainer;
	Random random = new Random();
	private List<Position2D> spawPositionArr = new List<Position2D>();
	
	private HUD hud;
	private AudioStreamPlayer explosionSound;
	private Area2D player;
	
	private int lives = 3;
	private int score = 0;
	
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
		
		EnemyExplosion = GD.Load<PackedScene>("res://scenes/EnemyExplosion.tscn");
		effects = GetNode<Node2D>("Effects");
		
		hud = GetNode<HUD>("CanvasLayer/HUD");
		spawnTimer = GetNode<Timer>("SpawnTimer");
		gameOver = GetNode<Control>("CanvasLayer/GameOver");

		explosionSound = GetNode<AudioStreamPlayer>("ExplosionSound");
		player = GetNode<Area2D>("Player");
		
		setDefaults();	
	}
	
	private void setDefaults() 
	{
		lives = 3;
		score = 0;
		hud.SetLives(lives);
		hud.UpdateScore(score);
		gameOver.Visible = false;
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
		randomEnemy.Connect("enemyDied", this, "_on_Enemy_Died");
	}
	
	private void _on_Enemy_Died(Vector2 location, bool hitsPlayer) 
	{
		if (hitsPlayer) 
		{
			hud.ReduceLives();
		}
		spawnExplosion(location);
		updateScore();
	}
	
	private void spawnExplosion(Vector2 location) 
	{
		explosionSound.Play();
		var enemyExplosion = EnemyExplosion.Instance<EnemyExplosion>();
		effects.AddChild(enemyExplosion);
		enemyExplosion.GlobalPosition = location;
		
		enemyExplosion.PlayAnimation();
	}
	
	private void updateScore() 
	{
		score++;
		hud.UpdateScore(score);
	}
	
	private void updateLives() 
	{
		lives--;

		if (lives <= 0) 
		{
			spawnTimer.Stop();
			gameOver.Visible = true;
			player.QueueFree();

		}
		hud.ReduceLives();
		
	}
	
	private void _on_Player_enemyProjectileHitsPlayer(Vector2 location) 
	{
		updateLives();
		spawnExplosion(location);
		updateScore();
	}
	
}

