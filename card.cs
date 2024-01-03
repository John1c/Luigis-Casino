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

		/*
		// function to get the card image from the atlas based on the card's suit and value
		// tilemap source name is TileCards
		public Sprite getCardImage(Card card)
		{
			// suits (spades, hearts, diamonds, clubs)
			// id's (1-13)
			// 1 = Ace, 11 = Jack, 12 = Queen, 13 = King


			// get the tilemap atlas
			var atlas = Resources.Load("TileCards") as Texture2D;

			// get the tilemap atlas size
			var atlasWidth = atlas.GetWidth();
			var atlasHeight = atlas.GetHeight();

			// get the tilemap atlas tile size
			var tileWidth = atlasWidth / 13;
			var tileHeight = atlasHeight / 4;

			// suit 5, id 0 = blank card
			// suit 5, id 11 = black back of card
			// suit 5, id 12 = blue back of card
			// suit 5, id 13 = red back of card

			// get the tilemap atlas tile position
			var tileX = (card.id - 1) * tileWidth;
			var tileY = card.suit * tileHeight;

			// get the tilemap atlas tile
			//var tile = atlas.GetPixels(tileX, tileY, tileWidth, tileHeight);

			// create a new texture
			//var texture = new Texture2D();
			//texture.LoadImage(tile);

			// create a new sprite
			// var sprite = new Sprite();
			// sprite.Texture = texture;

			// return the sprite
			// return sprite;
			return null;
		}
		*/
	}
}

		