using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class Container : Godot.Container
{
	// Called when the node enters the scene tree for the first time.
	
	public TileMap cardMask;
	
	[Export] public Button moveBtnUp;
	[Export] public Button moveBtnDown;
	[Export] public Button moveBtnLeft;
	[Export] public Button moveBtnRight;


	public override void _Ready()
	{
	

		//get the position of the tilemap "CardMask/CardSets" 
		cardMask = GetNode<TileMap>("CardSets");
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		GD.Print("Ready");
	}



	public void _on_btnMoveTileMapUp()
	{
		//move the tilemap up
		cardMask.Position = new Vector2(cardMask.Position.X, cardMask.Position.Y + 104);
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		GD.Print("Up");
	}

	public void _on_btnMoveTileMapDown()
	{
		//move the tilemap down
		cardMask.Position = new Vector2(cardMask.Position.X, cardMask.Position.Y - 104);
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		GD.Print("Down");
	}

	public void _on_btnMoveTileMapLeft()
	{
		//move the tilemap to the left
		cardMask.Position = new Vector2(cardMask.Position.X + 75, cardMask.Position.Y);
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		GD.Print("Left");
	}

	public void _on_btnMoveTileMapRight()
	{
		//move the tilemap to the left
		cardMask.Position = new Vector2(cardMask.Position.X - 75, cardMask.Position.Y);
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		GD.Print("Right");
	}


	public void cardSelect()
	{
		//get the position of the tilemap "CardMask/CardSets" 
		
		GD.Print(cardMask.Position.X + " " + cardMask.Position.Y);
		

		
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
