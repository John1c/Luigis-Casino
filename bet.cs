using Godot;
using System;

public partial class bet : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var betButton = new Button();
		betButton.Text = "bet";
		betButton.Pressed += ButtonPressed;
		AddChild(betButton);
	}

	private void ButtonPressed()
	{
		GD.Print("bet button pressed");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
