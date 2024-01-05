using CardSystem;
using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

	public bool isRoundStarted = false;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// if player is null, clear cards objects
		if (player != null) player.clearCards();
		if (dealer != null) dealer.clearCards();

		player = new playerHand(false);
		dealer = new playerHand(true);
		deck = new Deck(); // this creates a new deck of cards and shuffles them
		isRoundStarted = false;

		// update player balance
		setBalance(30);

		// set bet
		BetButton.Show();
		player.betAmount = 0;

		// hide win/lose screen
		WinScreen.Hide();
		LoseScreen.Hide();
		winStateLabel.Text = "";

		// hide round start, stand and hit button
		RoundStartButton.Hide();
		StandButton.Hide();
		HitButton.Hide();
	}

	public void _on_RoundStartButton_up()
	{
		// hide win/lose screen
		WinScreen.Hide();
		LoseScreen.Hide();
		winStateLabel.Text = "";

		// hide round start, stand and hit button
		RoundStartButton.Hide();
		StandButton.Hide();
		HitButton.Hide();

		// show bet button
		BetButton.Show();

		// reset bet amount
		player.betAmount = 0;

		// reset round started
		isRoundStarted = false;

		// show total bet pool
		pooltexture.Show();

		// reset dealer and player cards
		player.clearCards();
		dealer.clearCards();
	}

	public void RoundStart()
	{
		GD.Print("Starting game with " + player.betAmount + " bet");
		BetButton.Hide();
		RoundStartButton.Hide();
		HitButton.Show();
		StandButton.Show();
		winStateLabel.Text = "";

		// reset player and dealer hands
		player.clearCards();
		dealer.clearCards();
		deck = new Deck();
		
		// no need to null check, deck is always full at this point
		player.cards.Add(deck.drawRandomCard());
		player.cards.Add(deck.drawRandomCard());

		dealer.cards.Add(deck.drawRandomCard());
		dealer.cards.Add(deck.drawRandomCard(true)); // hide the second card

		UpdateRenderer();

		//check if player has blackjack right away
		if (player.handValue == 21)
			_on_StandButton_up();
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

		// render player cards
		float playerCardsWidth = player.cardObjects.Count * (cardSize.X + margin) - margin;
		float playerCardsX = (gameWindow.Size.X - playerCardsWidth) / 2;
		float playerCardsY = gameWindow.Size.Y / 4 * 3 - cardSize.Y / 2 + yOffset;
		for (int i = 0; i < player.cardObjects.Count; i++)
			player.cardObjects[player.cards[i]].Position = new Vector2(playerCardsX + i * (cardSize.X + margin), playerCardsY);

		// render dealer cards
		float dealerCardsWidth = dealer.cardObjects.Count * (cardSize.X + margin) - margin;
		float dealerCardsX = (gameWindow.Size.X - dealerCardsWidth) / 2;
		float dealerCardsY = gameWindow.Size.Y / 4 - cardSize.Y / 2 + yOffset;
		for (int i = 0; i < dealer.cardObjects.Count; i++)
			dealer.cardObjects[dealer.cards[i]].Position = new Vector2(dealerCardsX + i * (cardSize.X + margin), dealerCardsY);

		// print hand value of both player and dealer (print dealer first like 6+4 = 10)
		GD.Print("Dealer hand: ");
		for (int i = 0; i < dealer.cards.Count; i++)
			GD.Print("+" + dealer.cards[i].value);
		GD.Print("=" + dealer.handValue);

		// print player hand value
		GD.Print("Player hand: ");
		for (int i = 0; i < player.cards.Count; i++)
			GD.Print("+" + player.cards[i].value);
		GD.Print("=" + player.handValue);
	}



	public void _on_BetButton_up()
	{
		// if first bet of round, show hit button
		if (!isRoundStarted && player.betAmount == 0)
			HitButton.Show();

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
			BetButton.Hide();

		updateBalance();
		GD.Print("Bet amount upped to: " + player.betAmount);
	}

	public void _on_HitButton_up()
	{
		// if round has not started, start round
		if (!isRoundStarted)
		{
			RoundStart();
			isRoundStarted = true;
			return;
		}


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
		dealer.cards[1].isHidden = false; //reveal the hidden dealer card

		// hide stand & hit button
		StandButton.Hide();
		HitButton.Hide();

		while(getGameState() == WinState.Continue)
		{
			dealer.cards.Add(deck.drawRandomCard());
		}

		switch(getGameState())
		{
			case WinState.Lost:
				GD.Print("You lost");
				winStateLabel.Text = "You lost";
				if (player.Balance <= 0) // if player has no money left, show lose screen
					LoseScreen.Show();
				else { // else show round start button
					RoundStartButton.Show();
					pooltexture.Hide();
				}
				break;
			case WinState.Won:
				GD.Print("You won");
				winStateLabel.Text = "You won";
				player.Balance += player.betAmount * 2;
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.Push:
				GD.Print("Push");
				winStateLabel.Text = "Push";
				player.Balance += player.betAmount; // money back
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.BlackJack:
				GD.Print("BlackJack");
				winStateLabel.Text = "Blackjack";
				player.Balance += player.betAmount * 3; // win 3x bet
				RoundStartButton.Show();
				pooltexture.Hide();
				break;
			case WinState.Unknown: GD.Print("Error: getWinState() returned unknown"); break;
			case WinState.Continue: GD.Print("Error: getWinState() returned continue"); break;
			default: GD.Print("Super Error: getWinState() returned default case..."); break;
		}
		UpdateRenderer(); // update renderer to show dealer cards
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
	
	public WinState getGameState()
	{
		if (player.handValue > 21)
			return WinState.Lost;
		else if(dealer.handValue > 21)
			return WinState.Won;
		else if(player.handValue == 21 && player.cards.Count == 2)
			return WinState.BlackJack; 
		else if(dealer.handValue > player.handValue)
			return WinState.Lost;
		else if (dealer.handValue < player.handValue && dealer.handValue >= 17)
			return WinState.Won;
		else if (dealer.handValue < 17)
			return WinState.Continue;
		else if(dealer.handValue == player.handValue)
			return WinState.Push;
		else
		{
			GD.Print("Error: getWinState() returned default case");
			return WinState.Push; // push on default case
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