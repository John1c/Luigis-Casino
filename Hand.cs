using Godot;
using System;

public partial class Hand : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	//	ResourceLoader.Load("res://cardTilemap.jpg") as PackedScene;
		public PackedScene cardGen = GD.Load<PackedScene>("res://.godot/imported/cardTilemap.jpg-9df5f554a927ed0916ce16cfe0d4e4a1.ctex");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
//	public override void _Process(double delta)
//	{
//	}
}
