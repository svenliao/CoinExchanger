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
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
        }

        public bool ReloadBlance()
        {
            var account = new AccountDao().Select()
                   .Find(a => a.Default > 0 || a.Platform.ID == platform.ID);
            string uid = account.UID;

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

                BalanceTable balance = new BalanceTable()
                {
                    UID = uid,
                    Coin = coin,
                    Platform = this.platform,
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

        public OrderTable GetOrder(string txid)
        {
            var dao = new OrderDao();
            return dao.Select(txid);
        }

        public List<OrderTable> GetOrder()
        {
            var dao = new OrderDao();
            return dao.Select()
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
        }

        public bool ReloadOrder()
        {
            var account = new AccountDao().Select()
                   .Find(a => a.Default > 0 || a.Platform.ID == platform.ID);
            string uid = account.UID;

            var dao = new OrderDao();
            var tradeDao = new TradeDao();
            var orders = new List<OrderTable>();
            var trads = new List<TradeTable>();

            string end = dao.SelectTxid();
            var depth = client.GetClosedOrders(true, "", "", end);

            var res = depth["result"] as JsonObject;
            if (res != null)
            {
                var os = res["closed"] as JsonObject;

                foreach (var item in os)
                {
                    JsonObject jOrder = item.Value as JsonObject;
                    JsonObject jDescr = jOrder["descr"] as JsonObject;

                    string pair = jDescr["pair"].ToString();
                    int spiltIndex = (int)Math.Ceiling((double)pair.Length / 2);
                    string coin = pair.Substring(0, spiltIndex);
                    string currency = pair.Substring(spiltIndex, pair.Length - spiltIndex);

                    var order = new OrderTable()
                    {
                        OrderTxid = item.Name,
                        UID=uid,
                        Opened = UnixTimeStampToDateTime(double.Parse(jOrder["opentm"].ToString())),
                        Closed = UnixTimeStampToDateTime(double.Parse(jOrder["closetm"].ToString())),
                        Status = jOrder["status"].ToString(),
                        Volume = double.Parse(jOrder["vol"].ToString()),
                        Price = double.Parse(jOrder["price"].ToString()),
                        LimitPrice = double.Parse(jOrder["limitprice"].ToString()),

                        Type = jDescr["type"].ToString(),
                        LimitType = jDescr["ordertype"].ToString(),
                        Coin = coin,
                        Currency = currency,
                        Pair = pair,
                        CreateTime = DateTime.Now,
                        LastChangeTime = DateTime.Now,
                        Platform = this.platform
                    };

                    var jtrads = jOrder["trades"] as JsonArray;
                    foreach (var jtrad in jtrads)
                    {
                        var trad = new TradeTable()
                        {
                            TransactionID = jtrad.ToString(),
                            OrderTxid = item.Name,
                            CreateTime = DateTime.Now,
                            LastChangeTime = DateTime.Now,
                            Platform = this.platform
                        };

                        trads.Add(trad);
                    }

                    orders.Add(order);
                }

                dao.InsertOrUpdate(orders);
                tradeDao.InsertOrUpdate(trads);
            }
            return true;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime.ToLocalTime();
        }
    }
}