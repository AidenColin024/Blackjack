using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten = 10,
        Jack = 10,
        Queen = 10,
        King = 10,
        Ace = 11
    }
    internal class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card( Rank rank, Suit suit)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
