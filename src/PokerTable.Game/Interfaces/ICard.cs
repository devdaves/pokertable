using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Models;

namespace PokerTable.Game.Interfaces
{
    public interface ICard
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Card.Colors Color { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        Card.States State { get; set; }

        /// <summary>
        /// Gets or sets the suite.
        /// </summary>
        /// <value>
        /// The suite.
        /// </value>
        Card.Suites Suite { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        int Value { get; set; }

        /// <summary>
        /// Determine the name of the card.  For example 2H = 2 of Hearts
        /// </summary>
        /// <returns>the name of the card</returns>
        string Name();
}
