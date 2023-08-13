using Godot;
using System;

public class EnemyExplosion : Node2D
{

	private AnimatedSprite animatedSprite;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}
	
	private void _on_AnimatedSprite_animation_finished() 
	{
		QueueFree();
	}
	
	public void PlayAnimation() 
	{
		animatedSprite.Frame = 0;
		animatedSprite.Play();
	}
}
