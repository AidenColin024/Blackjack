using Blackjack.Models;
using System;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        private Deck deck;
        private Dealer dealer;
        private List<Speler> spelers;
        private int huidigeSpelerIndex = 0;
        private int dealerScore = 0;

        // Houdt bij in welke stap van het uitdelen we zitten
        private int dealStap = 0;

        public Form1()
        {
            InitializeComponent();

            // Spelers worden eenmalig aangemaakt zodat bankroll bewaard blijft tussen rondes
            spelers = new List<Speler>();


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

        // Knop 2: Hit - huidige speler vraagt een extra kaart
        private void button2_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet hitten.");
                return;
            }

            Speler huidigeSpeler = spelers[huidigeSpelerIndex];
            int totaalVoorHit = huidigeSpeler.hand.GetTotalValue();

            // Controleer of Hit de juiste beslissing was
            // Hit is correct als het totaal van de speler 11 of lager is
            if (totaalVoorHit <= 11)
            {
                dealerScore++;
                MessageBox.Show("Goede beslissing! Je score: " + dealerScore);
            }
            else if (totaalVoorHit >= 17)
            {
                dealerScore--;
                MessageBox.Show("Foute beslissing! Bij " + totaalVoorHit + " had je moeten standen.\nJe score: " + dealerScore);
            }
            else
            {
                // Tussen 12 en 16 is het situatieafhankelijk, geen straf of punt
                MessageBox.Show("Twijfelgeval bij " + totaalVoorHit + ". Geen punt of strafpunt.");
            }

            huidigeSpeler.AddCard(deck.DrawCard());
            int totaal = huidigeSpeler.hand.GetTotalValue();

            // Controleer of de speler bust is (meer dan 21 punten)
            if (totaal > 21)
            {
                MessageBox.Show(huidigeSpeler.Naam + " trekt: " + huidigeSpeler.hand.cards[huidigeSpeler.hand.cards.Count - 1] + "\nTotaal: " + totaal + "\nBust! Je verliest.");
                VolgendeSpeler();
            }
            else
            {
                MessageBox.Show(huidigeSpeler.Naam + " trekt: " + huidigeSpeler.hand.cards[huidigeSpeler.hand.cards.Count - 1] + "\nTotaal: " + totaal);
            }
        }

        // Knop 3: Stand - huidige speler past, volgende speler of dealer is aan de beurt
        private void button3_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet standen.");
                return;
            }

            Speler huidigeSpeler = spelers[huidigeSpelerIndex];
            int totaal = huidigeSpeler.hand.GetTotalValue();

            // Controleer of Stand de juiste beslissing was
            // Stand is correct als het totaal van de speler 17 of hoger is
            if (totaal >= 17)
            {
                dealerScore++;
                MessageBox.Show("Goede beslissing! Je score: " + dealerScore);
            }
            else
            {
                dealerScore--;
                MessageBox.Show("Foute beslissing! Bij " + totaal + " had je moeten hitten.\nJe score: " + dealerScore);
            }

            MessageBox.Show(huidigeSpeler.Naam + " past met totaal: " + totaal);
            VolgendeSpeler();
        }

        // Bepaalt de uitslag voor alle spelers en verwerkt de uitbetalingen
        // Regels: bust verliest, Blackjack betaalt 3:2, gewone winst 1:1, gelijkspel = push
        private void BepaalUitslagAlleSpelers()
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
                    decimal winst = speler.Inzet * 1.5m;
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
            }

            MessageBox.Show(overzicht);

            // Reset voor nieuwe ronde
            huidigeSpelerIndex = 0;
            dealStap = 0;
        }

        // Gaat naar de volgende speler, of naar de dealer als alle spelers klaar zijn
        private void VolgendeSpeler()
        {
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
                    BepaalUitslagAlleSpelers();
                }
                else
                {
                    MessageBox.Show("Dealer heeft " + dealer.hand.GetTotalValue() + ". Druk op Deal om een kaart te trekken.");
                }
            }
        }

        // Knop 4: Start spel - vraag aantal spelers en namen
        private void button4_Click(object sender, EventArgs e)
        {
            spelers.Clear();
            huidigeSpelerIndex = 0;

            // Vraag hoeveel spelers er zijn
            string aantalInput = Microsoft.VisualBasic.Interaction.InputBox("Hoeveel spelers? (1 tot 4)", "Aantal spelers", "1");

            int aantalSpelers;
            bool aantalGeldig = int.TryParse(aantalInput, out aantalSpelers);

            if (aantalGeldig == false || aantalSpelers < 1 || aantalSpelers > 4)
            {
                MessageBox.Show("Ongeldig aantal. Kies tussen 1 en 4 spelers.");
                return;
            }

            // Vraag de naam van elke speler
            for (int i = 0; i < aantalSpelers; i++)
            {
                string naam = Microsoft.VisualBasic.Interaction.InputBox("Naam van speler " + (i + 1) + ":", "Spelersnaam", "Speler " + (i + 1));

                if (naam == "")
                {
                    naam = "Speler " + (i + 1);
                }

                spelers.Add(new Speler(naam));
            }

            // Vraag inzet voor elke speler
            for (int i = 0; i < spelers.Count; i++)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Inzet voor " + spelers[i].Naam + "? (Bankroll: €" + spelers[i].Bankroll + ")", "Inzet plaatsen", "10");

                decimal inzet;
                bool isGeldig = decimal.TryParse(input, out inzet);

                if (isGeldig == false || inzet <= 0 || inzet > spelers[i].Bankroll)
                {
                    MessageBox.Show("Ongeldige inzet voor " + spelers[i].Naam + ". Inzet wordt €10.");
                    inzet = 10;
                }

                spelers[i].PlaatsInzet(inzet);
            }

            // Deck en dealer aanmaken
            deck = new Deck();
            dealer = new Dealer();
            deck.shuffle();
            dealStap = 1;

            MessageBox.Show("Spel gestart met " + spelers.Count + " speler(s)!\nDruk op Deal om te beginnen.");
        }

        // Knop 5: Deal - deelt per klik één kaart
        private void button5_Click(object sender, EventArgs e)
        {
            if (dealStap == 0)
            {
                MessageBox.Show("Start eerst een spel.");
                return;
            }

            if (dealStap == 1)
            {
                // Eerste kaart voor elke speler
                string overzicht = "Eerste kaart per speler:\n\n";
                foreach (Speler speler in spelers)
                {
                    speler.AddCard(deck.DrawCard());
                    overzicht += speler.Naam + ": " + speler.hand.cards[0] + "\n";
                }
                MessageBox.Show(overzicht);
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
                // Tweede kaart voor elke speler
                string overzicht = "Tweede kaart per speler:\n\n";
                foreach (Speler speler in spelers)
                {
                    speler.AddCard(deck.DrawCard());
                    overzicht += speler.Naam + ": " + speler.hand.cards[1] + " (totaal: " + speler.hand.GetTotalValue() + ")\n";
                }
                MessageBox.Show(overzicht);
                dealStap = 4;
            }
            else if (dealStap == 4)
            {
                // Tweede kaart dealer gesloten
                dealer.AddCard(deck.DrawCard());
                MessageBox.Show("Dealer deelt zichzelf een gesloten kaart.\nDealer zichtbare kaart: " + dealer.hand.cards[0] + "\n\nBegin met de beurt van " + spelers[huidigeSpelerIndex].Naam + ".");
                dealStap = 5;
            }
            else if (dealStap == 6)
            {
                // Dealer trekt zelf één kaart per klik
                Card nieuweKaart = deck.DrawCard();
                dealer.AddCard(nieuweKaart);
                MessageBox.Show("Dealer trekt: " + nieuweKaart + "\nDealer totaal: " + dealer.hand.GetTotalValue());

                // Controleer of dealer genoeg heeft
                if (dealer.hand.GetTotalValue() >= 17)
                {
                    MessageBox.Show("Dealer totaal is " + dealer.hand.GetTotalValue() + ". Dealer past.");
                    BepaalUitslagAlleSpelers();
                }
            }
        }
    }
}