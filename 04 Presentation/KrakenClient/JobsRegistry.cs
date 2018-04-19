using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;

namespace KrakenClient
{
    public class JobsRegistry : Registry
    {
        public JobsRegistry()
        {
            Schedule<NotifyIconJob>().ToRunNow().AndEvery(1).Seconds();
            Schedule<BalanceJob>().ToRunNow().AndEvery(5).Seconds();
            Schedule<TickerJob>().ToRunNow().AndEvery(5).Seconds();
            Schedule<BookingJob>().ToRunNow().AndEvery(30).Seconds();
            Schedule<ExchangeRateJob>().ToRunNow().AndEvery(60).Seconds();
        }      
    }
}
