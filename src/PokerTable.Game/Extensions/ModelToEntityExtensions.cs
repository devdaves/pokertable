using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Extensions
{
    /// <summary>
    /// Model to Entity Extension Methods
    /// </summary>
    internal static class ModelToEntityExtensions
    {
        /// <summary>
        /// To the seat entity.
        /// </summary>
        /// <param name="seat">The seat.</param>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns a seat entity
        /// </returns>
        internal static SeatEntity ToSeatEntity(this ISeat seat, Guid tableId)
        {
            var seatEntity = new SeatEntity(tableId, seat.Id);
            seatEntity.SeatId = seat.Id;
            seatEntity.IsDealer = seat.IsDealer;
            seatEntity.PlayerId = seat.PlayerId.HasValue ? seat.PlayerId.Value.ToString() : string.Empty;

            return seatEntity;
        }

        /// <summary>
        /// To the player entity.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns a player entity
        /// </returns>
        internal static PlayerEntity ToPlayerEntity(this IPlayer player, Guid tableId)
        {
            var playerEntity = new PlayerEntity(tableId, player.ID);
            playerEntity.PlayerId = player.ID.ToString();
            playerEntity.Name = player.Name;
            playerEntity.Cards = Util.Serialize<List<ICard>>(player.Cards);
            playerEntity.State = (int)player.State;
            return playerEntity;
        }

        /// <summary>
        /// To the poker table entity.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>
        /// returns a poker table entity
        /// </returns>
        internal static PokerTableEntity ToPokerTableEntity(this ITable table)
        {
            var pokerTableEntity = new PokerTableEntity(table.Id);
            pokerTableEntity.Name = table.Name;
            pokerTableEntity.Password = table.Password;
            pokerTableEntity.Deck = Util.Serialize<IDeck>(table.Deck);
            pokerTableEntity.BurnCards = Util.Serialize<List<ICard>>(table.Burn);
            pokerTableEntity.PublicCards = Util.Serialize<List<ICard>>(table.PublicCards);
            return pokerTableEntity;
        }
    }
}
