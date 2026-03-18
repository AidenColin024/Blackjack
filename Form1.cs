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

        private void btnDrawCard_Click(object sender, EventArgs e)
        {
            Card card = new Card(Suit.Clubs,Rank.Ace);

            MessageBox.Show(card.ToString());
        }
    }
}