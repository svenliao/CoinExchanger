using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Unity;
using FluentScheduler;

namespace Exmo.Kraken.RUB2EUR
{
    public class AppHost : IDisposable
    {
        public ServerStatu Statu { get; private set; }
        public void Init()
        {
            Statu = ServerStatu.INIT;
            JobManager.Initialize(new KrakenRegistry());
            JobManager.Initialize(new ExmoRegistry());
        }

        public void Start()
        {
            Statu = ServerStatu.RUNING;
        }

        public void Stop()
        {
            JobManager.RemoveJob("");
            Statu = ServerStatu.PENDING;
        }

        public void Dispose()
        {
            JobManager.RemoveAllJobs();
            Statu = ServerStatu.CLOSED;
        }
    }

    public enum ServerStatu
    {
        INIT,
        RUNING,
        PENDING,
        CLOSED
    }
}
