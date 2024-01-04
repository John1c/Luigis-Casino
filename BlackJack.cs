using CardSystem;
using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class BlackJack : Control
{
	// Components
	[Export] public TextureButton HitButton;
	[Export] public TextureButton StandButton;
	[Export] public TextureButton BetButton;
	[Export] public TextureButton MenuButton;


	// Variables
	public playerHand player;
	public playerHand dealer;
	public Deck deck;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = new playerHand(false);
		dealer = new playerHand(true);
		deck = new Deck(); // this creates a new deck of cards and shuffles them

		// update player balance
		player.Balance = 100;

		// update renderer
		UpdateRenderer();
		
	}
	public void RoundStart()
	{
		// reset player and dealer hands
		player.cards.Clear();
		dealer.cards.Clear();
		//reset deck
		deck = new Deck();
		// shuffel deck
		deck.shuffleDeck();
		// draw 2 cards for player and dealer
		player.cards.Add(deck.drawRandomCard());
		player.cards.Add(deck.drawRandomCard());

		dealer.cards.Add(deck.drawRandomCard());
		dealer.cards.Add(deck.drawRandomCard(true)); // hide the second card

		//check if player has blackjack
		if(player.handValue == 21)
		{
			_on_StandButton_up();
		}
	}
	public void UpdateRenderer()
	{
		// instantiate card objects for player and dealer (card.tscn)
		// use card.getCardImagePosition() to get the position of the card image
		// get root scene
		var parent = GetTree().Root;
		GD.Print(parent);

		GD.Print("Player cards count: " + player.cards.Count);
		GD.Print("Dealer cards count: " + dealer.cards.Count);

		for (int i = 0; i < player.cards.Count; i++)
		{
			// instantiate card object
			GD.Print("Creating card object... #" + i + " (" + player.cards[i].suit + "," + player.cards[i].id + ")");
			
			// load card scene
			var card = (PackedScene)ResourceLoader.Load("res://card_tile_set.tscn");
			Node2D cardInstance = card.Instantiate() as Node2D;
			
			// move card to a position on the screen
			cardInstance.Position = new Vector2(100, 100);

			var mask = cardInstance.GetNode<Container>("CardMask");
			var cardSet = mask.GetNode<TileMap>("CardSets");
			// set cell to card image based on suit, id
			cardSet.SetCell(0, new Vector2I(player.cards[i].suit, player.cards[i].id));

			// add card to scene
			parent.AddChild(cardInstance);


			GD.Print("Card added to scene");
		}



		GD.Print("Player hand value: " + player.handValue);
	}



	public void _on_BetButton_up()
	{
		if (player.Balance >= 5)
		{
			player.betAmount += 5;
			player.Balance -= 5;
		}
		else
		{
			player.betAmount += player.Balance;
			player.Balance = 0;
		}

		// if balance is 0, disable bet button
		if (player.Balance == 0)
		{
			BetButton.Disabled = true;
			BetButton.Hide();
		}
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
		else if(player.handValue == 21)
		{
			_on_StandButton_up();
		}
		
		
	}


	public void _on_StandButton_up()
	{
		GD.Print("Stand");
		//end player turn
		//start dealer turn
		while(getWinState() == WinState.Continue)
		{
			dealer.cards.Add(deck.drawRandomCard());
		}
		// get win state
		
		switch(getWinState())
		{
			case WinState.Lost:
				GD.Print("You lost");
				//restart game
				// no points ):
				break;
			case WinState.Won:
				GD.Print("You won");
				//restart game
				// points to player
				break;
			case WinState.Push:
				GD.Print("Push");
				//restart game
				// money back
				break;
			case WinState.BlackJack:
				GD.Print("BlackJack");
				//restart game
				// money back + 1.5x
				break;
			case WinState.Unknown:
				GD.Print("Error: getWinState() returned unknown");
				break;
			case WinState.Continue:
				GD.Print("Error: getWinState() returned continue");
				break;
			default:
				GD.Print("Super Error: getWinState() returned default case... how tf did you manage to fo this");
				break;
		}

		
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
	
	public WinState getWinState()
	{
		if (player.handValue > 21)
			return WinState.Lost;
		else if(dealer.handValue > 21)
			return WinState.Won;
		else if(dealer.handValue == 21 && player.handValue == 21 && dealer.cards.Count == 2 && player.cards.Count == 2)
			return WinState.Push;
		else if(player.handValue == 21 && player.cards.Count == 2)
			return WinState.BlackJack; 
		else if(dealer.handValue >= player.handValue)
			return WinState.Lost;
		else if (dealer.handValue < player.handValue)
			return WinState.Won;
		else if (dealer.handValue < 18)
			return WinState.Continue;
		else
		{
			GD.Print("Error: getWinState() returned default case");
			return WinState.Unknown; // push on default case
		}
	}

	public enum WinState
	{
		Unknown = 0,
		Lost = 1,
		Won = 2,
		Push = 3,
		BlackJack = 4,
		Continue = 5,
	}
}
