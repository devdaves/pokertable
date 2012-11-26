using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Models
{
    /// <summary>
    /// Deck Object
    /// </summary>
    [Serializable]
    public class Deck : IDeck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Deck" /> class.
        /// </summary>
        public Deck()
        {
            this.Cards = new List<Card>();
        }

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        public List<Card> Cards { get; set; }
    }
}
