using Godot;
using System;

public partial class hit : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
//		var hitButton = new Button();
//		hitButton.Text = "Hit";
//		hitButton.Pressed += ButtonPressed;
//		AddChild(hitButton);
	}




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}


private void _on_button_up()
{
	GD.Print("Hit");
}
