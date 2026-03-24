using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Models
{
    internal class Dealer
    {
        public Hand hand;

        public Dealer()
        {
            hand = new Hand();
        }

        public void AddCard(Card card)
        {
            hand.AddCard(card);
        }

        public void Play(Deck deck)
        {
            while (hand.GetTotalValue() < 17)
            {
                hand.AddCard(deck.DrawCard());
            }
        }
    }
}
