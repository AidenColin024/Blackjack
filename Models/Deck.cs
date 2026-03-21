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

        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void shuffle()
        {
            Random random = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = random.Next(0, cards.Count);

                Card temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
    }
}
