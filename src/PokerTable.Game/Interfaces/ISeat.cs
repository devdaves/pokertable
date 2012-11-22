using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Seat Object
    /// </summary>
    public interface ISeat
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dealer.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dealer; otherwise, <c>false</c>.
        /// </value>
        bool IsDealer { get; set; }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        Guid? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the deal order.
        /// </summary>
        /// <value>
        /// The deal order.
        /// </value>
        int DealOrder { get; set; }
    }
}
