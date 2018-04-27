using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kraken.EUR2USD.Jobs;
using FluentScheduler;

namespace Kraken.EUR2USD
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
