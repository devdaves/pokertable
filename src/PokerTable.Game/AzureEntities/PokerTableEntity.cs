using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    /// <summary>
    /// Poker Table Entity
    /// </summary>
    internal class PokerTableEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PokerTableEntity" /> class.
        /// </summary>
        public PokerTableEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerTableEntity" /> class.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        public PokerTableEntity(Guid tableId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = "pokertable";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the deck.
        /// </summary>
        /// <value>
        /// The deck.
        /// </value>
        public string Deck { get; set; }

        /// <summary>
        /// Gets or sets the burn cards.
        /// </summary>
        /// <value>
        /// The burn cards.
        /// </value>
        public string BurnCards { get; set; }

        /// <summary>
        /// Gets or sets the public cards.
        /// </summary>
        /// <value>
        /// The public cards.
        /// </value>
        public string PublicCards { get; set; }

        /// <summary>
        /// Gets or sets the last updated UTC.
        /// </summary>
        /// <value>
        /// The last updated UTC.
        /// </value>
        public DateTime LastUpdatedUTC { get; set; }
    }
}
