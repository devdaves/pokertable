using System;
using System.Globalization;

namespace PokerTable.Game.Models
{
    [Serializable]
    public class Card
    {
        public enum Suites
        {
            Clubs = 1,
            Hearts = 2,
            Spades = 3,
            Diamonds = 4
        }

        public enum Colors
        {
            Red = 0,
            Black = 1
        }

        public enum States
        {
            Available = 1,
            Dealt = 2
        }

        public Suites Suite { get; set; }

        public Colors Color { get; set; }

        public States State { get; set; }

        public string StringValue
        {
            get
            {
                return this.ValueToString();
            }
        }

        public string HtmlCssClass
        {
            get
            {
                return this.SuiteToHtmlClass();
            }
        }

        public int Value { get; set; }

        public string Name()
        {
            return string.Format("{0}:{1}", this.ValueToString(), this.SuiteToString());
        }

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
                    return this.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

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
