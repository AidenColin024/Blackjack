using System;

namespace Blackjack.Models
{
    // Stelt een speler voor met een hand, inzet en bankroll
    internal class Speler
    {
        public Hand hand;


        public string Naam { get; }

        // Huidige inzet van de speler voor deze ronde
        public decimal Inzet {  get; private set; }
        // Huidig saldo van de speler
        public decimal Bankroll {  get; private set; }

        public Speler(string naam)
        {
            Naam = naam;
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

        // Tweede hand na een split
        public Hand gesplitsteHand;

        // Geeft aan of de speler gesplitst heeft
        public bool HeeftGesplitst { get; private set; }

        // Splitst de hand in twee handen
        public void Split()
        {
            gesplitsteHand = new Hand();

            // Verplaats de tweede kaart naar de gesplitste hand
            gesplitsteHand.AddCard(hand.cards[1]);
            hand.cards.RemoveAt(1);

            HeeftGesplitst = true;
        }

        // Reset de split voor een nieuwe ronde
        public void ResetSplit()
        {
            gesplitsteHand = null;
            HeeftGesplitst = false;
        }
    }
}