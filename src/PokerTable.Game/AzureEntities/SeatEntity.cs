using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PokerTable.Game.AzureEntities
{
    internal class SeatEntity : TableEntity
    {
        public const string Prefix = "Seat-";

        public SeatEntity()
        {
        }

        public SeatEntity(Guid tableId, int seatId)
        {
            this.PartitionKey = tableId.ToString();
            this.RowKey = string.Format("{0}{1}", Prefix, seatId);
        }

        public int SeatId { get; set; }

        public bool IsDealer { get; set; }

        public string PlayerId { get; set; }
    }
}
