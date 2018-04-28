﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Exmo.Kraken.RUB2EUR.KrakenJobs;

namespace Exmo.Kraken.RUB2EUR
{
    public class KrakenRegistry:Registry
    {
        public KrakenRegistry()
        {
            Schedule<BalanceJob>().ToRunNow().AndEvery(10).Seconds();            
            Schedule<TickerJob>().ToRunNow().AndEvery(15).Seconds();           
        }      
    }
}
