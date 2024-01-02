using Godot;
using System;

public partial class MainMenu : Control
{


	[Export] public Button StartGame;
	[Export] public Button QuitGame;


	private void _on_StartGame_button_up()
	{
		GD.Print("Start Game");
		var nextScene = (PackedScene)ResourceLoader.Load("res://BlackJack.tscn");
		GetTree().ChangeSceneToPacked(nextScene);
	}

	private void _on_QuitGame_button_up()
	{
		GetTree().Quit();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
