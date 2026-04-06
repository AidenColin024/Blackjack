using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    // Stelt de hand voor van een speler of dealer (verzameling kaarten)
    internal class Hand
    {
        public List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        // Voegt een kaart toe aan de hand
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        // Berekent de totale waarde van alle kaarten in de hand
        public int GetTotalValue()
        {
            int Total = 0;

            foreach (Card card in cards)
            {
                Total += card.GetValue();
            }
            return Total;
        }
    }
}