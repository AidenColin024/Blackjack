using System;

namespace Blackjack.Models
{
    // Stelt een speler voor met een hand, inzet en bankroll
    internal class Speler
    {
        public Hand hand;

        // Huidige inzet van de speler voor deze ronde
        public decimal Inzet {  get; private set; }
        // Huidig saldo van de speler
        public decimal Bankroll {  get; private set; }

        public Speler()
        {
            hand = new Hand();
            Inzet = 0;
            Bankroll = 100;

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

        // Stelt de inzet in voor de huidige ronde
        public void PlaatsInzet (decimal bedrag)
        {
            Inzet = bedrag;
            Bankroll -= bedrag;
        }

        // Voegt uitbetaling toe aan de bankroll
        public void Uitbetaling (decimal bedrag)
        {
            Bankroll += bedrag;
        }
    }
}