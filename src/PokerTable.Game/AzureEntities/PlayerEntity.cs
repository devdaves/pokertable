using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    internal class PlayerEntity : TableEntity
    {
        public const string Prefix = "Player-";

        public PlayerEntity()
        {
        }

        public PlayerEntity(Guid tableId, Guid playerId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = string.Format("{0}{1}", Prefix, playerId);
        }

        public string PlayerId { get; set; }
        
        public string Name { get; set; }

        public string Cards { get; set; }

        public int State { get; set; }
    }
}
