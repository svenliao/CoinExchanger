using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Domain.Common.DataBaseModels;
using Domain.Exmo;
using DAL.DataBase.Dao;

namespace API.Exmo
{
    public class ExmoRepertory : IExmoRepertory
    {

        Client client;
        PlatformTable platform;

        private string[] pairs = { "BTC_USD", "BTC_EUR", "BTC_RUB", "ETH_USD", "ETH_EUR", "ETH_RUB", "XRP_USD", "XRP_RUB" };

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

        public List<BookingTable> GetBooking(string coin)
        {
            var dao = new BookingTableDao();
            return dao.Select(coin)
                .Where(b => b.Platform.ID == platform.ID)
                .ToList();
        }

        public bool ReloadBooking()
        {
            var bookings = client.GetOrderBooking(pairs.ToList(), 10);

            if (bookings != null && bookings.Any())
            {
                var tables = new List<BookingTable>();

                foreach (var b in bookings)
                {
                    var table = new BookingTable();
                    string[] spiltIndex = b.Pair.Split('_');

                    table.Currency = spiltIndex[1];
                    table.Coin = spiltIndex[0];
                    table.Pair = b.Pair;
                    table.Platform = this.platform;

                    table.Asks = JsonConvert.SerializeObject(b.Ask);

                    table.Bids = JsonConvert.SerializeObject(b.Bid);

                    tables.Add(table);
                }

                var dao = new BookingTableDao();
                dao.InsertOrUpdate(tables);
            }
            return true;
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
            var depth = client.GetUserInfo();
            var tables = new List<BalanceTable>();

            var account = new AccountDao().Select()
                   .Find(a => a.Default > 0 || a.Platform.ID == platform.ID);
            string uid = account.UID;

            var balances = depth["balances"] as JObject;
            var reserved = depth["reserved"] as JObject;
            foreach (var item in balances)
            {                         
                BalanceTable balance = new BalanceTable()
                {
                    UID = uid,
                    Coin = item.Key,
                    Platform = this.platform,
                    Amount = double.Parse(item.Value.ToString())
                };
                if (reserved[item.Key].HasValues)
                {
                    balance.Amount += double.Parse(reserved[item.Key].ToString());
                }

                tables.Add(balance);
            }

            var dao = new BalanceDao();
            dao.InsertOrUpdate(tables);

            return true;
        }
    }
}
