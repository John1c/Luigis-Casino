using System;
using System.Collections.Generic;
using Godot;

namespace CardSystem
{
	public class Deck
	{	
		List<Card> cards = new List<Card>();

		public Deck()
		{
			createDeck(); // create deck
			shuffleDeck(); // shuffle deck
		}
	
		public void createDeck()
		{
			// reset deck 
			cards.Clear();

			// create deck
			for (int suit = 0; suit < 4; suit++) // suits (diamonds, clubs, hearts, spades)
				for (int id = 1; id < 14; id++) // id's (1-13) 
					cards.Add(new Card(suit, id));
		}

		public void shuffleDeck()
		{
			// randomize deck
			GD.Print("Shuffling deck...");
			Random random = new Random();
			for (int i = 0; i < cards.Count; i++)
			{
				int randomIndex = random.Next(0, cards.Count);
				Card temp = cards[i];
				cards[i] = cards[randomIndex];
				cards[randomIndex] = temp;
			}
		}

	
		public Card drawRandomCard()
		{
			// check if deck is empty
			if (cards.Count == 0)
			{
				GD.Print("Deck is empty!");
				return null;
			}

			// draw random card
			Random random = new Random();
			int randomIndex = random.Next(0, cards.Count);
			Card card = cards[randomIndex];
			cards.RemoveAt(randomIndex);
			return card;
		}
	}
}
