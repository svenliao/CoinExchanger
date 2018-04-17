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
        public void GetTicker()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.GetTicker();
            Assert.IsNotNull(ticker, "Ticker error.");
            Assert.IsTrue(ticker.Count > 0, "Ticker emty");
        }

        [TestMethod]
        public void ReloadTicker()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.ReloadTicker();          
            Assert.IsTrue(ticker, "Reload Ticker error");
        }

        [TestMethod]
        public void GetBalances()
        {
            KrakenRepertory client = new KrakenRepertory();

            var ticker = client.GetBalance();
            Assert.IsNotNull(ticker, "Balance error.");
            Assert.IsTrue(ticker.Count > 0, "Balance emty");
        }

        [TestMethod]
        public void ReloadBalances()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.ReloadBlance();
            Assert.IsTrue(ticker, "Reload Balance error");
        }
    }
}
