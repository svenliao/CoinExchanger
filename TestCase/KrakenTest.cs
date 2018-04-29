using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Kraken;
using Domain.Kraken;

namespace TestCase
{
    [TestClass]
    public class KrakenTest
    {
        [TestMethod]
        public void GetKrakenTicker()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.GetTicker();
            Assert.IsNotNull(ticker, "Ticker error.");
            Assert.IsTrue(ticker.Count > 0, "Ticker emty");
        }

        [TestMethod]
        public void ReloadKrakenTicker()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.ReloadTicker();          
            Assert.IsTrue(ticker, "Reload Ticker error");
        }

        [TestMethod]
        public void GetKrakenBalances()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var balance = client.GetBalance();
            Assert.IsNotNull(balance, "Balance error.");
            Assert.IsTrue(balance.Count > 0, "Balance emty");
        }

        [TestMethod]
        public void ReloadKrakenBalances()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.ReloadBlance();
            Assert.IsTrue(ticker, "Reload Balance error");
        }

        [TestMethod]
        public void GetKrakenBookings()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.GetBooking("BTC");
            Assert.IsNotNull(ticker, "Balance error.");
            Assert.IsTrue(ticker.Count > 0, "Balance emty");
        }

        [TestMethod]
        public void ReloadKrakenBookings()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.ReloadBooking();
            Assert.IsTrue(ticker, "Reload Balance error");
        }

        [TestMethod]
        public void GetKrakenOrders()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var orders = client.GetOrder();
            Assert.IsNotNull(orders, "Balance error.");
            Assert.IsTrue(orders.Count > 0, "Orders emty");
        }

        [TestMethod]
        public void ReloadKrakenOrders()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var istrue = client.ReloadOrder();
            Assert.IsTrue(istrue, "Reload Orders error");
        }
    }
}
