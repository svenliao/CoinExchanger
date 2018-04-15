using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Domain.Common.DataBaseModels;
using Domain.Kraken;

namespace API.Kraken
{
    public class KrakenRepertory:IKrakenRepertory
    {
        public  Client client = new Client();
        public  Broker broker = new Broker();
        public  CurrencyClient rateClient = new CurrencyClient();

        public List<BalanceTable> GetBalance()
        {
            return null;
        }

        public bool ReloadBlance()
        {
            return true;
        }
    }
}