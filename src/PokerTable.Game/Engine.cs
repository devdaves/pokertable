using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using PokerTable.Game.Data;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface IEngine
    {
        Table Table { get; set; }

        void CreateNewTable(int numberOfSeats, string name);

        void LoadTable(Guid tableId);

        Guid JoinTable(string tablePassword, string playerName);

        void AssignSeatToPlayer(int seatId, Guid playerId);

        void RemovePlayerFromSeat(int seatId);

        void RemovePlayerFromSeat(Guid playerId);

        bool DealerExists();

        void SetDealer(int seatId);

        void NextDealer();

        void ShuffleDeck();

        void DealPlayers();

        void DealFlop();

        void DealTurn();

        void DealRiver();

        void FoldPlayer(Guid playerId);

        void ResetTable();

        void AddSeat(bool saveNow = true);

        void RemoveSeat(int seatId);

        bool AddPlayer(Player player);

        bool RemovePlayer(Guid playerId);
    }

    public class Engine : IEngine
    {
        private const int RandomCodeLength = 5;

        private readonly IRepository repository;
        private readonly IDeckBuilder deckBuilder;
        private readonly IDealer dealer;
        private readonly ISeatManager seatManager;

        public Engine(IRepository repository, IDeckBuilder deckBuilder, IDealer dealer, ISeatManager seatManager)
        {
            this.deckBuilder = deckBuilder;
            this.repository = repository;
            this.dealer = dealer;
            this.seatManager = seatManager;
        }

        public Table Table { get; set; }

        public void CreateNewTable(int numberOfSeats, string name)
        {
            this.Table = new Table(name, this.GetNewTablePassword());
            this.Table.Seats = this.seatManager.AddManySeats(numberOfSeats, this.Table.Id);
            this.BuildDeck();
            this.ShuffleDeck();

            this.repository.SaveTable(this.Table);
            this.repository.DeleteOldTables();
        }

        public void LoadTable(Guid tableId)
        {
            this.Table = this.repository.LoadTable(tableId);
        }

        public Guid JoinTable(string tablePassword, string playerName)
        {
            var tableId = this.repository.GetTableIdByTablePassword(tablePassword);

            if (tableId.HasValue)
            {
                this.LoadTable(tableId.Value);
            }

            if (this.Table == null)
            {
                throw new TableDoesNotExistException();
            }

            Player player = null;

            if (this.Table.Players.Any(x => x.Name == playerName))
            {
                return this.Table.Players.Single(x => x.Name == playerName).Id;
            }
            else
            {
                player = new Player(playerName);
                this.AddPlayer(player);
                return player.Id;
            }
        }

        public void AssignSeatToPlayer(int seatId, Guid playerId)
        {
            this.Table.Seats = this.seatManager.AssignSeatToPlayer(this.Table.Seats, seatId, playerId, this.Table.Id);
        }

        public void RemovePlayerFromSeat(int seatId)
        {
            this.Table.Seats = this.seatManager.RemovePlayerFromSeat(this.Table.Seats, seatId, this.Table.Id);
        }

        public void RemovePlayerFromSeat(Guid playerId)
        {
            this.Table.Seats = this.seatManager.RemovePlayerFromSeat(this.Table.Seats, playerId, this.Table.Id);
        }

        public bool DealerExists()
        {
            return this.Table.Seats.Any(x => x.IsDealer);
        }

        public void SetDealer(int seatId)
        {
            this.dealer.SetDealer(this.Table.Seats, seatId);
            this.seatManager.SaveSeats(this.Table.Seats, this.Table.Id);
        }

        public void NextDealer()
        {
            this.Table.Seats = this.dealer.NextDealer(this.Table.Seats, this.Table.Players);
            this.seatManager.SaveSeats(this.Table.Seats, this.Table.Id);
        }

        public void ShuffleDeck()
        {
            this.dealer.Shuffle(this.Table.Deck);
            this.repository.SaveTable(this.Table);
        }

        public void DealPlayers()
        {
            if (this.DealerExists())
            {
                this.Table.Seats = this.dealer.CalculateDealOrder(this.Table.Seats);
                for (int i = 0; i < 2; i++) //two cards each player
                {
                    foreach (var seat in this.Table.Seats.Where(x => x.PlayerId != null).OrderBy(x => x.DealOrder))
                    {
                        var player = this.Table.Players.Single(x => x.Id == seat.PlayerId);
                        if (player.State == Player.States.Available)
                        {
                            player.Cards.Add(this.DealCard());
                        }
                    }
                }

                this.repository.SaveTable(this.Table);
                this.repository.SavePlayerAll(this.Table.Id, this.Table.Players);
            }
        }

        public void DealFlop()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        public void DealTurn()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        public void DealRiver()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        public void FoldPlayer(Guid playerId)
        {
            var player = this.Table.Players.SingleOrDefault(x => x.Id == playerId);
            if (player != null)
            {
                player.State = Player.States.Folded;
                this.repository.SavePlayer(this.Table.Id, player);
            }
        }

        public void ResetTable()
        {
            this.Table.Burn = new List<Card>();
            this.Table.PublicCards = new List<Card>();
            this.repository.SaveTable(this.Table);

            this.ResetPlayerAll(); // saves the players inside the method

            this.Table.Deck = new Deck();
            this.BuildDeck();
            this.ShuffleDeck(); // saves deck inside method
        }

        public void AddSeat(bool saveNow = true)
        {
            this.Table.Seats = this.seatManager.AddSeat(this.Table.Seats, this.Table.Id);
        }

        public void RemoveSeat(int seatId)
        {
            this.Table.Seats = this.seatManager.RemoveSeat(this.Table.Seats, seatId, this.Table.Id);
        }

        public bool AddPlayer(Player player)
        {
            bool playerAdded = false;
            try
            {
                if (this.Table.Players.All(x => x.Id != player.Id))
                {
                    this.Table.Players.Add(player);
                    this.repository.SavePlayer(this.Table.Id, player);
                    playerAdded = true;
                }
            }
            catch (Exception)
            {
                playerAdded = false;
            }

            return playerAdded;
        }

        public bool RemovePlayer(Guid playerId)
        {
            bool playerRemoved = false;
            try
            {
                if (this.Table.Seats.Any(x => x.PlayerId == playerId))
                {
                    this.Table.Seats.Where(x => x.PlayerId == playerId).ToList().ForEach(x => x.PlayerId = null);
                    this.repository.SaveSeatAll(this.Table.Id, this.Table.Seats);
                }

                if (this.Table.Players.Any(x => x.Id == playerId))
                {
                    var playerToRemove = this.Table.Players.Single(x => x.Id == playerId);
                    this.Table.Players.Remove(playerToRemove);
                    this.repository.RemovePlayer(this.Table.Id, playerToRemove);
                    playerRemoved = true;
                }
            }
            catch (Exception)
            {
                playerRemoved = false;
            }

            return playerRemoved;
        }

        private Card DealCard()
        {
            return this.dealer.Deal(this.Table.Deck);
        }

        private void BuildDeck()
        {
            this.Table.Deck = this.deckBuilder.Build();
        }

        internal void ResetPlayer(Guid playerId, bool saveNow = true)
        {
            var player = this.Table.Players.SingleOrDefault(x => x.Id == playerId);
            if (player != null)
            {
                player.State = Player.States.Available;
                player.Cards = new List<Card>();
                if (saveNow)
                {
                    this.repository.SavePlayer(this.Table.Id, player);
                }
            }
        }

        internal void ResetPlayerAll()
        {
            this.Table.Players.ForEach(x => this.ResetPlayer(x.Id, false));
            this.repository.SavePlayerAll(this.Table.Id, this.Table.Players);
        }

        internal string BuildRandomCode()
        {
            var code = string.Empty;
            var random = new Random((int)DateTime.Now.Ticks);
            while (code.Length < RandomCodeLength)
            {
                code = code + Convert.ToChar(random.Next(97, 122));
            }

            return code;
        }

        internal string GetNewTablePassword()
        {
            var code = this.BuildRandomCode();
            int count = 0;
            while (this.repository.TablePasswordExists(code) || count < 5)
            {
                code = this.BuildRandomCode();
                count++;
            }

            return code;
        }
    }
}
