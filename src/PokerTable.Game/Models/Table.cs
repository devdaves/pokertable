using System;
using System.Collections.Generic;
using System.Linq;
using PokerTable.Game.Extensions;

namespace PokerTable.Game.Models
{
    public class Table
    {
        public Table()
        {
        }

        public Table(string tableName, string tablePassword)
        {
            this.Id = Guid.NewGuid();
            this.Name = tableName;
            this.Password = tablePassword;
            this.Deck = new Deck();
            this.Seats = new List<Seat>();
            this.Players = new List<Player>();
            this.Burn = new List<Card>();
            this.PublicCards = new List<Card>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Deck Deck { get; set; }

        public List<Seat> Seats { get; set; }

        public List<Player> Players { get; set; }

        public List<Card> Burn { get; set; }

        public List<Card> PublicCards { get; set; }

        public bool ResetTableAvailable
        {
            get { return this.CalculateResetTableAvailable(); }
        }

        public bool DealPlayersAvailable
        {
            get { return this.CalculateDealPlayersAvailable(); }
        }

        public bool DealFlopAvailable
        {
            get { return this.CalculateDealFlopAvailable(); }
        }

        public bool DealTurnAvailable
        {
            get { return this.CalculateDealTurnAvailable(); }
        }

        public bool DealRiverAvailable
        {
            get { return this.CalculateDealRiverAvailable(); }
        }

        private bool CalculateResetTableAvailable()
        {
            return true;
        }

        private bool CalculateDealPlayersAvailable()
        {
            return !this.Players.Any(x => x.Cards.Any());
        }

        private bool CalculateDealFlopAvailable()
        {
            return !this.PublicCards.Any() && this.DealPlayersAvailable == false;
        }

        private bool CalculateDealTurnAvailable()
        {
            return this.PublicCards.Count() == 3;
        }

        private bool CalculateDealRiverAvailable()
        {
            return this.PublicCards.Count() == 4;
        }
    }
}
