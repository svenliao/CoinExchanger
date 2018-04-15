using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Exchange;
using API.Exchange;
using Domain.Exchange.Rate;

namespace TestCase
{
    [TestClass]
    public class ExchangeTest
    {
        [TestMethod]
        public void GetTodayRate()
        {
            IExchangeRepertory client = new ExchangeRepertory();

            var rate= client.GetTodayRates();
            Assert.IsNotNull(rate, "Exchange error.");
            Assert.IsTrue(rate.Count > 0, "Exchange emty");
        }

        [TestMethod]
        public void ReloadTodayRate()
        {
            IExchangeRepertory client = new ExchangeRepertory();

            var isTrue = client.ReloadTodayRates();
            Assert.IsTrue(isTrue, "Exchange reload failed");
        }
    }
}
