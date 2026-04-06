using System;

namespace Blackjack.Models
{
    // Stelt een speler voor met een hand
    internal class Speler
    {
        public Hand hand;

        // Huidige inzet van de speler voor deze ronde
        public decimal Inzet {  get; private set; }

        public Speler()
        {
            hand = new Hand();
            Inzet = 0;

        }

        // Voegt een kaart toe aan de hand van de speler
        public void AddCard(Card card)
        {
            hand.AddCard(card);
        }

        // Geeft de totale waarde van de hand terug
        public int GetTotalValue()
        {
            return hand.GetTotalValue();
        }

        public void PlaatsInzet (decimal bedrag)
        {
            Inzet = bedrag;
        }
    }
}