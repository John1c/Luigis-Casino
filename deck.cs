using Godot;
using System;

public partial class deck : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shuffle();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	public void shuffle()
	{
		GD.Print("Shuffling deck...");
		
		// reset deck 
		cards.Clear();

		// create deck
		for (int suit = 0; suit < 4; suit++) // suits (diamonds, clubs, hearts, spades)
		{
			for (int id = 1; id < 14; id++) // id's (1-13) 
			{
				Card card = new Card();
				card.suit = suit;
				card.id = id;
				card.value = (id > 10) ? 10 : id; // Face card value = 10 
				cards.Add(card);
			}
		}
		
		// shuffle deck
		Random rand = new Random();
		for (int i = 0; i < cards.Count; i++)
		{
			int r = rand.Next(i, cards.Count);
			Card temp = cards[i];
			cards[i] = cards[r];
			cards[r] = temp;
		}
	}

	public Card drawRandomCard()
	{
		// if any cards left in deck
		if (cards.Count == 0)
			return null;

		Card card = cards[0];
		cards.RemoveAt(0);
		return card;
	}
}
