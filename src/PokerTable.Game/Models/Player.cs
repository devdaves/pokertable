using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Models
{
    /// <summary>
    /// Player Object
    /// </summary>
    [Serializable]
    public class Player : IPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        public Player(string playerName)
        {
            this.ID = Guid.NewGuid();
            this.Name = playerName;
            this.Cards = new List<ICard>();
            this.State = States.Available;
        }

        /// <summary>
        /// States the player can be in
        /// </summary>
        public enum States
        {
            /// <summary>
            /// Available, player can play
            /// </summary>
            Available = 1,

            /// <summary>
            /// Folded, can not play the rest of the hand
            /// </summary>
            Folded = 2,

            /// <summary>
            /// Sitting out, skip this player during the deal
            /// </summary>
            SittingOut = 3
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        public List<ICard> Cards { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public States State { get; set; }
    }
}
