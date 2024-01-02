using Godot;
using System;

public partial class BlackJack : Control
{

	[Export] public TextureButton HitButton;

	[Export] public TextureButton StandButton;

	[Export] public Button MenuButton;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
				
	}



	public void _on_HitButton_up()
	{
		GD.Print("Hit");
	}


	public void _on_StandButton_up()
	{
		GD.Print("Stand");
	}
	public void _on_MenuButton_up()
	{
		GD.Print("Menu");
		var nextScene = (PackedScene)ResourceLoader.Load("res://main_menu.tscn");
		GetTree().ChangeSceneToPacked(nextScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
