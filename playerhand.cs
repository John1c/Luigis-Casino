using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CardSystem
{
	public class playerHand
	{
		public bool isDealer = false;
		public int Balance;
		public int betAmount = 0;
		public List<Card> cards = new List<Card>();
		public Dictionary<Card, Node2D> cardObjects = new Dictionary<Card, Node2D>();
		public int handValue {
			get { return this.getHandValue(); }
		}

		public playerHand(bool isDealer = false)
		{
			this.isDealer = isDealer;
		}

		public void clearCards()
		{
			foreach (var card in cardObjects)
				card.Value.QueueFree();

			cardObjects.Clear();
			cards.Clear();
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

		public static void CreateCardObjects(this playerHand player, Window parent)
		{
			for (int i = 0; i < player.cards.Count; i++)
			{
				// Check if card is already in cardObjects
				if (player.cardObjects.ContainsKey(player.cards[i]))
					continue; // Skip the card if it is already in cardObjects

				// instantiate card object
				GD.Print("Creating card object... #" + i + " (" + player.cards[i].suit + "," + player.cards[i].id + ") => " + player.cards[i].value);

				// load card scene
				var card = (PackedScene)ResourceLoader.Load("res://card_tile_set.tscn");
				Node2D cardInstance = card.Instantiate() as Node2D;

				// add card to scene
				parent.AddChild(cardInstance);
				player.cardObjects.Add(player.cards[i], cardInstance);
			}


			// update card image
			foreach (Node2D cardInstance in player.cardObjects.Values)
			{
				var mask = cardInstance.GetNode<Container>("CardMask");
				var cardSet = mask.GetNode<TileMap>("CardSets");
				Card card = player.cardObjects.FirstOrDefault(x => x.Value == cardInstance).Key;

				// set card sprite
				if (card.isHidden)
				{
					GD.Print("Card is hidden!");
					cardSet.SetCell(0, new Vector2I(0, 0), 0, new Vector2I(12, 4)); // red backside
				}
				else
					cardSet.SetCell(0, new Vector2I(0, 0), 0, new Vector2I(card.id, card.suit));
			}
		}
	}
}