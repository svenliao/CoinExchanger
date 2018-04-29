using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Exmo.Kraken.RUB2EUR.ExmoJobs;

namespace Exmo.Kraken.RUB2EUR
{
    public class ExmoRegistry : Registry
    {
        public ExmoRegistry()
        {
            Schedule<BalanceJob>().ToRunNow().AndEvery(5).Seconds();
            Schedule<BookingJob>().ToRunNow().AndEvery(10).Seconds();
            Schedule<TickerJob>().ToRunNow().AndEvery(5).Seconds();
        }      
    }
}
