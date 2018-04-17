using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Domain.Common.DataBaseModels;

namespace Domain.Kraken
{
    public interface IKrakenRepertory
    {
        List<BalanceTable> GetBalance();

        bool ReloadBlance();

        List<TickerTable> GetTicker();

        bool ReloadTicker();
    }
}