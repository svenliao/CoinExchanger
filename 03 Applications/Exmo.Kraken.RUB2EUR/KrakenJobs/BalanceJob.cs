using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Kraken;
using Domain.Common.Ioc;
using FluentScheduler;
using Unity;


namespace Exmo.Kraken.RUB2EUR.KrakenJobs
{
    public class BalanceJob : IJob
    {
        private IUnityContainer resolver;
        private IKrakenRepertory repertory;

        public BalanceJob()
        {
            resolver = IocFactory.Default;
            repertory = resolver.Resolve<IKrakenRepertory>();
        }
        public void Execute()
        {
            repertory.ReloadBlance();
        }
    }
}
