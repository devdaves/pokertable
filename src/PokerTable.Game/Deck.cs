using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game
{
    /// <summary>
    /// Deck Object
    /// </summary>
    public class Deck : IDeck
    {
        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        public List<ICard> Cards { get; set; }
    }
}
