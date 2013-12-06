using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    internal class PokerTableEntity : TableEntity
    {
        public const string Prefix = "PokerTable";

        public PokerTableEntity()
        {
        }

        public PokerTableEntity(Guid tableId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = Prefix;
        }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Deck { get; set; }

        public string BurnCards { get; set; }

        public string PublicCards { get; set; }

        public DateTime LastUpdatedUTC { get; set; }
    }
}
