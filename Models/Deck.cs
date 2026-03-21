using System;
using System.Collections.Generic;
using System.Text;
using Blackjack;

namespace Blackjack.Models
{
    internal class Deck
    {
        public List<Card> cards {  get; set; }

        public Deck()
        {
            cards = new List<Card>();
            GenerateDeck();
        }

        private void GenerateDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(rank, suit));
                }
            }
        }
    }
}
