using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.DataBaseModels;
using Domain.Exmo;
using DAL.DataBase.Dao;

namespace API.Exmo
{
    public class ExmoRepertory:IExmoRepertory
    {

        Client client;
        PlatformTable platform;

        private string[] pairs = { "BTC_USD","BTC_EUR","BTC_RUB","ETH_USD","ETH_EUR","ETH_RUB","XRP_USD","XRP_RUB"};

        public ExmoRepertory()
        {
            client = new Client();
            platform = client.Platform;
        }

        public bool ReloadTicker()
        {
            var tickers = client.GetTicker(pairs.ToList());

            if (tickers != null && tickers.Any())
            {
                var tables = new List<TickerTable>();

                foreach (var t in tickers)
                {
                    var table = new TickerTable();
                    string[] spiltIndex = t.Pair.Split('_');

                    table.Currency = spiltIndex[1];
                    table.Coin = spiltIndex[0];
                    table.Pair = t.Pair;
                    table.Platform = this.platform;

                    table.Ask = (double)t.SellPrice;
                    table.AskCount = 0;

                    table.Bid = (double)t.BuyPrice;
                    table.BidCount = 0;

                    tables.Add(table);
                }

                var dao = new TickerDao();
                dao.InsertOrUpdate(tables);
            }
            return true;
        }
        public List<TickerTable> GetTicker()
        {
            var dao = new TickerDao();
            return dao.Select()
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
        }
    }
}
