using System;
using System.Collections.Generic;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Deck
    {
        public Deck()
        {
            this.Cards = new List<Card>();
        }

       public List<Card> Cards { get; set; }
    }
}
