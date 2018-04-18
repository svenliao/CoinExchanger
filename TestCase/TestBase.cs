using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Exchange;
using API.Kraken;
using Domain.Exchange.Rate;
using Domain.Kraken;
using Domain.Common.Ioc;
using Unity;

namespace TestCase
{
    public class TestBase
    {
        
        public TestBase()
        {
            var resolver = IocFactory.Default;
            resolver.RegisterType<IKrakenRepertory, KrakenRepertory>();
            resolver.RegisterType<IExchangeRepertory, ExchangeRepertory>();


        }

    }
}
