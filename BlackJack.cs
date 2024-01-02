using CardSystem;
using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class BlackJack : Control
{
	playerHand player;
	playerHand dealer;
	Deck deck;
	[Export] public TextureButton HitButton;

	[Export] public TextureButton StandButton;

	[Export] public Button MenuButton;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		player = new playerHand(false);
		dealer = new playerHand(true);
		deck = new Deck();
		deck.createDeck();
		deck.shuffleDeck();

	}



	public void _on_HitButton_up()
	{
		GD.Print("Hit");

		//give another card to player
		player.cards.Add(deck.drawRandomCard());
		//check if player is bust
		if(player.handValue > 21)
		{
			GD.Print("Bust");
			//end game
		}
		//if bust, end game
	}


	public void _on_StandButton_up()
	{
		GD.Print("Stand");
		//end player turn
		//start dealer turn
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
	
	public static string getWinState(playerHand player, playerHand dealer)
	{
		if	   (player.handValue > 21)
			return "lost";
		else if(dealer.handValue > 21)
			return "won";
		else if(dealer.handValue == 21 && player.handValue == 21 && dealer.cards.Count == 2 && player.cards.Count == 2)
			return "tie";
		else if(dealer.handValue >= player.handValue)
			return "lost";
		else if (dealer.handValue < player.handValue)
			return "won";
		else
			return "error could not determine win state";
	}
}
