using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Domain.Exchange;
using API.Exchange;
using Domain.Exchange.Rate;
using API.Kraken;
using Domain.Kraken;
using Applications.Server;

namespace TestCase
{
    [TestClass]
    public class AppTest:TestBase
    {
        [TestMethod]
        public void StartServer()
        {
            var host = new AppHost();

            var startTime = DateTime.Now;
            host.Start();

            Thread.Sleep(2*60 * 1000);

            IExchangeRepertory client = new ExchangeRepertory();
            var rate = client.GetTodayRates();
            Assert.IsTrue(rate[0].LastChangeTime.ToFileTimeUtc() > startTime.ToFileTimeUtc(), "Rate error.");

            IKrakenRepertory krakenClient = new KrakenRepertory();
            var ticker = krakenClient.GetTicker();
            Assert.IsTrue(ticker[0].LastChangeTime.ToFileTimeUtc() > startTime.ToFileTimeUtc(), "Ticker error.");

            var balance = krakenClient.GetBalance();
            Assert.IsTrue(balance[0].LastChangeTime.ToFileTimeUtc() > startTime.ToFileTimeUtc(), "Balance error.");

            var booking = krakenClient.GetBooking("BTC");
            Assert.IsTrue(booking[0].LastChangeTime.ToFileTimeUtc() > startTime.ToFileTimeUtc(), "Booking error.");

            host.Dispose();
        }
    }
}
