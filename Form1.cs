using Blackjack.Models;
using System;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        private Deck deck;
        private Dealer dealer;
        private Speler speler;

        public Form1()
        {
            InitializeComponent();

           speler = new Speler();
        }

        // Knop: Start een nieuw spel
        private void button1_Click(object sender, EventArgs e)
        {
            // Alleen deck en dealer resetten per ronde, speler blijft hetzelfde
            deck = new Deck();
            dealer = new Dealer();

            //inzet ophalen via inputdialoog
            string input = Microsoft.VisualBasic.Interaction.InputBox("Hoeveel wil je inzetten? (Max €" + speler.Bankroll + ")", "Inzet plaatsen", "10");

            //valideer of inzet geldig is
            if (!decimal.TryParse(input, out decimal inzet) || inzet <= 0 || inzet > speler.Bankroll)
            {
                MessageBox.Show("Ongeldige inzet. Probeer opnieuw.");
                return;
            }

            // Hand resetten voor nieuwe ronde
            speler.hand = new Hand();
            speler.PlaatsInzet(inzet);

            deck.shuffle();

            dealer.AddCard(deck.DrawCard());
            dealer.AddCard(deck.DrawCard());
            speler.AddCard(deck.DrawCard());
            speler.AddCard(deck.DrawCard());

            //toon 2 kaarten van de speler en de eerse kaart van de dealer
            string output = $"Inzet: €{speler.Inzet} | Bankroll: €{speler.Bankroll}\n\n";
            output += "Speler kaarten:\n";
            foreach (Card card in speler.hand.cards)
                output += card + "\n";
            output += "Totaal: " + speler.hand.GetTotalValue();
            output += "\n\nDealer zichtbare kaart: " + dealer.hand.cards[0];

            MessageBox.Show(output);
        }

        // Knop: Hit - speler vraagt een extra kaart
        private void button2_Click(object sender, EventArgs e)
        {
            if (deck == null || speler == null)
            {
                MessageBox.Show("Start eerst een nieuw spel.");
                return;
            }

            speler.AddCard(deck.DrawCard());
            int totaal = speler.hand.GetTotalValue();

            // Controleer of de speler bust is (meer dan 21 punten)
            if (totaal > 21)
                MessageBox.Show("Totaal speler: " + totaal + "\nBust! Je verliest.");
            else
                MessageBox.Show("Totaal speler: " + totaal);
        }

        // Knop: Stand - dealer speelt en uitslag wordt bepaald
        // Knop: Stand - dealer speelt en uitslag wordt bepaald
        private void button3_Click(object sender, EventArgs e)
        {
            if (deck == null || dealer == null || speler == null)
            {
                MessageBox.Show("Start eerst een nieuw spel.");
                return;
            }

            // Dealer trekt kaarten tot 17 of hoger
            dealer.Play(deck);

            int spelerTotaal = speler.hand.GetTotalValue();
            int dealerTotaal = dealer.hand.GetTotalValue();

            // Blackjack = 21 punten met exact 2 kaarten
            bool spelerBlackjack = spelerTotaal == 21 && speler.hand.cards.Count == 2;

            string resultaat = $"Speler: {spelerTotaal}\nDealer: {dealerTotaal}\n\n";

            //Bepaald wie gewonnen heeft
            if (spelerTotaal > 21)
            {
                // Bust: speler verliest, geen uitbetaling
                resultaat += $"Bust! Je verliest €{speler.Inzet}.";
            }
            else if (spelerBlackjack) 
            { 
                // Blackjack: inzet terug + 3:2 winst
                decimal winst = speler.Inzet * 1.5m;
                speler.Uitbetaling(speler.Inzet + winst);
                resultaat += $"Blackjack! Je wint €{winst} (3:2).\nBankroll: €{speler.Bankroll}";
            }
            else if (dealerTotaal > 21 || spelerTotaal > dealerTotaal)
            {
                // Winst: inzet terug + 1:1 winst
                speler.Uitbetaling(speler.Inzet * 2);
                resultaat += $"Gewonnen! Je ontvangt €{speler.Inzet * 2} (1:1).\nBankroll: €{speler.Bankroll}";
            }
            else if (spelerTotaal == dealerTotaal)
            {
                // Push: inzet terug, geen winst of verlies
                speler.Uitbetaling(speler.Inzet);
                resultaat += $"Gelijkspel! Je krijgt €{speler.Inzet} terug.\nBankroll: €{speler.Bankroll}";
            }
            else
            {
                resultaat += $"Verloren! Dealer wint met {dealerTotaal}.";
            }

            MessageBox.Show(resultaat);
        }
    }
}