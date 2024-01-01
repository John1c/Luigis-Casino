using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSystem
{
	public class playerHand
	{
		public bool isDealer = false;
		public List<Card> cards = new List<Card>();
		public int handValue {
			get { return this.getHandValue(); }
		}

		public playerHand(bool isDealer = false)
		{
			this.isDealer = isDealer;
		}
	}

	public static class playerHandExtensions
	{
		public static int getHandValue(this playerHand hand)
		{
			int sum = 0;

			// count all cards values
			for (int i = 0; i < hand.cards.Count(); i++)
				sum += hand.cards[i].value;

			while (sum > 21)
			{
				// check if any cards are aces
				var unflippedAcesLeft = hand.cards.Where(x => x.id == 1 && x.value == 11).ToList();
				if (unflippedAcesLeft.Count() == 0)
					break;

				// flip an ace
				unflippedAcesLeft[0].value = 1;
				sum -= 10;
			}
 







			return sum;
		}



//		public void GAMELOGIC()
//		{
//			playerHand player = new playerHand();
//			playerHand dealer = new playerHand(true);
//
//			// check if user has blackjack
//			if (player.handValue == 21)
//			{
//				// player wins
//			}
//		}
	}
}

