using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jayrock.Json;
using System.Threading;
using Jayrock.Json.Conversion;
using Domain.Common.DataBaseModels;
using Domain.Kraken;
using DAL.DataBase.Dao;

namespace API.Kraken
{
    public class KrakenRepertory : IKrakenRepertory
    {
        Client client;
        Broker broker;
        PlatformTable platform;

        private string[] pairs = { "XXBTZEUR","XXBTZUSD","BCHEUR","BCHUSD","EOSEUR","EOSUSD","XETCZEUR","XETCZUSD","XETHZEUR",
                    "XETHZUSD","XLTCZEUR","XLTCZUSD","XXLMZEUR","XXLMZUSD","XXMRZEUR","XXMRZUSD","XXRPZEUR","XXRPZUSD"};

        public KrakenRepertory()
        {
            client = new Client();
            broker = new Broker();
            platform = client.Platform;
        }
        public List<BalanceTable> GetBalance()
        {
            var dao = new BalanceDao();
            return dao.Select()
                .Where(b=>b.Platform.ID==platform.ID)
                .ToList();
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

                var account = new AccountDao().Select()
                    .Find(a => a.Default > 0||a.Platform.ID==platform.ID);

                string uid = account.UID;

                BalanceTable balance = new BalanceTable()
                {
                    UID = uid,
                    Coin = coin,
                    Platform=this.platform,
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
            return dao.Select()
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
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
                ticker.Platform = this.platform;

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

        public List<BookingTable> GetBooking(string coin)
        {
            var dao = new BookingTableDao();
            return dao.Select(coin)
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
        }

        public bool ReloadBooking()
        {
            var bookings = new List<BookingTable>();
            foreach (var aPair in pairs)
            {
                var depth = client.GetOrderBook(aPair, 5);

                var res = depth["result"] as JsonObject;
                var xrp = res[aPair] as JsonObject;
                var asks = xrp["asks"].ToString();
                var bids = xrp["bids"].ToString();

                int spiltIndex = (int)Math.Ceiling((double)aPair.Length / 2);
                string coin = aPair.Substring(0, spiltIndex);
                string currency = aPair.Substring(spiltIndex, aPair.Length - spiltIndex);
                if (coin.StartsWith("X"))
                {
                    coin = coin.Substring(1);
                }
                if (currency.StartsWith("Z"))
                {
                    currency = currency.Substring(1);
                }

                var booking = new BookingTable()
                {
                    Pair = aPair,
                    Coin = coin.Replace("XBT", "BTC"),
                    Currency = currency,
                    Platform = this.platform,
                    Asks = asks,
                    Bids = bids
                };
                bookings.Add(booking);                
            }

            var dao = new BookingTableDao();
            dao.InsertOrUpdate(bookings);

            return true;
        }
    }
}