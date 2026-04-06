using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    // Enum voor de vier kleuren van een speelkaart
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    // Enum voor de waarden van een speelkaart (Two t/m Ace)
    public enum Rank
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    // Stelt één speelkaart voor met een kleur en waarde
    internal class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(Rank rank, Suit suit)
        {
            Suit = suit;
            Rank = rank;
        }

        // Geeft de Blackjack-waarde terug van de kaart
        // Plaatjes en tienen = 10, Aas = 11, overige kaarten = hun eigen waarde
        public int GetValue()
        {
            if (Rank == Rank.Jack || Rank == Rank.Queen || Rank == Rank.King || Rank == Rank.Ten)
            {
                return 10;
            }
            else if (Rank == Rank.Ace)
            {
                return 11;
            }
            else
            {
                return (int)Rank;
            }
        }

        // Geeft een leesbare weergave van de kaart, bijv. "King of Hearts"
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}