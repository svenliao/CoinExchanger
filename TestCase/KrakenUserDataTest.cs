using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Kraken;
using Domain.Kraken;

namespace TestCase
{
    [TestClass]
    public class KrakenUserDataTest
    {
        [TestMethod]
        public void GetTicker()
        {
            IKrakenRepertory client = new KrakenRepertory();

            var ticker = client.GetTicker();
            Assert.IsNotNull(ticker, "Ticker error.");
            Assert.IsTrue(ticker.Count > 0, "Ticker emty");
        }       
    }
}
