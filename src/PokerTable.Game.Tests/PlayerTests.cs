using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokerTable.Game.Tests
{
    /// <summary>
    /// Player Tests
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// When creating a new player make sure they have an id.
        /// </summary>
        [TestMethod]
        public void Creating_A_New_Player_Should_Have_ID()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.ID);
        }

        /// <summary>
        /// When creating a new player make sure the name is persisted to the object.
        /// </summary>
        [TestMethod]
        public void Creating_A_New_Player_Player_Name_Should_Have_Value()
        {
            var expectedName = "test";
            var player = new Player(expectedName);

            Assert.IsNotNull(player);
            Assert.AreEqual(expectedName, player.Name);
        }

        /// <summary>
        /// When creating a new player make sure the players state is set to Available.
        /// </summary>
        [TestMethod]
        public void Create_A_New_Player_PlayerState_Should_Be_Available()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.AreEqual(Player.States.Available, player.State);
        }

        /// <summary>
        /// When creating a new player make sure the players cards are empty.
        /// </summary>
        [TestMethod]
        public void Create_A_New_Player_Cards_Should_Be_Empty()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Cards);
            Assert.AreEqual(0, player.Cards.Count());
        }

        /// <summary>
        /// When calling Fold, status should be set to fold.
        /// </summary>
        [TestMethod]
        public void Fold_Should_Set_Status_To_Folded()
        {
            var expectedState = Player.States.Folded;
            var player = new Player("test");
            player.Fold();
            Assert.IsNotNull(player);
            Assert.AreEqual(expectedState, player.State);
        }

        /// <summary>
        /// When calling reset, the status should be set to available.
        /// </summary>
        [TestMethod]
        public void Reset_Should_Set_Status_To_Available()
        {
            var expectedState = Player.States.Available;
            var player = new Player("test");
            player.Fold();
            player.Reset();

            Assert.AreEqual(expectedState, player.State);
        }

        /// <summary>
        /// When calling reset, the players cards should be cleared.
        /// </summary>
        [TestMethod]
        public void Reset_Should_Set_Cards_To_EmptyList()
        {
            var player = new Player("test");
            player.Cards.Add(new Card());
            player.Cards.Add(new Card());

            player.Reset();

            Assert.AreEqual(0, player.Cards.Count());
        }
    }
}
