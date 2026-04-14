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
        private bool heeftVerdubbeld = false;
        private bool heeftGesplitst = false;

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

            MessageBox.Show(huidigeSpeler.Naam + " past met totaal: " + totaal);
            VolgendeSpeler();
        }

        // Knop 4: Start spel - vraag aantal spelers en namen
        private void button4_Click(object sender, EventArgs e)
        {
            spelers.Clear();
            huidigeSpelerIndex = 0;
            heeftVerdubbeld = false;
            heeftGesplitst = false;

            foreach (Speler speler in spelers)
            {
                speler.ResetSplit();
            }

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

                double inzet;
                bool isGeldig = double.TryParse(input, out inzet);

                if (isGeldig == false || inzet <= 0 || inzet > spelers[i].Bankroll)
                {
                    MessageBox.Show("Ongeldige inzet voor " + spelers[i].Naam + ". Inzet wordt €10.");
                    inzet = 10;
                }

                spelers[i].PlaatsInzet(inzet);
            }

            // Vraag hoeveel decks de shoe moet bevatten
            string deckInput = Microsoft.VisualBasic.Interaction.InputBox("Hoeveel decks in de shoe? (1, 4 of 6)", "Shoe", "1");

            int aantalDecks;
            bool deckGeldig = int.TryParse(deckInput, out aantalDecks);

            if (deckGeldig == false || (aantalDecks != 1 && aantalDecks != 4 && aantalDecks != 6))
            {
                MessageBox.Show("Ongeldig aantal decks. Er wordt 1 deck gebruikt.");
                aantalDecks = 1;
            }

            // Maak de shoe aan met het opgegeven aantal decks
            deck = new Deck(aantalDecks);

            //dealer aanmaken
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
                // Controleer of de dealer de juiste beslissing maakt
                // Dealer moet trekken onder 17, standen op 17 of hoger
                if (dealer.hand.GetTotalValue() >= 17)
                {
                    // Dealer drukt op Deal terwijl hij al 17 of hoger heeft, foute beslissing
                    dealerScore--;
                    MessageBox.Show("Foute beslissing! Bij " + dealer.hand.GetTotalValue() + " moet je passen.\nJe score: " + dealerScore);
                    return;
                }

                // Dealer trekt zelf één kaart per klik
                Card nieuweKaart = deck.DrawCard();
                dealer.AddCard(nieuweKaart);

                // Juiste beslissing want dealer had minder dan 17
                dealerScore++;
                MessageBox.Show("Goede beslissing! Je trekt bij " + (dealer.hand.GetTotalValue() - nieuweKaart.GetValue()) + ".\nDealer trekt: " + nieuweKaart + "\nDealer totaal: " + dealer.hand.GetTotalValue() + "\nJe score: " + dealerScore);

                if (dealer.hand.GetTotalValue() >= 17)
                {
                    MessageBox.Show("Dealer totaal is " + dealer.hand.GetTotalValue() + ". Dealer past.");
                    BepaalUitslagAlleSpelers();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet verdubbelen.");
                return;
            }

            Speler huidigeSpeler = spelers[huidigeSpelerIndex];

            // Verdubbelen mag alleen met de eerste twee kaarten
            if (huidigeSpeler.hand.cards.Count != 2)
            {
                MessageBox.Show("Je kan alleen verdubbelen met je eerste twee kaarten.");
                return;
            }

            // Controleer of de speler genoeg bankroll heeft om te verdubbelen
            if (huidigeSpeler.Inzet > huidigeSpeler.Bankroll)
            {
                MessageBox.Show("Niet genoeg bankroll om te verdubbelen.");
                return;
            }

            // Sla de oude inzet op voor de melding
            double oudeInzet = huidigeSpeler.Inzet;

            // Verdubbel de inzet door nog eens hetzelfde bedrag in te zetten
            huidigeSpeler.PlaatsInzet(huidigeSpeler.Inzet);
            heeftVerdubbeld = true;

            // Speler krijgt precies één extra kaart
            huidigeSpeler.AddCard(deck.DrawCard());
            int totaal = huidigeSpeler.hand.GetTotalValue();

            // Dealer krijgt een punt want verdubbelen is correct afgehandeld
            dealerScore++;

            MessageBox.Show(huidigeSpeler.Naam + " verdubbelt!" +
                "\nOude inzet: €" + oudeInzet +
                "\nNieuwe totale inzet: €" + (oudeInzet * 2) +
                "\nGekregen kaart: " + huidigeSpeler.hand.cards[huidigeSpeler.hand.cards.Count - 1] +
                "\nTotaal: " + totaal +
                "\nGoede beslissing! Score: " + dealerScore);

            // Speler staat automatisch na verdubbelen
            VolgendeSpeler();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dealStap != 5)
            {
                MessageBox.Show("Je kan nu niet splitsen.");
                return;
            }

            Speler huidigeSpeler = spelers[huidigeSpelerIndex];

            // Splitsen mag alleen met de eerste twee kaarten
            if (huidigeSpeler.hand.cards.Count != 2)
            {
                MessageBox.Show("Je kan alleen splitsen met je eerste twee kaarten.");
                return;
            }

            // Controleer of de twee kaarten dezelfde waarde hebben
            if (huidigeSpeler.hand.cards[0].GetValue() != huidigeSpeler.hand.cards[1].GetValue())
            {
                MessageBox.Show("Je kan alleen splitsen als beide kaarten dezelfde waarde hebben.");
                return;
            }

            // Controleer of de speler genoeg bankroll heeft voor de extra inzet
            if (huidigeSpeler.Inzet > huidigeSpeler.Bankroll)
            {
                MessageBox.Show("Niet genoeg bankroll om te splitsen.");
                return;
            }

            // Sla de oude inzet op
            double oudeInzet = huidigeSpeler.Inzet;

            // Splits de hand en plaats dezelfde inzet op de tweede hand
            huidigeSpeler.Split();
            huidigeSpeler.PlaatsInzet(huidigeSpeler.Inzet);
            heeftGesplitst = true;

            // Geef elke hand een extra kaart
            huidigeSpeler.hand.AddCard(deck.DrawCard());
            huidigeSpeler.gesplitsteHand.AddCard(deck.DrawCard());

            // Dealer krijgt een punt want splitsen is correct afgehandeld
            dealerScore++;

            MessageBox.Show(huidigeSpeler.Naam + " splitst!" +
                "\nHand 1: " + huidigeSpeler.hand.cards[0] + " + " + huidigeSpeler.hand.cards[1] + " (totaal: " + huidigeSpeler.hand.GetTotalValue() + ")" +
                "\nHand 2: " + huidigeSpeler.gesplitsteHand.cards[0] + " + " + huidigeSpeler.gesplitsteHand.cards[1] + " (totaal: " + huidigeSpeler.gesplitsteHand.GetTotalValue() + ")" +
                "\nGoede beslissing! Score: " + dealerScore);
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

        // Gaat naar de volgende speler, of naar de dealer als alle spelers klaar zijn
        private void VolgendeSpeler()
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
                    BepaalUitslagAlleSpelers();
                }
                else
                {
                    MessageBox.Show("Dealer heeft " + dealer.hand.GetTotalValue() + ". Druk op Deal om een kaart te trekken.");
                }
            }
        }
    }
}