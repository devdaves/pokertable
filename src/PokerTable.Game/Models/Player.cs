using System;
using System.Collections.Generic;
using PokerTable.Game.Models.Interfaces;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Player : IPlayer
    {
        public Player(string playerName)
        {
            this.Id = Guid.NewGuid();
            this.Name = playerName;
            this.Cards = new List<Card>();
            this.State = States.Available;
        }

        public enum States
        {
            Available = 1,
            Folded = 2,
            SittingOut = 3
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Card> Cards { get; set; }

        public States State { get; set; }
    }
}
