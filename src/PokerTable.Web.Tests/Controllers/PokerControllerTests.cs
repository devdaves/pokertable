using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game;
using PokerTable.Game.Models;
using PokerTable.Web.Controllers;
using PokerTable.Web.Models.JsonModels;

namespace PokerTable.Web.Tests.Controllers
{
    [TestClass]
    public class PokerControllerTests
    {
        private Mock<IEngine> engineMock;

        private PokerController controller;

        [TestInitialize]
        public void Setup()
        {
            this.engineMock = new Mock<IEngine>();
            this.controller = new PokerController(engineMock.Object);
        }

        [TestMethod]
        public void CreateTable_Returns_JsonResult()
        {
            var someTableName = "something";
            var result = this.controller.CreateTable(someTableName);

            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void CreateTable_Returns_CreateTableJson_as_Model()
        {
            var someTableName = "something";
            var result = this.controller.CreateTable(someTableName);
            
            Assert.IsInstanceOfType(result.Data, typeof(CreateTableJson));
        }

        [TestMethod]
        public void CreateTable_EmptyTableName_Returns_Status_of_One()
        {
            var expectedStatus = 1;
            var emptyTableName = string.Empty;
            var result = this.controller.CreateTable(emptyTableName);
            var data = result.Data as CreateTableJson;

            Assert.IsNotNull(data);
            Assert.AreEqual(expectedStatus, data.Status);
        }

        [TestMethod]
        public void CreateTable_ValidTableName_Calls_CreateTableOnEngine()
        {
            var someTableName = "something";
            this.controller.CreateTable(someTableName);
            
            engineMock.Verify(x => x.CreateNewTable(10, someTableName));
        }

        [TestMethod]
        public void CreateTable_ValidTableName_ReturnsTableId()
        {
            var expectedTableId = Guid.NewGuid();
            var someTableName = "something";
            engineMock.SetupGet(x => x.Table).Returns(new Table {Id = expectedTableId});

            var result = this.controller.CreateTable(someTableName);
            var data = result.Data as CreateTableJson;

            Assert.IsNotNull(data);
            Assert.AreEqual(expectedTableId, data.TableId);
        }
    }
}
