using Godot;
using System;

public partial class restart : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var restartButton = new Button();
		restartButton.Text = "restart";
		restartButton.Pressed += ButtonPressed;
		AddChild(restartButton);
	}

	private void ButtonPressed()
	{
		GD.Print("restart");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
