using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    // Stelt de dealer voor in het Blackjack spel
    internal class Dealer
    {
        public Hand hand;

        public Dealer()
        {
            hand = new Hand();
        }

        // Voegt een kaart toe aan de hand van de dealer
        public void AddCard(Card card)
        {
            hand.AddCard(card);
        }

        // De dealer trekt kaarten totdat zijn totaal 17 of hoger is (standaard Blackjack regel)
        public void Play(Deck deck)
        {
            while (hand.GetTotalValue() < 17)
            {
                hand.AddCard(deck.DrawCard());
            }
        }
    }
}