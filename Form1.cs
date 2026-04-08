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
        }

        // Knop: Start een nieuw spel
        private void button1_Click(object sender, EventArgs e)
        {
            deck = new Deck();
            dealer = new Dealer();
            speler = new Speler();

            // Inzet ophalen via inputdialoog
            string input = Microsoft.VisualBasic.Interaction.InputBox("Hoeveel wil je inzetten? (Max €" + speler.Bankroll + ")", "Inzet plaatsen", "10");

            // Valideer of de invoer een geldig getal is
            if (!decimal.TryParse(input, out decimal inzet)|| inzet <= 0 || inzet > speler.Bankroll)
            {
                MessageBox.Show("Ongeldige inzet. Probeer opnieuw.");
                return;
            }

            speler.PlaatsInzet(inzet);

            deck.shuffle();

            // Dealer en speler krijgen elk twee kaarten
            dealer.AddCard(deck.DrawCard());
            dealer.AddCard(deck.DrawCard());
            speler.AddCard(deck.DrawCard());
            speler.AddCard(deck.DrawCard());

            // Toon kaarten speler en eerste kaart dealer
            string output = "Speler kaarten:\n";
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

            string resultaat = $"Speler: {spelerTotaal}\nDealer: {dealerTotaal}\n\n";

            //Bepaald wie gewonnen heeft
            if (spelerTotaal > 21)
            {
                // Bust: speler verliest, geen uitbetaling
                resultaat += $"Bust! Je verliest €{speler.Inzet}.";
            }
            else if (dealerTotaal > 21 || spelerTotaal > dealerTotaal)
            {
                // Winst: inzet terug + 1:1 winst
                speler.Uitbetaling(speler.Inzet * 2);
                resultaat += $"Gewonnen! Je ontvangt €{speler.Inzet * 2} (1:1).\nBankroll: €{speler.Bankroll}";
            }
            else if (spelerTotaal == dealerTotaal)
            {
                resultaat += "Gelijkspel!";
            }
            else
            {
                resultaat += $"Verloren! Dealer wint met {dealerTotaal}.";
            }

            MessageBox.Show(resultaat);
        }
    }
}