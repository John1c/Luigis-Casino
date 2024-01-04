using CardSystem;
using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public partial class BlackJack : Control
{
	// Components
	[Export] public TextureButton HitButton;
	[Export] public TextureButton StandButton;
	[Export] public TextureButton BetButton;
	[Export] public TextureButton MenuButton;
	[Export] public Control WinScreen;
	[Export] public Control LoseScreen;
	[Export] public Label winStateLabel;
	[Export] public Label BalanceLabel;
	[Export] public TextureButton RoundStartButton;
	[Export] public TextureRect pooltexture;

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
		setBalance(30);
		// update renderer
		player.betAmount = 0;
		// hide win/lose screen
		WinScreen.Hide();
		LoseScreen.Hide();
		winStateLabel.Text = "";
		// hide round start button
		RoundStartButton.Hide();
		RoundStart();
	}

	public TextureButton _on_RoundStartButton_up()
	{	
		RoundStartButton.Hide();
		player.betAmount = 0;
		GD.Print("Round Start");
		RoundStart();	
		pooltexture.Show();
		return null;
	}

	public void RoundStart()
	{
		// reset player and dealer hands
		player.clearCards();
		dealer.clearCards();
		deck = new Deck();
		
		// no need to null check, deck is always full at this point
		player.cards.Add(deck.drawRandomCard());
		player.cards.Add(deck.drawRandomCard());

		dealer.cards.Add(deck.drawRandomCard());
		dealer.cards.Add(deck.drawRandomCard(true)); // hide the second card

		GD.Print("Updating renderer...");
		UpdateRenderer();

		//check if player has blackjack
		if(player.handValue == 21)
		{
			_on_StandButton_up();
		}
		
		winStateLabel.Text = "";
		
		if(BetButton.Disabled == true)
		{
			BetButton.Disabled = false;
			BetButton.Show();
		}
	}

	public void UpdateRenderer()
	{
		updateBalance(); // update balance label

		var gameWindow = GetTree().Root;

		// create card objects for player (skips if already created)
		player.CreateCardObjects(gameWindow);
		dealer.CreateCardObjects(gameWindow);

		// player cards
		Vector2 cardSize = new Vector2(69, 104);
		int margin = 10;
		int yOffset = 50;

		float playerCardsWidth = player.cardObjects.Count * (cardSize.X + margin) - margin;
		float playerCardsX = (gameWindow.Size.X - playerCardsWidth) / 2;
		float playerCardsY = gameWindow.Size.Y / 4 * 3 - cardSize.Y / 2 + yOffset;

		for (int i = 0; i < player.cardObjects.Count; i++)
		{
			player.cardObjects[player.cards[i]].Position = new Vector2(playerCardsX + i * (cardSize.X + margin), playerCardsY);
		}

		// dealer cards
		float dealerCardsWidth = dealer.cardObjects.Count * (cardSize.X + margin) - margin;
		float dealerCardsX = (gameWindow.Size.X - dealerCardsWidth) / 2;
		float dealerCardsY = gameWindow.Size.Y / 4 - cardSize.Y / 2 + yOffset;

		for (int i = 0; i < dealer.cardObjects.Count; i++)
		{
			dealer.cardObjects[dealer.cards[i]].Position = new Vector2(dealerCardsX + i * (cardSize.X + margin), dealerCardsY);
		}

		// print hand value of both player and dealer (print dealer first like 6+4 = 10)
		GD.Print("Dealer hand: ");
		for (int i = 0; i < dealer.cards.Count; i++)
		{
			GD.Print("+" + dealer.cards[i].value);
		}
		GD.Print("Dealer hand value: " + dealer.handValue);

		// print player hand value
		GD.Print("Player hand: ");
		for (int i = 0; i < player.cards.Count; i++)
		{
			GD.Print("+" + player.cards[i].value);
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
		UpdateRenderer();
		GD.Print(player.betAmount);
	}

	public void _on_HitButton_up()
	{
		GD.Print("Hit");
		Card newcard = deck.drawRandomCard();

		if (newcard != null)
			player.cards.Add(newcard);
		else
			GD.Print("No more cards in deck");
		

		//print hand 
		GD.Print("Player hand: ");
		for (int i = 0; i < player.cards.Count; i++)
		{
			GD.Print(+ player.cards[i].value);	
		}
		GD.Print("Player hand value: " + player.handValue);
		//give another card to player
		
		//check if player is bust
		if(player.handValue > 21)
		{
			GD.Print("Bust");
			_on_StandButton_up();
		}
		else if(player.handValue == 21)
		{
			_on_StandButton_up();
		}
		
		UpdateRenderer();
	}


	public void _on_StandButton_up()
	{
		GD.Print("Stand");
		//reveal dealer card
		dealer.cards[1].isHidden = false;
		UpdateRenderer();
		while(getWinState() == WinState.Continue)
		{
			dealer.cards.Add(deck.drawRandomCard());
			UpdateRenderer();
		}
		// get win state
		GD.Print("dealer hand value:" + dealer.handValue);
		switch(getWinState())
		{
			case WinState.Lost:
				GD.Print("You lost");
				UpdateRenderer();
				winStateLabel.Text = "You lost";
				if(player.Balance <= 0)
				{
					LoseScreen.Show();
				}
				
				//restart game
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.Won:
				GD.Print("You won");
				
				winStateLabel.Text = "You won";
				//points to player
				player.Balance += player.betAmount * 2;
				UpdateRenderer();
				//restart game
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.Push:
				
				GD.Print("Push");
				
				winStateLabel.Text = "Push";
				// money back
				player.Balance += player.betAmount;
				UpdateRenderer();
				//restart game
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.BlackJack:
				GD.Print("BlackJack");
				
				winStateLabel.Text = "Blackjack";
				// money back + 2.0x
				player.Balance += player.betAmount * 3;
				UpdateRenderer();
				//restart game
				RoundStartButton.Show();
				pooltexture.Hide();

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
		else if(dealer.handValue == player.handValue)
			return WinState.Push;
		else if(player.handValue == 21 && player.cards.Count == 2)
			return WinState.BlackJack; 
		else if(dealer.handValue > player.handValue)
			return WinState.Lost;
		else if (dealer.handValue < player.handValue && dealer.handValue >= 17)
			return WinState.Won;
		else if (dealer.handValue < 17)
			return WinState.Continue;
		else
		{
			GD.Print("Error: getWinState() returned default case");
			return WinState.Unknown; // push on default case
		}
	}

	public void updateBalance()
	{
		Label balanceLable = GetNode<Label>("BalanceLabel");
		balanceLable.Text = player.Balance.ToString();
		Label MoneyPool = GetNode<Label>("MarginContainer/HBoxContainer/VBoxContainer/pool/MoneyPool");
		MoneyPool.Text = player.betAmount.ToString();
	}

	public void setBalance(int amount)
	{
		GD.Print("Setting balance to " + amount);
		player.Balance = amount;
		UpdateRenderer();
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
