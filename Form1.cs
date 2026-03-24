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

            deck.shuffle();

            Card card1 = deck.DrawCard();
            Card card2 = deck.DrawCard();
            Card card3 = deck.DrawCard();

            MessageBox.Show(card1 + "\n" + card2 + "\n" + card3);
        }
    }
}