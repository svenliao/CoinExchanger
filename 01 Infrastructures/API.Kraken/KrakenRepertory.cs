using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Domain.Common.DataBaseModels;
using Domain.Kraken;
using DAL.DataBase.Dao;

namespace API.Kraken
{
    public class KrakenRepertory : IKrakenRepertory
    {
        private Client client = new Client();
        private Broker broker = new Broker();

        private string[] pairs = { "XXBTZEUR","XXBTZUSD","BCHEUR","BCHUSD","EOSEUR","EOSUSD","XETCZEUR","XETCZUSD","XETHZEUR",
                    "XETHZUSD","XLTCZEUR","XLTCZUSD","XXLMZEUR","XXLMZUSD","XXMRZEUR","XXMRZUSD","XXRPZEUR","XXRPZUSD"};

        public List<BalanceTable> GetBalance()
        {
            var dao = new BalanceDao();
            return dao.Select();
        }

        public bool ReloadBlance()
        {
            var depth = client.GetBalance();
            var balances = new List<BalanceTable>();
            var res = depth["result"] as JsonObject;
            foreach (var item in res)
            {
                string coin = item.Name.Replace("XBT", "BTC");
                if (coin.StartsWith("X"))
                {
                    coin = coin.Substring(1);
                }
                if (coin.StartsWith("Z"))
                {
                    coin = coin.Substring(1);
                }

                var account = new AccountDao().Select().Find(a => a.Default > 0);
                string uid = account.UID;

                BalanceTable balance = new BalanceTable()
                {
                    UID = uid,
                    Coin = coin,
                    Amount = double.Parse(item.Value.ToString())
                };

                balances.Add(balance);
            }

            var dao = new BalanceDao();
            dao.InsertOrUpdate(balances);

            return true;
        }

        public List<TickerTable> GetTicker()
        {
            var dao = new TickerDao();
            return dao.Select();
        }

        public bool ReloadTicker()
        {
            var depth = client.GetTicker(this.pairs.ToList());
            var tickers = new List<TickerTable>();
            var res = depth["result"] as JsonObject;
            foreach (var item in res)
            {
                TickerTable ticker = null;

                int spiltIndex = (int)Math.Ceiling((double)item.Name.Length / 2);
                string coin = item.Name.Substring(0, spiltIndex);
                string currency = item.Name.Substring(spiltIndex, item.Name.Length - spiltIndex);
                if (coin.StartsWith("X"))
                {
                    coin = coin.Substring(1);
                }
                if (currency.StartsWith("Z"))
                {
                    currency = currency.Substring(1);
                }

                ticker = new TickerTable();
                tickers.Add(ticker);

                ticker.Currency = currency;
                ticker.Coin = coin.Replace("XBT", "BTC");
                ticker.Pair = item.Name;

                JsonObject o = item.Value as JsonObject;
                ticker.Ask = double.Parse((o["a"] as JsonArray)[0].ToString());
                ticker.AskCount = double.Parse((o["a"] as JsonArray)[1].ToString());

                ticker.Bid = double.Parse((o["b"] as JsonArray)[0].ToString());
                ticker.BidCount = double.Parse((o["b"] as JsonArray)[1].ToString());
            }

            var dao = new TickerDao();
            dao.InsertOrUpdate(tickers);

            return true;
        }

    }
}