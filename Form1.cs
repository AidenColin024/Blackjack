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

        // Houdt bij in welke stap van het uitdelen we zitten
        private int dealStap = 0;

        public Form1()
        {
            InitializeComponent();

            // Speler wordt eenmalig aangemaakt zodat bankroll bewaard blijft tussen rondes
            speler = new Speler();
        }

        // Knop 1: Toon alle kaarten die nog in het deck zitten
        // Handig om te controleren welke kaarten er nog over zijn
        private void button1_Click(object sender, EventArgs e)
        {
            if (deck == null)
            {
                MessageBox.Show("Er is nog geen deck. Start eerst een spel.");
                return;
            }

            // Loop door alle kaarten in het deck en voeg ze toe aan de output
            string deckOverzicht = "Kaarten in het deck (" + deck.cards.Count + " kaarten):\n\n";

            foreach (Card card in deck.cards)
            {
                deckOverzicht += card + "\n";
            }

            MessageBox.Show(deckOverzicht);
        }

        // Knop 2: Hit - speler vraagt een extra kaart
        private void button2_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet hitten.");
                return;
            }

            speler.AddCard(deck.DrawCard());
            int totaal = speler.hand.GetTotalValue();

            // Controleer of de speler bust is (meer dan 21 punten)
            if (totaal > 21)
            {
                MessageBox.Show("Speler trekt: " + speler.hand.cards[speler.hand.cards.Count - 1] + "\nSpeler totaal: " + totaal + "\nBust! Je verliest.");
                BepaalUitslag();
            }
            else
            {
                MessageBox.Show("Speler trekt: " + speler.hand.cards[speler.hand.cards.Count - 1] + "\nSpeler totaal: " + totaal);
            }
        }

        // Knop 3: Stand - speler past, dealer is aan de beurt
        private void button3_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet standen.");
                return;
            }

            // Onthul de gesloten kaart van de dealer
            MessageBox.Show("Speler past.\nDealer onthult gesloten kaart: " + dealer.hand.cards[1] + "\nDealer totaal: " + dealer.hand.GetTotalValue());
            dealStap = 6;

            // Als dealer al op 17 of hoger zit hoeft hij niet meer te trekken
            if (dealer.hand.GetTotalValue() >= 17)
            {
                MessageBox.Show("Dealer heeft al " + dealer.hand.GetTotalValue() + ". Dealer past.");
                BepaalUitslag();
            }
            else
            {
                MessageBox.Show("Dealer heeft " + dealer.hand.GetTotalValue() + ". Druk op Deal om een kaart te trekken.");
            }
        }

        // Bepaalt de uitslag en verwerkt de uitbetaling
        // Regels: bust verliest, Blackjack betaalt 3:2, gewone winst 1:1, gelijkspel = push
        private void BepaalUitslag()
        {
            int spelerTotaal = speler.hand.GetTotalValue();
            int dealerTotaal = dealer.hand.GetTotalValue();

            // Blackjack = 21 punten met exact 2 kaarten
            bool spelerBlackjack = spelerTotaal == 21 && speler.hand.cards.Count == 2;

            string resultaat = "Speler: " + spelerTotaal + "\nDealer: " + dealerTotaal + "\n\n";

            if (spelerTotaal > 21)
            {
                // Bust: speler verliest, geen uitbetaling
                resultaat = resultaat + "Bust! Speler verliest €" + speler.Inzet;
            }
            else if (spelerBlackjack)
            {
                // Blackjack: inzet terug + 3:2 winst
                decimal winst = speler.Inzet * 1.5m;
                speler.Uitbetaling(speler.Inzet + winst);
                resultaat = resultaat + "Blackjack! Speler wint €" + winst + " (3:2).\nBankroll: €" + speler.Bankroll;
            }
            else if (dealerTotaal > 21 || spelerTotaal > dealerTotaal)
            {
                // Winst: inzet terug + 1:1 winst
                speler.Uitbetaling(speler.Inzet * 2);
                resultaat = resultaat + "Speler wint! Uitbetaling €" + speler.Inzet * 2 + " (1:1).\nBankroll: €" + speler.Bankroll;
            }
            else if (spelerTotaal == dealerTotaal)
            {
                // Push: inzet terug, geen winst of verlies
                speler.Uitbetaling(speler.Inzet);
                resultaat = resultaat + "Gelijkspel! Inzet €" + speler.Inzet + " terug.\nBankroll: €" + speler.Bankroll;
            }
            else
            {
                // Verlies: dealer wint
                resultaat = resultaat + "Verloren! Dealer wint met " + dealerTotaal + ".\nBankroll: €" + speler.Bankroll;
            }

            MessageBox.Show(resultaat);

            // Reset dealStap zodat een nieuwe ronde gestart kan worden
            dealStap = 0;
        }

        // Knop 4: Start spel en inzet plaatsen
        // Maakt een nieuw deck en dealer aan, vraagt de inzet en reset de hand van de speler
        private void button4_Click(object sender, EventArgs e)
        {
            // Alleen deck en dealer resetten per ronde, speler blijft hetzelfde
            deck = new Deck();
            dealer = new Dealer();

            // Hand resetten voor nieuwe ronde
            speler.hand = new Hand();

            // Inzet ophalen via inputdialoog
            string input = Microsoft.VisualBasic.Interaction.InputBox("Hoeveel wil je inzetten? (Max €" + speler.Bankroll + ")", "Inzet plaatsen", "10");

            // Valideer of de invoer een geldig getal is
            decimal inzet;
            bool isGeldig = decimal.TryParse(input, out inzet);

            if (isGeldig == false || inzet <= 0 || inzet > speler.Bankroll)
            {
                MessageBox.Show("Ongeldige inzet. Probeer opnieuw.");
                return;
            }

            speler.PlaatsInzet(inzet);
            deck.shuffle();

            // Zet dealStap op 1 zodat de Deal knop weet dat hij mag beginnen
            dealStap = 1;

            MessageBox.Show("Spel gestart! Bankroll: €" + speler.Bankroll + "\nDruk op Deal om te beginnen.");
        }

        // Knop 5: Deal - deelt per klik één kaart
        // Elke klik gaat een stap verder in de uitdeelfase
        private void button5_Click(object sender, EventArgs e)
        {
            if (dealStap == 0)
            {
                MessageBox.Show("Start eerst een spel.");
                return;
            }

            if (dealStap == 1)
            {
                // Eerste kaart speler
                speler.AddCard(deck.DrawCard());
                MessageBox.Show("Speler krijgt: " + speler.hand.cards[0]);
                dealStap = 2;
            }
            else if (dealStap == 2)
            {
                // Eerste kaart dealer open
                dealer.AddCard(deck.DrawCard());
                MessageBox.Show("Dealer toont: " + dealer.hand.cards[0]);
                dealStap = 3;
            }
            else if (dealStap == 3)
            {
                // Tweede kaart speler
                speler.AddCard(deck.DrawCard());
                MessageBox.Show("Speler krijgt: " + speler.hand.cards[1] + "\nSpeler totaal: " + speler.hand.GetTotalValue());
                dealStap = 4;
            }
            else if (dealStap == 4)
            {
                // Tweede kaart dealer gesloten, speler mag nu kiezen
                dealer.AddCard(deck.DrawCard());
                MessageBox.Show("Dealer deelt zichzelf een gesloten kaart.\nSpeler totaal: " + speler.hand.GetTotalValue() + "\nDealer zichtbare kaart: " + dealer.hand.cards[0] + "\n\nKies Hit of Stand voor de speler.");
                dealStap = 5;
            }
            else if (dealStap == 6)
            {
                // Dealer trekt zelf één kaart per klik totdat hij 17 of hoger heeft
                Card nieuweKaart = deck.DrawCard();
                dealer.AddCard(nieuweKaart);
                MessageBox.Show("Dealer trekt: " + nieuweKaart + "\nDealer totaal: " + dealer.hand.GetTotalValue());

                // Controleer of dealer genoeg heeft
                if (dealer.hand.GetTotalValue() >= 17)
                {
                    MessageBox.Show("Dealer totaal is " + dealer.hand.GetTotalValue() + ". Dealer past.");
                    BepaalUitslag();
                }
            }
        }
    }
}