using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    internal class Hand
    {
        public List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int GetTotalValue()
        {
            int Total = 0;
            
            foreach (Card card  in cards)
            {
                Total += card.GetValue();
            }
            return Total;
        }
    }
}
