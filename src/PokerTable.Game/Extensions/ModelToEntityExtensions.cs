using System;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Models;

namespace PokerTable.Game.Extensions
{
    internal static class ModelToEntityExtensions
    {
        internal static SeatEntity ToSeatEntity(this Seat seat, Guid tableId)
        {
            var seatEntity = new SeatEntity(tableId, seat.Id)
            {
                SeatId = seat.Id,
                IsDealer = seat.IsDealer,
                PlayerId = seat.PlayerId.HasValue ? seat.PlayerId.Value.ToString() : string.Empty
            };

            return seatEntity;
        }

        internal static PlayerEntity ToPlayerEntity(this Player player, Guid tableId)
        {
            var playerEntity = new PlayerEntity(tableId, player.Id)
            {
                PlayerId = player.Id.ToString(),
                Name = player.Name,
                Cards = Util.Serialize(player.Cards),
                State = (int) player.State
            };
            return playerEntity;
        }

        internal static PokerTableEntity ToPokerTableEntity(this Table table)
        {
            var pokerTableEntity = new PokerTableEntity(table.Id)
            {
                Name = table.Name,
                Password = table.Password,
                Deck = Util.Serialize(table.Deck),
                BurnCards = Util.Serialize(table.Burn),
                PublicCards = Util.Serialize(table.PublicCards),
                LastUpdatedUtc = DateTime.UtcNow
            };
            return pokerTableEntity;
        }
    }
}
