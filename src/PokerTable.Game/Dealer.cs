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
    }
}
