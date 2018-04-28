using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exmo;
using Domain.Common.Ioc;
using FluentScheduler;
using Unity;

namespace Exmo.Kraken.RUB2EUR.ExmoJobs
{
    public class TickerJob : IJob
    {
        private IUnityContainer resolver;
        private IExmoRepertory repertory;

        public TickerJob()
        {
            resolver = IocFactory.Default;
            repertory = resolver.Resolve<IExmoRepertory>();
        }
        public void Execute()
        {
            repertory.ReloadTicker();
        }
    }
}
