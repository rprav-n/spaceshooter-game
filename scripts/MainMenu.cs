using Godot;
using System;

public class MainMenu : Control
{

	private Button startButton;
	
	public override void _Ready()
	{
		startButton = GetNode<Button>("StartButton");
	}

	public void _on_StartButton_pressed() 
	{
		GetTree().ChangeScene("res://scenes/World.tscn");
	}
}
