using System;

namespace PokerTable.Game.Models.Interfaces
{
    public interface ISeat
    {
        int Id { get; set; }

        bool IsDealer { get; set; }

        Guid? PlayerId { get; set; }

        int DealOrder { get; set; }
    }
}
