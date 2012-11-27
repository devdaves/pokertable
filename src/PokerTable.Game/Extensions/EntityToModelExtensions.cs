using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Models;

namespace PokerTable.Game.Extensions
{
    /// <summary>
    /// Entity to Model Extension Methods
    /// </summary>
    internal static class EntityToModelExtensions
    {
        /// <summary>
        /// To the seat model.
        /// </summary>
        /// <param name="seatEntity">The seat entity.</param>
        /// <returns>returns a seat</returns>
        internal static Seat ToSeatModel(this SeatEntity seatEntity)
        {
            var seat = new Seat();
            seat.Id = seatEntity.SeatId;
            seat.IsDealer = seatEntity.IsDealer;
            if (!string.IsNullOrEmpty(seatEntity.PlayerId))
            {
                seat.PlayerId = Guid.Parse(seatEntity.PlayerId); 
            }

            return seat;
        }

        /// <summary>
        /// To the player model.
        /// </summary>
        /// <param name="playerEntity">The player entity.</param>
        /// <returns>returns a player</returns>
        internal static Player ToPlayerModel(this PlayerEntity playerEntity)
        {
            var player = new Player(playerEntity.Name);
            player.ID = Guid.Parse(playerEntity.PlayerId);
            player.State = (Player.States)playerEntity.State;
            player.Cards = Util.DeSerialize<List<Card>>(playerEntity.Cards);
            return player;
        }

        /// <summary>
        /// To the table model.
        /// </summary>
        /// <param name="pokerTableEntity">The poker table entity.</param>
        /// <returns>returns a table</returns>
        internal static Table ToTableModel(this PokerTableEntity pokerTableEntity)
        {
            var table = new Table(pokerTableEntity.Name, pokerTableEntity.Password);
            table.Id = Guid.Parse(pokerTableEntity.PartitionKey);
            table.Deck = Util.DeSerialize<Deck>(pokerTableEntity.Deck);
            table.Burn = Util.DeSerialize<List<Card>>(pokerTableEntity.BurnCards);
            table.PublicCards = Util.DeSerialize<List<Card>>(pokerTableEntity.PublicCards);
            return table;
        }
    }
}
