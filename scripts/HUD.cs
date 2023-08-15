using Godot;
using System;

public class HUD : Control
{

	private TextureRect lives;
	private Label scoreLabel;
	
	public override void _Ready()
	{
		lives = GetNode<TextureRect>("Lives");
		scoreLabel = GetNode<Label>("ScoreLabel");
	}

	public void ReduceLives() 
	{
		var size = lives.RectSize;
		lives.RectSize = new Vector2(size.x - 16, 16);
	}
	
	public void SetLives(int liveCount) 
	{
		var size = lives.RectSize;
		lives.RectSize = new Vector2(size.x * liveCount, 16);
	}
	
	public void UpdateScore(int score) 
	{
		scoreLabel.Text = "Score: " + score.ToString();
	}
}
