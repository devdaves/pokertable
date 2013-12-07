using System;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface IEngine
    {
        Table Table { get; set; }

        void CreateNewTable(int numberOfSeats, string name);

        void LoadTable(Guid tableId);

        Guid JoinTable(string tablePassword, string playerName);

        bool AssignSeatToPlayer(int seatId, Guid playerId);

        bool RemovePlayerFromSeat(int seatId);

        bool RemovePlayerFromSeat(Guid playerId);

        bool DealerExists();

        void SetDealer(int seatId);

        void NextDealer();

        void ShuffleDeck();

        void DealPlayers();

        void DealFlop();

        void DealTurn();

        void DealRiver();

        void FoldPlayer(Guid playerId);

        void ResetTable();

        bool AddSeat(bool saveNow = true);

        bool RemoveSeat(int seatId);

        bool AddPlayer(Player player);

        bool RemovePlayer(Guid playerId);
    }
}
