using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSystem
{
	public class playerHand
	{
		public bool isDealer = false;
		public int Balance;
		public int betAmount = 0;
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
			
			for (int i = 0; i < hand.cards.Count(); i++)
				sum += hand.cards[i].value;

			bool done = false;

			while (sum > 21 && !done)
			{
				done = true;
				for (int i = 0; i < hand.cards.Count(); i++)
				{
					if(hand.cards[i].value == 11 && sum > 21 && done)
					{
						hand.cards[i].value = 1;
						sum -= 10;
						done = false;
					}
				}
			}

			for (int i = 0; i < hand.cards.Count(); i++)
				if(hand.cards[i].value == 1)
					hand.cards[i].value = 11;
			return sum;
		}

		
	}



}


