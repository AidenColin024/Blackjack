using Blackjack.Models;
using System;
using System.Drawing.Text;
using System.Media;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Deck deck = new Deck();

            Dealer dealer = new Dealer();
            Speler speler = new Speler();

            deck.shuffle();

            dealer.AddCard(deck.DrawCard());
            dealer.AddCard(deck.DrawCard());

            speler.AddCard(deck.DrawCard());
            speler.AddCard(deck.DrawCard());

            dealer.Play(deck);

            string output = "";

            foreach (Card card in dealer.hand.cards)
            {
                output += card + "\n";
            }

            output += "\nTotaal: " + dealer.hand.GetTotalValue();

            MessageBox.Show(output);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            speler.AddCard(deck.DrawCard());

            MessageBox.Show("Totaal speler " + speler.GetTotalValue());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dealer.Play(deck);

            MessageBox.Show("Dealer totaal " + dealer.hand.GetTotalValue());
        }
    }
}