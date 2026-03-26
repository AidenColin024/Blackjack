using System;
using System.Collections.Generic;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Blackjack.Models
{
    internal class Speler
    {
        public Hand hand;

        public Speler()
        {
            Hand hand = new Hand();
        }

        public void AddCard(Card card)
        {
            hand.AddCard(card);
        }

        public int GetTotalValue()
        {
            return hand.GetTotalValue();
        }
    }
}
