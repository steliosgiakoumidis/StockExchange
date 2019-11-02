
using StockExchange;
using StockExchange.DbModels;
using StockExchange.Interfaces;
using StockExchange.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestAlpcot
{
    public class ServerSideLogicTests
    {
        private Mock<IDatabaseAccess> _databaseAccess = new Mock<IDatabaseAccess>();
        private Mock<IOrderBookActions> _orderBookActions = new Mock<IOrderBookActions>();

        [Fact]
        public async Task BuyOneORderSame()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);

            Assert.False(orderBook.BuyOrders.IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Once);
        }


        [Fact]
        public async Task SellOneOrder()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Sell,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);

            Assert.False(orderBook.SellOrders[order1.Price].IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task BuyAndSellOneSetOfTransactionSamePriceStocksLeftInBook()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Sell,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var order2 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 8,
                CustomerId = 2,
                OrderId = 3141,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);
            await orderbookActions.PlaceOrder(order2);

            Assert.False(orderBook.SellOrders[order2.Price].IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Exactly(2));
            _databaseAccess.Verify(f => f.PersistTransaction(It.IsAny<Transaction>(), It.IsAny<Transaction>()),
                Times.Exactly(8));
        }

        [Fact]
        public async Task BuyOnHigherPriceAllSold()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Sell,
                StockName = "Microsoft",
                NumberOfStocks = 8,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var order2 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 8,
                CustomerId = 2,
                OrderId = 3141,
                Price = 15,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);
            await orderbookActions.PlaceOrder(order2);

            Assert.True(orderBook.SellOrders[order1.Price].IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Exactly(2));
            _databaseAccess.Verify(f => f.PersistTransaction(It.IsAny<Transaction>(), It.IsAny<Transaction>()),
                Times.Exactly(8));
        }

        [Fact]
        public async Task BuyAndSellLowerPriceNoTransaction()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Sell,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var order2 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 8,
                CustomerId = 2,
                OrderId = 3141,
                Price = 10,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);
            await orderbookActions.PlaceOrder(order2);

            Assert.False(orderBook.SellOrders[order1.Price].IsEmpty);
            Assert.False(orderBook.SellOrders[order2.Price].IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Exactly(2));
        }


        [Fact]
        public async Task BuyAndSellTwoSetOfTransactionStocksLeftOnBuyBook()
        {
            var orderBook = new OrderBook();
            var order1 = new Order()
            {
                Action = StockExchange.Models.Action.Sell,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 1,
                OrderId = 312341,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var order2 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 8,
                CustomerId = 2,
                OrderId = 3141,
                Price = 12,
                Timestamp = DateTime.UtcNow
            };
            var order3 = new Order()
            {
                Action = StockExchange.Models.Action.Buy,
                StockName = "Microsoft",
                NumberOfStocks = 10,
                CustomerId = 3,
                OrderId = 2341,
                Price = 15,
                Timestamp = DateTime.UtcNow
            };
            var orderbookActions = new OrderBookActions(orderBook, _databaseAccess.Object);
            await orderbookActions.PlaceOrder(order1);
            await orderbookActions.PlaceOrder(order2);
            await orderbookActions.PlaceOrder(order3);

            Assert.Throws<KeyNotFoundException>(() => orderBook.BuyOrders[order2.Price]);
            Assert.False(orderBook.BuyOrders[order3.Price].IsEmpty);
            Assert.True(orderBook.SellOrders[order2.Price].IsEmpty);
            _databaseAccess.Verify(f => f.PersistOrder(It.IsAny<Order>()), Times.Exactly(3));
            _databaseAccess.Verify(f => f.PersistTransaction(It.IsAny<Transaction>(), It.IsAny<Transaction>()),
                Times.Exactly(10));
        }
    }
}
