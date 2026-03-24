using Blackjack.Models;
using System;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Deck deck = new Deck();

            Hand hand = new Hand();

            deck.shuffle();

            hand.AddCard(deck.DrawCard());
            hand.AddCard(deck.DrawCard());

            string output = "";

            foreach (Card card in hand.cards)
            {
                output += card + "\n";
            }

            output += "\nTotaal: " + hand.GetTotalValue();

            MessageBox.Show(output);
        }
    }
}