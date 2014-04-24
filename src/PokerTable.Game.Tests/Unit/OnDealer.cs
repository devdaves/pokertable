using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class OnDealer
    {
        [TestClass]
        public class WhenShuffle
        {
            private IDealer dealer;
            private IDeckBuilder deckBuilder;

            [TestInitialize]
            public void Setup()
            {
                this.dealer = new Dealer();
                this.deckBuilder = new DeckBuilder();
            }

            [TestMethod]
            public void ShouldReturnDeck()
            {
                
                var result = this.dealer.Shuffle(this.deckBuilder.Build());

                Assert.IsInstanceOfType(result, typeof(Deck));
            }

            [TestMethod]
            public void ShouldReorderCards()
            {
                var deck1 = deckBuilder.Build();
                var deck1String = Utils.DeckCardsToString(deck1);
                var deck2 = this.dealer.Shuffle(deck1);
                var deck2String = Utils.DeckCardsToString(deck2);

                Assert.AreNotEqual(deck1String, deck2String, "After shuffling the deck should be in a different order");
            }

            [TestMethod]
            public void After500IterationsNoDuplicateDecks()
            {
                var deck = this.deckBuilder.Build();
                var shuffleResults = new List<string>();
                for (var i = 0; i < 500; i++)
                {
                    deck = this.dealer.Shuffle(deck);
                    shuffleResults.Add(Utils.DeckCardsToString(deck));
                }

                var duplicatesGroup = shuffleResults.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);

                Assert.AreEqual(0, duplicatesGroup.Count());
            }
        }

        [TestClass]
        public class WhenDeal
        {
            private IDealer dealer;
            private IDeckBuilder deckBuilder;

            [TestInitialize]
            public void Setup()
            {
                this.dealer = new Dealer();
                this.deckBuilder = new DeckBuilder();
            }

            [TestMethod]
            public void ShouldReturnCard()
            {
                var deck = this.deckBuilder.Build();

                var result = this.dealer.Deal(deck);

                Assert.IsInstanceOfType(result, typeof(Card));
            }

            [TestMethod]
            public void ShouldReturnFirstCardIfItHasntBeenDealt()
            {
                var deck = this.deckBuilder.Build();
                deck.Cards[0].State = Card.States.Available;
                var expectedCard = deck.Cards[0];

                var result = this.dealer.Deal(deck);

                Assert.AreEqual(expectedCard.Name(), result.Name());
            }

            [TestMethod]
            public void ShouldMarkReturnedCardAsDealt()
            {
                var expectedState = Card.States.Dealt;
                var deck = this.deckBuilder.Build();
                deck.Cards[0].State = Card.States.Available;

                var result = this.dealer.Deal(deck);

                Assert.AreEqual(expectedState, result.State);
            }

            [TestMethod]
            public void ShouldReturnSecondCardIfFirstCardHasBeenDealt()
            {
                var deck = this.deckBuilder.Build();
                deck.Cards[0].State = Card.States.Dealt;
                deck.Cards[1].State = Card.States.Available;
                var expectedCard = deck.Cards[1];

                var result = this.dealer.Deal(deck);

                Assert.AreEqual(expectedCard.Name(), result.Name());
            }

            [TestMethod]
            [ExpectedException(typeof(NoAvailableCardsException))]
            public void ThrowsNotAvailableCardExcpetionIfAllCardsDealt()
            {
                var deck = this.deckBuilder.Build();
                deck.Cards.ForEach(x => x.State = Card.States.Dealt);

                var result = this.dealer.Deal(deck);
            }
        }

        [TestClass]
        public class WhenCalculateDealOrder_ThreeSeats
        {
            private IDealer dealer;

            [TestInitialize]
            public void Setup()
            {
                this.dealer = new Dealer();
            }

            [TestMethod]
            public void ShouldReturnListOfSeats()
            {
                var seats = this.dealer.CalculateDealOrder(this.BuildSeatList(3));

                Assert.IsInstanceOfType(seats, typeof(List<Seat>));
            }

            [TestMethod]
            public void CorrectOrderNoDealer()
            {
                var seats = this.BuildSeatList(3);
                seats = this.dealer.CalculateDealOrder(seats).OrderBy(x => x.DealOrder).ToList();

                Assert.AreEqual(1, seats[0].Id);
                Assert.AreEqual(2, seats[1].Id);
                Assert.AreEqual(3, seats[2].Id);
            }

            [TestMethod]
            public void CorrectOrderSeat1IsDealer()
            {
                var seats = this.BuildSeatList(3);
                seats[0].IsDealer = true;
                seats = this.dealer.CalculateDealOrder(seats).OrderBy(x => x.DealOrder).ToList();

                Assert.AreEqual(2, seats[0].Id);
                Assert.AreEqual(3, seats[1].Id);
                Assert.AreEqual(1, seats[2].Id);
            }

            [TestMethod]
            public void CorrectOrderSeat2IsDealer()
            {
                var seats = this.BuildSeatList(3);
                seats[1].IsDealer = true;
                seats = this.dealer.CalculateDealOrder(seats).OrderBy(x => x.DealOrder).ToList();

                Assert.AreEqual(3, seats[0].Id);
                Assert.AreEqual(1, seats[1].Id);
                Assert.AreEqual(2, seats[2].Id);
            }

            [TestMethod]
            public void CorrectOrderSeat3OIsDealer()
            {
                var seats = this.BuildSeatList(3);
                seats[2].IsDealer = true;
                seats = this.dealer.CalculateDealOrder(seats).OrderBy(x => x.DealOrder).ToList();

                Assert.AreEqual(1, seats[0].Id);
                Assert.AreEqual(2, seats[1].Id);
                Assert.AreEqual(3, seats[2].Id);
            }

            private List<Seat> BuildSeatList(int numberOfSeats)
            {
                var seats = new List<Seat>();

                for (int i = 1; i <= numberOfSeats; i++)
                {
                    seats.Add(new Seat(){Id = i, IsDealer = false, PlayerId = Guid.NewGuid()});
                }

                return seats;
            }
        }

        [TestClass]
        public class WhenNextDealer
        {
            private IDealer dealer;
            private ISeatManager seatManager;
            private Mock<IRepository> repositoryMock;

            [TestInitialize]
            public void Setup()
            {
                this.dealer = new Dealer();
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ShouldReturnSeatList()
            {
                var result = this.dealer.NextDealer(new List<Seat>(), new List<Player>());

                Assert.IsInstanceOfType(result, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldOrderSeats()
            {
                var seats = this.BuildSeats(2);
                var players = this.BuildPlayers(2);
                var results = this.dealer.NextDealer(seats, players);

                Assert.AreNotEqual(results[0].DealOrder, results[1].DealOrder);
            }

            [TestMethod]
            public void ShouldSetSeat1AsDealerIfNoOneIsDealerAndSeat1HasPlayer()
            {
                var seats = this.BuildSeats(2);
                var players = this.BuildPlayers(2);

                seats[0].PlayerId = players[0].Id;

                var results = this.dealer.NextDealer(seats, players);

                Assert.AreEqual(true, results[0].IsDealer);
            }

            [TestMethod]
            public void ShouldNotSetSeat1AsDealerIfHasPlayerButPlayerUnavailable()
            {
                var seats = this.BuildSeats(2);
                var players = this.BuildPlayers(2);

                seats[0].PlayerId = players[0].Id;
                players[0].State = Player.States.SittingOut;

                var results = this.dealer.NextDealer(seats, players);

                Assert.AreEqual(false, results[0].IsDealer);
            }

            [TestMethod]
            public void ShouldSetSeat2AsDealerIfSeat1IsDealerAndPlayersInBothSeats()
            {
                var seats = this.BuildSeats(2);
                var players = this.BuildPlayers(2);

                seats[0].PlayerId = players[0].Id;
                seats[0].IsDealer = true;
                seats[1].PlayerId = players[1].Id;

                var results = this.dealer.NextDealer(seats, players);

                Assert.AreEqual(true, seats[1].IsDealer);
            }

            [TestMethod]
            public void ShouldSetSeat1AsDealerIfSeat2IsDealerAndPlayersInBothSeats()
            {
                var seats = this.BuildSeats(2);
                var players = this.BuildPlayers(2);

                seats[0].PlayerId = players[0].Id;
                seats[1].PlayerId = players[1].Id;
                seats[1].IsDealer = true;

                var results = this.dealer.NextDealer(seats, players);

                Assert.AreEqual(true, seats[0].IsDealer);
            }

            private List<Seat> BuildSeats(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }

            private List<Player> BuildPlayers(int howMany)
            {
                var players = new List<Player>();

                for (int i = 0; i < howMany; i++)
                {
                    players.Add(new Player("Player"){ Id = Guid.NewGuid(), State = Player.States.Available});
                }

                return players;
            }
        }

        [TestClass]
        public class WhenSetDealer
        {
            private IDealer dealer;
            private ISeatManager seatManager;
            private Mock<IRepository> repositoryMock;

            [TestInitialize]
            public void Setup()
            {
                this.dealer = new Dealer();
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ShouldReturnSeatList()
            {
                var seats = this.BuildSeats(3);
                var result = this.dealer.SetDealer(seats, 1);

                Assert.IsInstanceOfType(result, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldSetSeat1Dealer()
            {
                var seats = this.BuildSeats(3);
                var result = this.dealer.SetDealer(seats, 1);

                Assert.AreEqual(true, result[0].IsDealer);
            }

            [TestMethod]
            public void ShouldSetSeat1AsNotDealerWhenSettingSeat1AsDealer()
            {
                var seats = this.BuildSeats(3);
                seats[0].IsDealer = true;

                var result = this.dealer.SetDealer(seats, 2);
                
                Assert.AreEqual(false, result[0].IsDealer);
            }

            private List<Seat> BuildSeats(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }
        }
    }
}
