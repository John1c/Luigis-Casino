using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSystem
{
	
	public class Card
	{
		public int suit { get; set; } = 0; // 0 = Diamonds, 1 = Clubs, 2 = Hearts, 3 = Spades
		public int id { get; set; } = 0; // 0 = Ace, 1 = 2, 2 = 3, ... , 9 = 10, 10 = Jack, 11 = Queen, 12 = King
		public int value { get; set; } = 0; //
		public bool isHidden { get; set; } = false;

		public Card(int suit, int id, bool isHidden = false)
		{
			this.suit = suit;
			this.id = id;
			this.isHidden = isHidden;

			// Set value of card
			this.value = (id > 9) ? 10 : id + 1; // 2-10

			if (id == 0) // Ace
				this.value = 11;
		}
	}
}