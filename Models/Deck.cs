using Blackjack.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    // Stelt een deck van 52 speelkaarten voor
    internal class Deck
    {
        public List<Card> cards { get; set; }

        public Deck()
        {
            cards = new List<Card>();
            // Vul het deck bij aanmaken
            GenerateDeck();
        }

        // Maakt alle 52 kaarten aan door elke rank met elke suit te combineren
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

        // Trekt de bovenste kaart uit het deck en verwijdert hem
        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        // Schudt het deck willekeurig
        public void shuffle()
        {
            Random random = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = random.Next(0, cards.Count);

                // Wissel kaart op positie i met een willekeurige kaart
                Card temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
    }
}