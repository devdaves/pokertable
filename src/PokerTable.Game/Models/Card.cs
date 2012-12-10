using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Models
{
    /// <summary>
    /// Card Object.  Many of these in a deck
    /// </summary>
    [Serializable]
    public class Card : ICard
    {
        /// <summary>
        /// Suites
        /// </summary>
        public enum Suites
        {
            /// <summary>
            /// Clubs
            /// </summary>
            Clubs = 1,

            /// <summary>
            /// Hearts
            /// </summary>
            Hearts = 2,

            /// <summary>
            /// Spades
            /// </summary>
            Spades = 3,

            /// <summary>
            /// Diamonds
            /// </summary>
            Diamonds = 4
        }

        /// <summary>
        /// Colors
        /// </summary>
        public enum Colors
        {
            /// <summary>
            /// Red, used by Hearts and Diamonds
            /// </summary>
            Red = 0,

            /// <summary>
            /// Black, used by Clubs and Spades
            /// </summary>
            Black = 1
        }

        /// <summary>
        /// States
        /// </summary>
        public enum States
        {
            /// <summary>
            /// Available to be dealt
            /// </summary>
            Available = 1,

            /// <summary>
            /// Already dealt
            /// </summary>
            Dealt = 2
        }

        /// <summary>
        /// Gets or sets the suite.
        /// </summary>
        /// <value>
        /// The suite.
        /// </value>
        public Suites Suite { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Colors Color { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public States State { get; set; }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue
        {
            get
            {
                return this.ValueToString();
            }
        }

        /// <summary>
        /// Gets the HTML CSS class.
        /// </summary>
        /// <value>
        /// The HTML CSS class.
        /// </value>
        public string HtmlCssClass
        {
            get
            {
                return this.SuiteToHtmlClass();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; set; }

        /// <summary>
        /// Determine the name of the card.  For example 2H = 2 of Hearts
        /// </summary>
        /// <returns>the name of the card</returns>
        public string Name()
        {
            return string.Format("{0}:{1}", this.ValueToString(), this.SuiteToString());
        }

        /// <summary>
        /// Turn the card value to its string counterpart.  Especially used for the 
        /// Jack, Queen, King and Ace
        /// </summary>
        /// <returns>the value as a string</returns>
        private string ValueToString()
        {
            if (this.Value < 1 || this.Value > 13)
            {
                throw new ArgumentException("Card value must be between 1 and 13");
            }

            switch (this.Value)
            {
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                case 1:
                    return "A";
                default:
                    return this.Value.ToString();
            }
        }

        /// <summary>
        /// Based on the Suite enumeration return the string designation.
        /// </summary>
        /// <returns>suite as a string</returns>
        private string SuiteToString()
        {
            switch (this.Suite)
            {
                case Suites.Clubs:
                    return "C";
                case Suites.Spades:
                    return "S";
                case Suites.Hearts:
                    return "H";
                case Suites.Diamonds:
                    return "D";
                default:
                    throw new ArgumentException("Suite is required");
            }
        }

        /// <summary>
        /// Suites to HTML CSS class.
        /// </summary>
        /// <returns>returns the HTML CSS class for the suite</returns>
        private string SuiteToHtmlClass()
        {
            switch (this.Suite)
            {
                case Suites.Clubs:
                    return "clubs";
                case Suites.Spades:
                    return "spades";
                case Suites.Hearts:
                    return "hearts";
                case Suites.Diamonds:
                    return "diamonds";
                default:
                    throw new ArgumentException("Suite is required");
            }
        }
    }
}
