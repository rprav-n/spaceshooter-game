using Godot;
using System;
using System.Drawing.Printing;

public class Player : Area2D
{
	[Export]
	private int speed = 100;
	
	[Signal]
	private delegate void enemyDestroyed(Vector2 location, bool hitsPlayer = false);
	
	[Signal]
	private delegate void enemyProjectileHitsPlayer(Vector2 location);
	
	private Vector2 newPosition = Vector2.Zero;
	
	private AnimatedSprite playerAnimatedSprite;
	private AnimatedSprite boosterAnimatedSprite;
	private PackedScene PlayerBeam;
	private Node beamContainer;
	private AudioStreamPlayer laserSound;
	
	public override void _Ready()
	{
		playerAnimatedSprite = GetNode<AnimatedSprite>("PlayerAnimatedSprite");
		boosterAnimatedSprite = GetNode<AnimatedSprite>("BoosterAnimatedSprite");
		PlayerBeam = GD.Load<PackedScene>("res://scenes/PlayerBeam.tscn");
		beamContainer = GetNode<Node>("BeamContainer");
		laserSound = GetNode<AudioStreamPlayer>("LaserSound");
	}

	public override void _Process(float delta)
	{
		playerMovement(delta);
		shootBeam();
	}
	
	private void playerMovement(float delta) 
	{
		var screenSize = GetViewportRect().Size;
		float xDirection = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		float yDirection = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
		
		if (xDirection == 1) 
		{
			playerAnimatedSprite.Play("move_right");
			boosterAnimatedSprite.Scale = new Vector2(0.5f, boosterAnimatedSprite.Scale.y);
			boosterAnimatedSprite.Play("move_right");
			
		} else if (xDirection == -1) 
		{
			playerAnimatedSprite.Play("move_left");
			boosterAnimatedSprite.Scale = new Vector2(0.5f, boosterAnimatedSprite.Scale.y);
			boosterAnimatedSprite.Play("move_left");
		} else 
		{
			playerAnimatedSprite.Play("idle");
			boosterAnimatedSprite.Scale = new Vector2(1f, boosterAnimatedSprite.Scale.y);
			boosterAnimatedSprite.Play("idle");
		}
		
		newPosition = new Vector2(xDirection * speed * delta, yDirection * speed * delta).Normalized();
		
		GD.Print("newPosition", newPosition);
		
		GlobalPosition += newPosition;
		
		var newGlobalPosition = GlobalPosition;
		
		newGlobalPosition.x =  Mathf.Clamp(newGlobalPosition.x, 0, screenSize.x);
		newGlobalPosition.y =  Mathf.Clamp(newGlobalPosition.y, 0, screenSize.y);
		
		GlobalPosition = newGlobalPosition;
		

		
	}
	
	private void shootBeam() 
	{
		if (Input.IsActionJustPressed("shoot")) 
		{
			var playerBeam = PlayerBeam.Instance<Beam>();
			beamContainer.AddChild(playerBeam);
			var playerPosition = GlobalPosition;
			playerPosition.y -= 10;
			playerBeam.GlobalPosition = playerPosition;
			laserSound.Play();
		}
	}
	
	private void _on_Player_area_entered(Area2D area) 
	{
		if (area is Enemy enemy) 
		{
			EmitSignal("enemyDestroyed", enemy.GlobalPosition, true);
			enemy.QueueFree();
		}
		if (area is Projectile projectile) 
		{
			EmitSignal("enemyProjectileHitsPlayer", projectile.GlobalPosition);
			projectile.QueueFree();
		}
	}
}
