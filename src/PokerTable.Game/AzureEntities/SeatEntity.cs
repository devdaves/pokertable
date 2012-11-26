using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    /// <summary>
    /// Seat Entity
    /// </summary>
    internal class SeatEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatEntity" /> class.
        /// </summary>
        public SeatEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatEntity" /> class.
        /// </summary>
        /// <param name="tableId">The table Id</param>
        /// <param name="seatId">The seat Id</param>
        public SeatEntity(Guid tableId, int seatId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = string.Format("Seat-{0}", seatId);
        }

        /// <summary>
        /// Gets or sets the seat id.
        /// </summary>
        /// <value>
        /// The seat id.
        /// </value>
        public int SeatId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dealer.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dealer; otherwise, <c>false</c>.
        /// </value>
        public bool IsDealer { get; set; }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        public string PlayerId { get; set; }
    }
}
