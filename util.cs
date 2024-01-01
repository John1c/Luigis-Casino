
// this file will be included into more scripts for utility functions such as a tilemap renderer for cards
// Godot game engine

using System;
using Godot;

public class utils {
    public static void GetCard(Card card) {

        // based on card.id, and card.suit we can get the card image

        // card.id is the number of the card (1-13)
        // card.suit is the suit of the card (1-4)

        // card.id = 1 is the ace
        // card.id = 11 is the jack
        // card.id = 12 is the queen
        // card.id = 13 is the king

        // card.suit = 1 is diamonds (♦)
        // card.suit = 2 is clubs (♣)
        // card.suit = 3 is hearts (♥)
        // card.suit = 4 is spades (♠)
    }
}