using System;
using System.Collections.Generic;
using System.Linq;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Models;

namespace PokerTable.Game.Extensions
{
    internal static class EntityToModelExtensions
    {
        internal static Seat ToSeatModel(this SeatEntity seatEntity)
        {
            var seat = new Seat
            {
                Id = seatEntity.SeatId, 
                IsDealer = seatEntity.IsDealer
            };

            if (!string.IsNullOrEmpty(seatEntity.PlayerId))
            {
                seat.PlayerId = Guid.Parse(seatEntity.PlayerId);
            }

            return seat;
        }

        internal static List<Seat> ToSeatModelList(this List<SeatEntity> seatEntities)
        {
            var seats = seatEntities.Select(
                entity => entity.ToSeatModel()).ToList();

            return seats.OrderBy(x => x.Id).ToList();
        }

        internal static Player ToPlayerModel(this PlayerEntity playerEntity)
        {
            var player = new Player(playerEntity.Name)
            {
                Id = Guid.Parse(playerEntity.PlayerId),
                State = (Player.States) playerEntity.State,
                Cards = Util.DeSerialize<List<Card>>(playerEntity.Cards)
            };
            return player;
        }

        internal static List<Player> ToPlayerModelList(this List<PlayerEntity> playerEntities)
        {
            return playerEntities.Select(
                entity => entity.ToPlayerModel()).ToList();
        }

        internal static Table ToTableModel(this PokerTableEntity pokerTableEntity)
        {
            var table = new Table(pokerTableEntity.Name, pokerTableEntity.Password)
            {
                Id = Guid.Parse(pokerTableEntity.PartitionKey),
                Deck = Util.DeSerialize<Deck>(pokerTableEntity.Deck),
                Burn = Util.DeSerialize<List<Card>>(pokerTableEntity.BurnCards),
                PublicCards = Util.DeSerialize<List<Card>>(pokerTableEntity.PublicCards)
            };
            return table;
        }
    }
}
