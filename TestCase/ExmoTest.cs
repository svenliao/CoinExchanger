using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exmo;
using API.Exmo;

namespace TestCase
{
    [TestClass]
    public class ExmoTest
    {      
        [TestMethod]
        public void ReloadExmoTicker()
        {
            IExmoRepertory client = new ExmoRepertory();

            var isTrue = client.ReloadTicker();
            Assert.IsTrue(isTrue, "ExmoTicker reload failed");
        }

        [TestMethod]
        public void ReloadExmoBooking()
        {
            IExmoRepertory client = new ExmoRepertory();

            var isTrue = client.ReloadBooking();
            Assert.IsTrue(isTrue, "Exmo Booking reload failed");
        }

        [TestMethod]
        public void ReloadExmoBlance()
        {
            IExmoRepertory client = new ExmoRepertory();

            var isTrue = client.ReloadBlance();
            Assert.IsTrue(isTrue, "Exmo Blance reload failed");
        }

        [TestMethod]
        public void GetExmoTicker()
        {
            IExmoRepertory client = new ExmoRepertory();

            var ticker = client.GetTicker();
            Assert.IsNotNull(ticker, "Ticker error.");
            Assert.IsTrue(ticker.Count > 0, "Ticker emty");
        }

        [TestMethod]
        public void GetExmoBooking()
        {
            IExmoRepertory client = new ExmoRepertory();

            var booking = client.GetBooking("BTC");
            Assert.IsNotNull(booking, "Ticker error.");
            Assert.IsTrue(booking.Count > 0, "Ticker emty");
        }

        [TestMethod]
        public void GetExmoBlance()
        {
            IExmoRepertory client = new ExmoRepertory();

            var balance = client.GetBalance();
            Assert.IsNotNull(balance, "Ticker error.");
            Assert.IsTrue(balance.Count > 0, "Ticker emty");
        }
    }
}
