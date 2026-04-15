using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Blackjack.Models
{
    internal class VolgendeSpeler
    {

        // Gaat naar de volgende speler, of naar de dealer als alle spelers klaar zijn
        public void volgendespeler(List<Speler> spelers, Dealer dealer, ref bool heeftVerdubbeld, ref bool heeftGesplitst, ref int huidigeSpelerIndex, ref int dealStap, ref int dealerScore)
        {
            heeftVerdubbeld = false;
            heeftGesplitst = false;
            huidigeSpelerIndex++;

            if (huidigeSpelerIndex < spelers.Count)
            {
                // Nog niet alle spelers geweest, toon beurt van volgende speler
                Speler volgende = spelers[huidigeSpelerIndex];
                MessageBox.Show("Beurt van " + volgende.Naam + ".\nTotaal: " + volgende.hand.GetTotalValue() + "\nDealer zichtbare kaart: " + dealer.hand.cards[0]);
            }
            else
            {
                // Alle spelers zijn geweest, dealer is aan de beurt
                MessageBox.Show("Alle spelers zijn geweest.\nDealer onthult gesloten kaart: " + dealer.hand.cards[1] + "\nDealer totaal: " + dealer.hand.GetTotalValue());
                dealStap = 6;

                // Als dealer al op 17 of hoger zit hoeft hij niet meer te trekken
                if (dealer.hand.GetTotalValue() >= 17)
                {
                    MessageBox.Show("Dealer heeft al " + dealer.hand.GetTotalValue() + ". Dealer past.");

                    BepaalUitslagAlleSpelers bepaalUitslag = new BepaalUitslagAlleSpelers();
                    bepaalUitslag.bepaaluitslagallespelers(spelers, dealer, huidigeSpelerIndex, dealerScore, dealStap);
                }
                else
                {
                    MessageBox.Show("Dealer heeft " + dealer.hand.GetTotalValue() + ". Druk op Deal om een kaart te trekken.");
                }
            }
        }
    }
}
