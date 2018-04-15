using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.DataBaseModels;

namespace Domain.Exchange.Rate
{
    public interface IExchangeRepertory
    {
        List<ExchangeRateTable> GetTodayRates();

        bool ReloadTodayRates();
    }
}
