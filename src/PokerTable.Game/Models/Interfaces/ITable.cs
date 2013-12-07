using System;
using System.Collections.Generic;

namespace PokerTable.Game.Models.Interfaces
{
    public interface ITable
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Password { get; set; }

        Deck Deck { get; set; }

        List<Seat> Seats { get; set; }

        List<Player> Players { get; set; }

        List<Card> Burn { get; set; }

        List<Card> PublicCards { get; set; }

        bool ResetTableAvailable { get; }

        bool DealPlayersAvailable { get; }

        bool DealFlopAvailable { get; }

        bool DealTurnAvailable { get; }

        bool DealRiverAvailable { get; }
    }
}
