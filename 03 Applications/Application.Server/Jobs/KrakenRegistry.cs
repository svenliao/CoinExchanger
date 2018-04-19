using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Server.Jobs;
using FluentScheduler;

namespace Applications.Server
{
    public class KrakenRegistry:Registry
    {
        public KrakenRegistry()
        {
            Schedule<BalanceJob>().ToRunNow().AndEvery(10).Seconds();
            Schedule<BookingJob>().ToRunNow().AndEvery(120).Seconds();
            Schedule<TickerJob>().ToRunNow().AndEvery(5).Seconds();

            Schedule<ExchangeRateJob>().ToRunNow().AndEvery(100).Seconds();
        }      
    }
}
