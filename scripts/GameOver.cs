using Godot;
using System;

public class GameOver : Control
{

    public void _on_RetryButton_pressed() 
    {
    	GetTree().ReloadCurrentScene();
    }
}
