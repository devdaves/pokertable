using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface ISeats
    {
        void Add();

        void Remove(int seatId);

        void RemovePlayerFromSeat(int seatId);

        void RemovePlayerFromSeat(Guid playerId);

        void AssignSeatToPlayer(int seatId, Guid playerId);

        bool DealerExists();
    }

    public class Seats : ISeats, IList<Seat>
    {
        private readonly IList<Seat> seats;

        public Seats()
        {
            this.seats = new List<Seat>();
        }

        public int IndexOf(Seat item)
        {
            return seats.IndexOf(item);
        }

        public void Insert(int index, Seat item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Seat this[int index]
        {
            get
            {
                return this.seats[index];
            }
            set
            {
                this.seats[index] = value;
            }
        }

        public void Add(Seat item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            this.seats.Clear();
        }

        public bool Contains(Seat item)
        {
            return this.seats.Contains(item);
        }

        public void CopyTo(Seat[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.seats.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.seats.IsReadOnly; }
        }

        public bool Remove(Seat item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Seat> GetEnumerator()
        {
            return this.seats.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add()
        {
            this.seats.Add(new Seat(){Id = this.seats.Count + 1});
        }

        public void Remove(int seatId)
        {
            if (this.seats.Any(x => x.Id == seatId))
            {
                var seatTotRemove = this.seats.Single(x => x.Id == seatId);
                this.seats.Remove(seatTotRemove);
                this.seats.Where(x => x.Id > seatId).ToList().ForEach(s => s.Id = s.Id - 1);
            }
        }

        public void RemovePlayerFromSeat(int seatId)
        {
            if (seats.Any(x => x.Id == seatId))
            {
                var seatToClear = seats.Single(x => x.Id == seatId);
                seatToClear.PlayerId = null;
            }
        }

        public void RemovePlayerFromSeat(Guid playerId)
        {
            if (seats.Any(x => x.PlayerId == playerId))
            {
                var seatToClear = seats.Single(x => x.PlayerId == playerId);
                seatToClear.PlayerId = null;
            }
        }

        public void AssignSeatToPlayer(int seatId, Guid playerId)
        {
            seats.Where(x => x.PlayerId == playerId).ToList().ForEach(x => x.PlayerId = null);
            seats.Where(x => x.Id == seatId).ToList().ForEach(x => x.PlayerId = playerId);
        }

        public bool DealerExists()
        {
            return this.seats.Any(x => x.IsDealer);
        }
    }
}
