using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface IDealer
    {
        Deck Shuffle(Deck deck);

        Card Deal(Deck deck);

        List<Seat> CalculateDealOrder(List<Seat> seats);

        List<Seat> NextDealer(List<Seat> seats, List<Player> players);

        List<Seat> SetDealer(List<Seat> seats, int seatId);
    }

    public class Dealer : IDealer
    {
        public Deck Shuffle(Deck deck)
        {
            deck.Cards = deck.Cards.OrderBy(x => Guid.NewGuid()).ToList();
            return deck;
        }

        public Card Deal(Deck deck)
        {
            var c = deck.Cards.FirstOrDefault(x => x.State == Card.States.Available);
            if (c == null)
            {
                throw new NoAvailableCardsException();
            }

            c.State = Card.States.Dealt;
            return c;
        }

        public List<Seat> CalculateDealOrder(List<Seat> seats)
        {
            int dealOrder = 200;
            foreach (var seat in seats)
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

            return seats;
        }

        public List<Seat> NextDealer(List<Seat> seats, List<Player> players)
        {
            seats = this.CalculateDealOrder(seats);

            foreach (var seat in seats.OrderBy(x => x.DealOrder))
            {
                var player = players.SingleOrDefault(x => x.Id == seat.PlayerId);
                if (seat.PlayerId.HasValue && player != null && player.State == Player.States.Available)
                {
                    seat.IsDealer = true;
                    break;
                }    
            }

            return seats;
        }

        public List<Seat> SetDealer(List<Seat> seats, int seatId)
        {
            seats.ForEach(s => s.IsDealer = s.Id == seatId);
            return seats;
        }
    }
}
