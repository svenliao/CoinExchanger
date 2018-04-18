using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Unity;
using FluentScheduler;

namespace Application.Server
{
    public class AppHost : IDisposable
    {
        public void Init()
        {
        }

        public void Start()
        {
            JobManager.Initialize(new KrakenRegistry());
        }

        public void Stop()
        {
            JobManager.RemoveAllJobs();
        }

        public void Dispose()
        {
            JobManager.RemoveAllJobs();
        }
    }
}
