using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Unity;
using FluentScheduler;

namespace Kraken.EUR2USD
{
    public class AppHost : IDisposable
    {
        public ServerStatu Statu { get; private set; }
        public void Init()
        {
            Statu = ServerStatu.INIT;
        }

        public void Start()
        {
            JobManager.Initialize(new KrakenRegistry());
            Statu = ServerStatu.RUNING;
        }

        public void Stop()
        {
            JobManager.RemoveAllJobs();
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
