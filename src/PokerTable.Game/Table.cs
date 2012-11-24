using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game
{
    /// <summary>
    /// Table Object
    /// </summary>
    public class Table : ITable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table" /> class.
        /// </summary>
        /// <param name="numberOfSeats">The number of seats.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="tablePassword">The table password.</param>
        public Table(int numberOfSeats, string tableName, string tablePassword)
        {
            this.Id = Guid.NewGuid();
            this.Name = tableName;
            this.Password = tablePassword;
            this.AddManySeats(numberOfSeats);
            this.Players = new List<IPlayer>();
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table" /> class.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        public Table(Guid tableId)
        {
            // TODO: load from persistant storage.
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the deck.
        /// </summary>
        /// <value>
        /// The deck.
        /// </value>
        public IDeck Deck { get; set; }

        /// <summary>
        /// Gets or sets the seats.
        /// </summary>
        /// <value>
        /// The seats.
        /// </value>
        public List<ISeat> Seats { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public List<IPlayer> Players { get; set; }

        /// <summary>
        /// Gets or sets the burn.
        /// </summary>
        /// <value>
        /// The burn.
        /// </value>
        public List<ICard> Burn { get; set; }

        /// <summary>
        /// Gets or sets the public cards.
        /// </summary>
        /// <value>
        /// The public cards.
        /// </value>
        public List<ICard> PublicCards { get; set; }

        /// <summary>
        /// Deals the flop.
        /// </summary>
        public void DealFlop()
        {
            this.Burn.Add(this.Deck.DealCard());
            this.PublicCards.Add(this.Deck.DealCard());
            this.PublicCards.Add(this.Deck.DealCard());
            this.PublicCards.Add(this.Deck.DealCard());
        }

        /// <summary>
        /// Deals the turn.
        /// </summary>
        public void DealTurn()
        {
            this.Burn.Add(this.Deck.DealCard());
            this.PublicCards.Add(this.Deck.DealCard());        
        }

        /// <summary>
        /// Deals the river.
        /// </summary>
        public void DealRiver()
        {
            this.Burn.Add(this.Deck.DealCard());
            this.PublicCards.Add(this.Deck.DealCard());          
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
                    foreach (var seat in this.Seats.Where(x => x.PlayerId != null).OrderBy(x => x.DealOrder))
                    {
                        var player = this.Players.Single(x => x.ID == seat.PlayerId);
                        if (player.State == Player.States.Available)
                        {
                            player.Cards.Add(this.Deck.DealCard());
                        }
                    }
                }

                // TODO: persist deal player changes - seats.save, players.save 
            }
        }

        /// <summary>
        /// Set dealer to next player
        /// </summary>
        public void NextDealer()
        {
            if (this.Seats.Any() && this.Seats.Any(x => x.PlayerId != null))
            {
                this.CalculateDealOrder();
                foreach (var seat in this.Seats.OrderBy(x => x.DealOrder))
                {
                    var player = this.Players.SingleOrDefault(x => x.ID == seat.PlayerId);
                    if (player != null && player.State == Player.States.Available)
                    {
                        this.SetDealer(seat.Id);
                        return;
                    }
                }
            }
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
                var seat = this.Seats.Single(x => x.Id == seatId && x.PlayerId.HasValue == false);
                seat.PlayerId = playerId;

                // TODO: persist assign seat to player changes - seats.save
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
                var seat = this.Seats.Single(x => x.Id == seatId);
                if (seat.PlayerId != null)
                {
                    seat.PlayerId = null;
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
                var seat = this.Seats.SingleOrDefault(x => x.PlayerId == playerId);
                if (seat != null)
                {
                    seat.PlayerId = null;
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
            return this.Seats.Any(x => x.IsDealer == true);
        }

        /// <summary>
        /// Sets the dealer.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        public void SetDealer(int seatId)
        {
            this.Seats.ForEach(s => s.IsDealer = s.Id == seatId);
            
            // TODO: persist dealer change - seats.save
        }

        /// <summary>
        /// Adds the seat to the table
        /// </summary>
        /// <param name="persistChanges">if set to <c>true</c> [persist changes].</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        public bool AddSeat(bool persistChanges = true)
        {
            try
            {
                int id = this.Seats.Count() + 1;
                this.Seats.Add(new Seat() { Id = id, DealOrder = 0, IsDealer = false, PlayerId = null });

                // TODO: persist add seat changes only if required - seats.save             
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
            if (!this.Seats.Any(x => x.Id == seatId))
            {
                return false;
            }

            var seatToRemove = this.Seats.Single(x => x.Id == seatId);
            this.Seats.Remove(seatToRemove);
            this.Seats.Where(x => x.Id > seatId).ToList().ForEach(s => s.Id = s.Id - 1);

            // TODO: persist remove seat changes - seats.save
            return true;
        }

        /// <summary>
        /// Adds the player to the table
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>
        /// returns true if player was added false if not
        /// </returns>
        public bool AddPlayer(IPlayer player)
        {
            try
            {
                if (!this.Players.Any(x => x.ID == player.ID))
                {
                    this.Players.Add(player);

                    // TODO: persist add player - players.save
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
                if (this.Seats.Any(x => x.PlayerId == playerId))
                {
                    this.Seats.Where(x => x.PlayerId == playerId).ToList().ForEach(x => x.PlayerId = null);

                    // TODO: remove player, persist seat changes - seats.save
                }

                if (this.Players.Any(x => x.ID == playerId))
                {
                    var playerToRemove = this.Players.Single(x => x.ID == playerId);
                    this.Players.Remove(playerToRemove);

                    // TODO: remove player, persist player changes - players.save
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Resets the deck, burn and public card lists, resets each player
        /// </summary>
        public void Reset()
        {
            this.Deck = new Deck();
            this.Burn = new List<ICard>();
            this.PublicCards = new List<ICard>();
            this.Players.ForEach(x => x.Reset());   
        }

        /// <summary>
        /// Add many seats to the table
        /// </summary>
        /// <param name="howManySeats">how many seats to add</param>
        private void AddManySeats(int howManySeats)
        {
            this.Seats = new List<ISeat>();

            for (int i = 0; i < howManySeats; i++)
            {
                this.AddSeat(false);
            }

            // TODO: persist add many seats - seats.save
        }

        /// <summary>
        /// Calculates the deal order.
        /// </summary>
        private void CalculateDealOrder()
        {
            int dealOrder = 200;
            foreach (var seat in this.Seats)
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
