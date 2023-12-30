using Godot;
using System;

public partial class card(int cardnumber, int cardsuit) : Node2D
{
	this.number = cardnumber;
	this.suit = cardsuit;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
