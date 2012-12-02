using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Data;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    /// <summary>
    /// Engine Object
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// repository field
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine" /> class.
        /// </summary>
        public Engine()
        {
            this.repository = new AzureRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public Engine(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public Table Table { get; set; }

        /// <summary>
        /// Creates the new table.
        /// </summary>
        /// <param name="numberOfSeats">The number of seats.</param>
        /// <param name="name">The name.</param>
        /// <param name="tablePassword">The table password.</param>
        public void CreateNewTable(int numberOfSeats, string name, string tablePassword)
        {
            this.Table = new Table(name, tablePassword);
            this.AddManySeats(numberOfSeats);
            this.BuildDeck();
            this.ShuffleDeck();

            this.repository.SaveTable(this.Table);
        }

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        public void LoadTable(Guid tableId)
        {
            this.Table = this.repository.LoadTable(tableId);
        }

        /// <summary>
        /// Joins the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="tablePassword">The table password.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <returns>returns the GUID of the player</returns>
        public Guid JoinTable(Guid tableId, string tablePassword, string playerName)
        {
            if (this.Table.Id != tableId)
            {
                this.LoadTable(tableId);
            }

            if (this.Table == null || this.Table.Id != tableId)
            {
                throw new TableDoesNotExistException();
            }

            if (this.Table.Password != tablePassword)
            {
                throw new TablePasswordInvalidException();
            }

            var player = new Player(playerName);
            this.AddPlayer(player);

            return player.ID;
        }

        /// <summary>
        /// Assigns the seat to player.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns true if successful false if failed
        /// </returns>
        public bool AssignSeatToPlayer(int seatId, Guid playerId)
        {
            try
            {
                this.RemovePlayerFromSeat(playerId);
                var seat = this.Table.Seats.Single(x => x.Id == seatId && x.PlayerId.HasValue == false);
                seat.PlayerId = playerId;
                this.repository.SaveSeat(this.Table.Id, seat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the player from seat.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>returns true if successful false if failed</returns>
        public bool RemovePlayerFromSeat(int seatId)
        {
            try
            {
                var seat = this.Table.Seats.Single(x => x.Id == seatId);
                if (seat.PlayerId != null)
                {
                    seat.PlayerId = null;
                    this.repository.SaveSeat(this.Table.Id, seat);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Removes the player from seat.
        /// </summary>
        /// <param name="playerId">The player ID</param>
        /// <returns>
        /// returns true if successful false if failed
        /// </returns>
        public bool RemovePlayerFromSeat(Guid playerId)
        {
            try
            {
                var seat = this.Table.Seats.SingleOrDefault(x => x.PlayerId == playerId);
                if (seat != null)
                {
                    seat.PlayerId = null;
                    this.repository.SaveSeat(this.Table.Id, seat);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Dealers the exists.
        /// </summary>
        /// <returns>
        /// returns true if dealer exists false if not
        /// </returns>
        public bool DealerExists()
        {
            return this.Table.Seats.Any(x => x.IsDealer == true);
        }

        /// <summary>
        /// Sets the dealer.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        public void SetDealer(int seatId)
        {
            this.Table.Seats.ForEach(s => s.IsDealer = s.Id == seatId);
            this.repository.SaveSeatAll(this.Table.Id, this.Table.Seats);
        }

        /// <summary>
        /// Set dealer to next player
        /// </summary>
        public void NextDealer()
        {
            if (this.Table.Seats.Any() && this.Table.Seats.Any(x => x.PlayerId != null))
            {
                this.CalculateDealOrder();
                foreach (var seat in this.Table.Seats.OrderBy(x => x.DealOrder))
                {
                    var player = this.Table.Players.SingleOrDefault(x => x.ID == seat.PlayerId);
                    if (player != null && player.State == Player.States.Available)
                    {
                        this.SetDealer(seat.Id);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void ShuffleDeck()
        {
            this.Table.Deck.Cards = this.Table.Deck.Cards.OrderBy(x => Guid.NewGuid()).ToList();
            this.repository.SaveTable(this.Table);
        }

        /// <summary>
        /// Deals the players.
        /// </summary>
        public void DealPlayers()
        {
            if (this.DealerExists())
            {
                this.CalculateDealOrder();
                for (int i = 0; i < 2; i++)
                {
                    foreach (var seat in this.Table.Seats.Where(x => x.PlayerId != null).OrderBy(x => x.DealOrder))
                    {
                        var player = this.Table.Players.Single(x => x.ID == seat.PlayerId);
                        if (player.State == Player.States.Available)
                        {
                            player.Cards.Add(this.DealCard());
                        }
                    }
                }

                this.repository.SavePlayerAll(this.Table.Id, this.Table.Players);
            }
        }

        /// <summary>
        /// Deals the flop.
        /// </summary>
        public void DealFlop()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        /// <summary>
        /// Deals the turn.
        /// </summary>
        public void DealTurn()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        /// <summary>
        /// Deals the river.
        /// </summary>
        public void DealRiver()
        {
            this.Table.Burn.Add(this.DealCard());
            this.Table.PublicCards.Add(this.DealCard());
            this.repository.SaveTable(this.Table);
        }

        /// <summary>
        /// Folds the specified player id.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        public void FoldPlayer(Guid playerId)
        {
            var player = this.Table.Players.SingleOrDefault(x => x.ID == playerId);
            if (player != null)
            {
                player.State = Player.States.Folded;
                this.repository.SavePlayer(this.Table.Id, player);
            }
        }

        /// <summary>
        /// Resets the deck, burn and public card lists, resets each player
        /// </summary>
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

        /// <summary>
        /// Adds the seat to the table
        /// </summary>
        /// <param name="saveNow">if set to <c>true</c> [save now].</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        public bool AddSeat(bool saveNow = true)
        {
            try
            {
                int id = this.Table.Seats.Count() + 1;
                var seat = new Seat() { Id = id, DealOrder = 0, IsDealer = false, PlayerId = null };
                this.Table.Seats.Add(seat);

                if (saveNow)
                {
                    this.repository.SaveSeat(this.Table.Id, seat);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the seat from the table
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        public bool RemoveSeat(int seatId)
        {
            if (!this.Table.Seats.Any(x => x.Id == seatId))
            {
                return false;
            }

            var seatToRemove = this.Table.Seats.Single(x => x.Id == seatId);
            this.Table.Seats.Remove(seatToRemove);
            this.Table.Seats.Where(x => x.Id > seatId).ToList().ForEach(s => s.Id = s.Id - 1);
            
            this.repository.RemoveSeat(this.Table.Id, seatToRemove);
            this.repository.SaveSeatAll(this.Table.Id, this.Table.Seats);

            return true;
        }

        /// <summary>
        /// Adds the player to the table
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>
        /// returns true if player was added false if not
        /// </returns>
        public bool AddPlayer(Player player)
        {
            try
            {
                if (!this.Table.Players.Any(x => x.ID == player.ID))
                {
                    this.Table.Players.Add(player);
                    this.repository.SavePlayer(this.Table.Id, player);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns true if player was removed false if not
        /// </returns>
        public bool RemovePlayer(Guid playerId)
        {
            try
            {
                if (this.Table.Seats.Any(x => x.PlayerId == playerId))
                {
                    this.Table.Seats.Where(x => x.PlayerId == playerId).ToList().ForEach(x => x.PlayerId = null);
                    this.repository.SaveSeatAll(this.Table.Id, this.Table.Seats);
                }

                if (this.Table.Players.Any(x => x.ID == playerId))
                {
                    var playerToRemove = this.Table.Players.Single(x => x.ID == playerId);
                    this.Table.Players.Remove(playerToRemove);
                    this.repository.RemovePlayer(this.Table.Id, playerToRemove);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Deals the card.
        /// </summary>
        /// <returns>returns the card dealt from the deck</returns>
        internal Card DealCard()
        {
            var c = this.Table.Deck.Cards.FirstOrDefault(x => x.State == Card.States.Available);
            if (c == null)
            {
                throw new NoAvailableCardsException();
            }

            c.State = Card.States.Dealt;
            return c;
        }

        /// <summary>
        /// Builds the deck.
        /// </summary>
        internal void BuildDeck()
        {
            this.Table.Deck.Cards = new List<Card>();

            for (int s = 1; s < 5; s++)
            {
                for (int v = 1; v < 14; v++)
                {
                    this.Table.Deck.Cards.Add(new Card() { Suite = (Card.Suites)s, Color = (Card.Colors)(s % 2), State = Card.States.Available, Value = v });
                }
            }
        }

        /// <summary>
        /// Resets the players state and cards
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <param name="saveNow">if set to <c>true</c> [save now].</param>
        internal void ResetPlayer(Guid playerId, bool saveNow = true)
        {
            var player = this.Table.Players.SingleOrDefault(x => x.ID == playerId);
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

        /// <summary>
        /// Resets the player all.
        /// </summary>
        internal void ResetPlayerAll()
        {
            this.Table.Players.ForEach(x => this.ResetPlayer(x.ID, false));
            this.repository.SavePlayerAll(this.Table.Id, this.Table.Players);
        }

        /// <summary>
        /// Add many seats to the table
        /// </summary>
        /// <param name="howManySeats">how many seats to add</param>
        private void AddManySeats(int howManySeats)
        {
            this.Table.Seats = new List<Seat>();

            for (int i = 0; i < howManySeats; i++)
            {
                this.AddSeat(false);
            }

            this.repository.SaveSeatAll(this.Table.Id, this.Table.Seats);
        }

        /// <summary>
        /// Calculates the deal order.
        /// </summary>
        private void CalculateDealOrder()
        {
            int dealOrder = 200;
            foreach (var seat in this.Table.Seats)
            {
                if (seat.IsDealer)
                {
                    dealOrder = 100;
                    seat.DealOrder = 300;
                }
                else
                {
                    seat.DealOrder = dealOrder;
                    dealOrder++;
                }
            }
        }
    }
}
