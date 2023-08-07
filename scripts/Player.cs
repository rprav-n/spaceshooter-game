using Godot;
using System;

public class Player : Area2D
{
	[Export]
	private int speed = 100;
	
	private Vector2 newPosition = Vector2.Zero;
	
	private AnimatedSprite playerAnimatedSprite;
	private AnimatedSprite boosterAnimatedSprite;
	private PackedScene PlayerBeam;
	private Node beamContainer;

	public override void _Ready()
	{
		playerAnimatedSprite = GetNode<AnimatedSprite>("PlayerAnimatedSprite");
		boosterAnimatedSprite = GetNode<AnimatedSprite>("BoosterAnimatedSprite");
		PlayerBeam = GD.Load<PackedScene>("res://scenes/PlayerBeam.tscn");
		beamContainer = GetNode<Node>("BeamContainer");
	}

	public override void _Process(float delta)
	{
		playerMovement(delta);
		shootBeam();
	}
	
	private void playerMovement(float delta) 
	{
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
		
		GlobalPosition += newPosition;
		
	}
	
	private void shootBeam() 
	{
		if (Input.IsActionJustPressed("shoot")) 
		{
			GD.Print("Player shoot");
			var playerBeam = PlayerBeam.Instance<Beam>();
			beamContainer.AddChild(playerBeam);
			var playerPosition = GlobalPosition;
			playerPosition.y -= 10;
			playerBeam.GlobalPosition = playerPosition;
		}
	}
}
