using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.DataBaseModels;
using Domain.Exchange.Rate;
using DAL.DataBase.Dao;

namespace API.Exchange
{
    public class ExchangeRepertory : IExchangeRepertory
    {
        string _url;
        string _key;
        ExchangeApi instance;

        public ExchangeRepertory()
        {
            _url= "http://apilayer.net/api/";
            _key = "960459c89a267414fe02bacf731816c2";
            instance = new ExchangeApi(_url, _key);
        }      

        public List<ExchangeRateTable> GetTodayRates()
        {
            var dao = new ExchangeRateDao();
            return dao.Select();
        }

        public bool ReloadTodayRates()
        {
            var rates = this.LoadTodayRates();

            var dao = new ExchangeRateDao();
            dao.InsertOrUpdate(rates);

            return true;

        }

        private List<ExchangeRateTable> LoadTodayRates()
        {
            var response = Task.Factory.StartNew<LiveModel>(() =>
            {
                var todayRatesQueried = instance.Invoke<LiveModel>("live", new Dictionary<string, string>
                {
                     { "currencies", "AUD,EUR,GBP,CNY,RUB" }
                });

                return todayRatesQueried.Result;
            });

            var task = response.ContinueWith<List<ExchangeRateTable>>((Task<LiveModel> t) => {
                var result = new List<ExchangeRateTable>();

                double cnyRate = Math.Round(double.Parse(t.Result.quotes["USDCNY"]), 4);
                result.Add(new ExchangeRateTable
                {
                    Source = "USD",
                    Currency = "CNY",
                    Quotes = cnyRate,
                    CreateTime = DateTime.Now,
                    LastChangeTime = DateTime.Now
                });

                foreach (var item in t.Result.quotes)
                {
                    if (item.Key != "USDCNY")
                    {
                        result.Add(new ExchangeRateTable
                        {
                            Source = item.Key.Replace("USD", ""),
                            Currency = "CNY",
                            Quotes = Math.Round(cnyRate / double.Parse(item.Value), 4),
                            CreateTime = DateTime.Now,
                            LastChangeTime = DateTime.Now
                        });
                    }
                }
                return result;
            });

            return task.GetAwaiter().GetResult();
        }
    }
}
