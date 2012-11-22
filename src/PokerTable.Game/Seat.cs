using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game
{
    /// <summary>
    /// Seat Object
    /// </summary>
    public class Seat : ISeat
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

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
        public Guid? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the deal order.
        /// </summary>
        /// <value>
        /// The deal order.
        /// </value>
        public int DealOrder { get; set; }
    }
}
