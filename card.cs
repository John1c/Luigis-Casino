using System;

namespace CardSystem
{
	public class Card
	{
		public int suit { get; set; } = 0; // 0 = Diamonds, 1 = Clubs, 2 = Hearts, 3 = Spades
		public int id { get; set; } = 0; // 1 = Ace, 1 = 2, 2 = 3, ... 9 = 10, 11 = Jack, 12 = Queen, 13 = King
		public int value { get; set; } = 0; //

		public Card(int suit, int id)
		{
			this.suit = suit;
			this.id = id;

			// Set value of card
			this.value = (id > 10) ? 10 : id; // Jack, Queen, King = 10, else same as id

			if (id == 1) // Ace
				this.value = 11;
		}
	}
}