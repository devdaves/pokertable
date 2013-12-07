using System;
using PokerTable.Game.Models.Interfaces;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Seat : ISeat
    {
        public int Id { get; set; }

        public bool IsDealer { get; set; }

        public Guid? PlayerId { get; set; }

        public int DealOrder { get; set; }
    }
}
