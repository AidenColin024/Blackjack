using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Blackjack.Models
{
    internal class BepaalUitslagAlleSpelers
    {
        // Bepaalt de uitslag voor alle spelers en verwerkt de uitbetalingen
        // Regels: bust verliest, Blackjack betaalt 3:2, gewone winst 1:1, gelijkspel = push
        public void bepaaluitslagallespelers(List<Speler> spelers, Dealer dealer, int huidigeSpelerIndex, int dealerScore, int dealStap)
        {
            int dealerTotaal = dealer.hand.GetTotalValue();
            string overzicht = "Dealer totaal: " + dealerTotaal + "\n\n";

            foreach (Speler speler in spelers)
            {
                int spelerTotaal = speler.hand.GetTotalValue();

                // Blackjack = 21 punten met exact 2 kaarten
                bool spelerBlackjack = spelerTotaal == 21 && speler.hand.cards.Count == 2;

                overzicht += speler.Naam + ": " + spelerTotaal + " — ";

                if (spelerTotaal > 21)
                {
                    // Bust: speler verliest, geen uitbetaling
                    overzicht += "Bust! Verliest €" + speler.Inzet + "\n";
                }
                else if (spelerBlackjack)
                {
                    // Blackjack: inzet terug + 3:2 winst
                    double winst = speler.Inzet * 1.5;
                    speler.Uitbetaling(speler.Inzet + winst);
                    overzicht += "Blackjack! Wint €" + winst + " (3:2). Bankroll: €" + speler.Bankroll + "\n";
                }
                else if (dealerTotaal > 21 || spelerTotaal > dealerTotaal)
                {
                    // Winst: inzet terug + 1:1 winst
                    speler.Uitbetaling(speler.Inzet * 2);
                    overzicht += "Wint €" + speler.Inzet + " (1:1). Bankroll: €" + speler.Bankroll + "\n";
                }
                else if (spelerTotaal == dealerTotaal)
                {
                    // Push: inzet terug, geen winst of verlies
                    speler.Uitbetaling(speler.Inzet);
                    overzicht += "Gelijkspel! Inzet terug. Bankroll: €" + speler.Bankroll + "\n";
                }
                else
                {
                    // Verlies: dealer wint
                    overzicht += "Verloren! Bankroll: €" + speler.Bankroll + "\n";
                }

                // Toon eindscore van de dealer
                MessageBox.Show("Ronde afgelopen!\nDealer score deze sessie: " + dealerScore + " punten.");

                // Reset voor nieuwe ronde
                huidigeSpelerIndex = 0;
                dealStap = 0;

                // Controleer of de speler ook een gesplitste hand heeft
                if (speler.HeeftGesplitst)
                {
                    int gesplitsttotaal = speler.gesplitsteHand.GetTotalValue();
                    overzicht += speler.Naam + " (hand 2): " + gesplitsttotaal + " — ";

                    if (gesplitsttotaal > 21)
                    {
                        overzicht += "Bust! Verliest €" + speler.OrgineleInzet + "\n";
                    }
                    else if (dealerTotaal > 21 || gesplitsttotaal > dealerTotaal)
                    {
                        speler.Uitbetaling(speler.OrgineleInzet * 2);
                        overzicht += "Wint €" + speler.OrgineleInzet + " (1:1). Bankroll: €" + speler.Bankroll + "\n";
                    }
                    else if (gesplitsttotaal == dealerTotaal)
                    {
                        speler.Uitbetaling(speler.OrgineleInzet);
                        overzicht += "Gelijkspel! Inzet terug. Bankroll: €" + speler.Bankroll + "\n";
                    }
                    else
                    {
                        overzicht += "Verloren! Bankroll: €" + speler.Bankroll + "\n";
                    }
                }
            }

            MessageBox.Show(overzicht);

            // Reset voor nieuwe ronde
            huidigeSpelerIndex = 0;
            dealStap = 0;
        }
    }
}
