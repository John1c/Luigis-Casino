using Godot;
using System;

public partial class Coins : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		var coin = GetNode<Sprite2D>("Coin");
		
		
	}

	



	public void AddCoin()
	{
		var coin = GetNode<Sprite2D>("Coin");
		var coin2 = coin.Duplicate() as Sprite2D;
		AddChild(coin2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
