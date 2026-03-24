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
                if(card.Rank == Rank.Jack || card.Rank == Rank.Queen || card.Rank == Rank.King)
                {
                    Total += 10;
                }
                else if(card.Rank == Rank.Ace)
                {
                    Total += 11;
                }
                else
                {
                    Total += (int)card.Rank + 2;
                }
            }
            return Total;
        }
    }
}
