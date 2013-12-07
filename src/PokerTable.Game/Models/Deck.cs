using System;
using System.Collections.Generic;
using PokerTable.Game.Models.Interfaces;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Deck : IDeck
    {
        public Deck()
        {
            this.Cards = new List<Card>();
        }

       public List<Card> Cards { get; set; }
    }
}
