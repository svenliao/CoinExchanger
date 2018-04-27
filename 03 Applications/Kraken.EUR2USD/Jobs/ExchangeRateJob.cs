using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exchange.Rate;
using Domain.Common.Ioc;
using FluentScheduler;
using Unity;

namespace Kraken.EUR2USD.Jobs
{
    public class ExchangeRateJob : IJob
    {
        private IUnityContainer resolver;
        private IExchangeRepertory repertory;

        public ExchangeRateJob()
        {
            resolver = IocFactory.Default;
            repertory = resolver.Resolve<IExchangeRepertory>();
        }
        public void Execute()
        {
            repertory.ReloadTodayRates();
        }
    }
}
