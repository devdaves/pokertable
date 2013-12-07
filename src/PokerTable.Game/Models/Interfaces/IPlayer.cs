using System;
using System.Collections.Generic;

namespace PokerTable.Game.Models.Interfaces
{
    public interface IPlayer
    {
        Guid Id { get; set; }

        string Name { get; set; }

        List<Card> Cards { get; set; }

        Player.States State { get; set; }
    }
}
