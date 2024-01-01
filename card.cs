using Godot;
using System;

public partial class card : Node2D
{
	public int id {
		get { return _id; }
		set {
			// ensure its a valid id (1-13)
			if (value < 1 || value > 13) {
				GD.Print("Invalid card id: " + value);
				return;
			}
		}
	}
	public int suit {
		get { return _suit; }
		set {
			// ensure its a valid suit (1-4)
			if (value < 1 || value > 4) {
				GD.Print("Invalid card suit: " + value);
				return;
			}
		}
	}
	public int value { get; set; } = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}
