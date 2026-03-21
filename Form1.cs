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

            String output = "";

            foreach (Card card in deck.cards)
            {
                output += card.ToString() + "\n";
            }
            MessageBox.Show(output);
        }
    }
}