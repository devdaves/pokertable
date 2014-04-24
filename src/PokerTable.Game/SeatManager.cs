using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface ISeatManager
    {
        List<Seat> AddSeat(List<Seat> seats, Guid tableId);

        List<Seat> AddManySeats(int numberOfSeatsToAdd, Guid tableId);

        List<Seat> RemoveSeat(List<Seat> seats, int seatId, Guid tableId);

        List<Seat> RemovePlayerFromSeat(List<Seat> seats, int seatId, Guid tableId);

        List<Seat> RemovePlayerFromSeat(List<Seat> seats, Guid playerId, Guid tableId);

        List<Seat> AssignSeatToPlayer(List<Seat> seats, int seatId, Guid playerId, Guid tableId);

        List<Seat> SaveSeats(List<Seat> seats, Guid tableId);
    }

    public class SeatManager : ISeatManager
    {
        private readonly IRepository repository;

        public SeatManager(IRepository repository)
        {
            this.repository = repository;
        }

        public List<Seat> AddSeat(List<Seat> seats, Guid tableId)
        {
            var seat = this.BuildNextSeat(seats);
            seats.Add(seat);

            this.repository.SaveSeat(tableId, seat);

            return seats;
        }

        public List<Seat> AddManySeats(int numberOfSeatsToAdd, Guid tableId)
        {
            var seats = new List<Seat>();

            for (int i = 0; i < numberOfSeatsToAdd; i++)
            {
                seats.Add(this.BuildNextSeat(seats));
            }

            this.repository.SaveSeatAll(tableId, seats);

            return seats;
        }

        private Seat BuildNextSeat(List<Seat> seats)
        {
            var seatId = seats.Count + 1;
            return new Seat() { Id = seatId };
        }

        public List<Seat> RemoveSeat(List<Seat> seats, int seatId, Guid tableId)
        {
            if (seats.Any(x => x.Id == seatId))
            {
                var seatToRemove = seats.Single(x => x.Id == seatId);

                seats.Remove(seatToRemove);
                seats.Where(x => x.Id > seatId).ToList().ForEach(s => s.Id = s.Id - 1);

                this.repository.RemoveSeat(tableId, seatToRemove);
                this.repository.SaveSeatAll(tableId, seats);
            }

            return seats;
        }

        public List<Seat> RemovePlayerFromSeat(List<Seat> seats, int seatId, Guid tableId)
        {
            if (seats.Any(x => x.Id == seatId))
            {
                var seatToClear = seats.Single(x => x.Id == seatId);
                seatToClear.PlayerId = null;

                this.repository.SaveSeat(tableId, seatToClear);
            }

            return seats;
        }

        public List<Seat> RemovePlayerFromSeat(List<Seat> seats, Guid playerId, Guid tableId)
        {
            if (seats.Any(x => x.PlayerId == playerId))
            {
                var seatToClear = seats.Single(x => x.PlayerId == playerId);
                seatToClear.PlayerId = null;

                this.repository.SaveSeat(tableId, seatToClear);
            }

            return seats;
        }

        public List<Seat> AssignSeatToPlayer(List<Seat> seats, int seatId, Guid playerId, Guid tableId)
        {
            seats.Where(x => x.PlayerId == playerId).ToList().ForEach(x => x.PlayerId = null);
            seats.Where(x => x.Id == seatId).ToList().ForEach(x => x.PlayerId = playerId);
            this.repository.SaveSeatAll(tableId, seats);
            return seats;
        }

        public List<Seat> SaveSeats(List<Seat> seats, Guid tableId)
        {
            this.repository.SaveSeatAll(tableId, seats);
            return seats;
        }
    }
}
