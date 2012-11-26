using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    /// <summary>
    /// Player Entity
    /// </summary>
    internal class PlayerEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity" /> class.
        /// </summary>
        public PlayerEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity" /> class.
        /// </summary>
        /// <param name="tableId">The Table Id</param>
        /// <param name="playerId">The Player Id</param>
        public PlayerEntity(Guid tableId, Guid playerId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = string.Format("Player-{0}", playerId);
        }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        public string PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        public string Cards { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State { get; set; }
    }
}
