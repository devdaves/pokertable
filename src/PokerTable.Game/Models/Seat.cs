using System;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Seat
    {
        public int Id { get; set; }

        public bool IsDealer { get; set; }

        public Guid? PlayerId { get; set; }

        public int DealOrder { get; set; }
    }
}
