using System;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Card Object.  Many of these in a deck
    /// </summary>
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
}
