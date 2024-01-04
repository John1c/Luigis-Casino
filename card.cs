using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// unsure which contains the correct functions
using Godot.Collections;
using Godot.Bridge;
using Godot.NativeInterop;



namespace CardSystem
{
	
	public class Card
	{
		public int suit { get; set; } = 0; // 0 = Diamonds, 1 = Clubs, 2 = Hearts, 3 = Spades
		public int id { get; set; } = 0; // 1 = Ace, 2 = 2, 3 = 3, ... 9 = 10, 11 = Jack, 12 = Queen, 13 = King
		public int value { get; set; } = 0; //
		public bool isHidden { get; set; } = false;

		public Card(int suit, int id, bool isHidden = false)
		{
			this.suit = suit;
			this.id = id;
			this.isHidden = isHidden;

			// Set value of card
			this.value = (id > 10) ? 10 : id; // Jack, Queen, King = 10, else same as id

			if (id == 1) // Ace
				this.value = 11;
		}
	}

	public static class CardExtensions
	{
		public static Vector2 getCardImagePosition(this Card card)
		{
			if (card.isHidden)
				return new Vector2(-378, -416); // return the back of the card (hidden)

			return new Vector2(card.id - 1 * 75 - 3, card.suit * 104);
		}
	}
}

		