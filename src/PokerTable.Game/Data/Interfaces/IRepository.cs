using System;
using System.Collections.Generic;
using PokerTable.Game.Models;

namespace PokerTable.Game.Data.Interfaces
{
    public interface IRepository
    {
        void SavePlayer(Guid tableId, Player player);

        void SavePlayerAll(Guid tableId, List<Player> players);

        void RemovePlayer(Guid tableId, Player player);

        void SaveSeat(Guid tableId, Seat seat);

        void SaveSeatAll(Guid tableId, List<Seat> seats);

        void RemoveSeat(Guid tableId, Seat seat);

        Table LoadTable(Guid tableId);

        void SaveTable(Table table);

        void DeleteOldTables();

        bool TablePasswordExists(string tablePassword);

        Guid? GetTableIdByTablePassword(string tablePassword);
    }
}
