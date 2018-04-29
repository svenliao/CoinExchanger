using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Exmo.Model;
using Domain.Common.DataBaseModels;
using DAL.DataBase.Dao;

namespace API.Exmo
{
    public class Client : IDisposable
    {
        string _url;
        int _version;
        string _key;
        string _secret;
        long _nounce;
        PlatformTable _platform;

        static object locker = new object();

        public PlatformTable Platform
        {
            get
            {
                if (_platform == null)
                {
                    var dao = new PlatformDao();
                    _platform = dao.Select("Exmo").FirstOrDefault();
                }
                return _platform;
            }
        }

        public Client()
        {
            var dao = new AccountDao();
            var account = dao.Select().Find(a => a.Default > 0 && a.Platform.ID==Platform.ID);

            _url = Platform.Url;
            _version = account.ApiVersion;
            _key = account.Key;
            _secret = account.Secret;
        }

        public void Dispose()
        {           
            GC.SuppressFinalize(this);
        }

        public async Task<string> QueryAsync(string apiName, IDictionary<string, string> req, string tradeCouples = null, int? limit = null)
        {
            using (var http = new HttpClient())
            {
                _nounce = this.GetTimestamp();
                var n = Interlocked.Increment(ref _nounce);
                req.Add("nonce", Convert.ToString(n));
                var message = ToQueryString(req);

                var sign = Sign(_secret, message);

                var content = new FormUrlEncodedContent(req);
                content.Headers.Add("Sign", sign);
                content.Headers.Add("Key", _key);
                HttpResponseMessage response;
                if (limit == null)
                {
                    if (tradeCouples != null)
                        response = await http.GetAsync(string.Format("{0}/{1}/?pair={2}",_url, apiName, tradeCouples));
                    else
                        response = await http.PostAsync(string.Format("{0}/{1}",_url, apiName), content);
                }
                else
                {
                    if (tradeCouples != null)
                        response = await http.GetAsync(string.Format("{0}/{1}/?pair={2}&limit={3}", _url,apiName, tradeCouples, limit.ToString()));
                    else
                        response = await http.PostAsync(string.Format("{0}/{1}", _url, apiName), content);
                }
                return await response.Content.ReadAsStringAsync();
            }
        }

        public string Query(string apiName, IDictionary<string, string> req, string tradeCouples = null, int? limit = null)
        {
            using (var wb = new WebClient())
            {
                _nounce = GetTimestamp();
                req.Add("nonce", Convert.ToString(_nounce));
                var message = ToQueryString(req);

                var sign = Sign(_secret, message);

                wb.Headers.Add("Sign", sign);
                wb.Headers.Add("Key", _key);

                var data = ToNameValueCollection(req);
                //var response = wb.UploadValues(string.Format(_url, apiName), "POST", data);
                byte[] response;
                if (limit == null)
                {
                    if (tradeCouples != null)
                    {
                        string tmp = string.Format("{0}/{1}/?pair={2}",_url ,apiName, tradeCouples);
                        response = wb.UploadValues(tmp, "POST", data);
                    }
                    else
                        response = wb.UploadValues(string.Format("{0}/{1}",_url, apiName), "POST", data);
                }
                else
                {
                    if (tradeCouples != null)
                        response = wb.UploadValues(string.Format("{0}/{1}/?pair={2}&limit={3}",_url, apiName, tradeCouples, limit.ToString()), "POST", data);
                    else
                        response = wb.UploadValues(string.Format("{0}/{1}", _url, apiName), "POST", data);
                }
                return Encoding.UTF8.GetString(response);
            }
        }

        #region Pulick queries

        public List<Ticker> GetTicker(List<string> pairs)
        {
            var jsonQuery = Query("ticker", new Dictionary<string, string>());
            var objQuery = JObject.Parse(jsonQuery.ToString());

            var tickers = new List<Ticker>();

            foreach (var pair in pairs)
            {
                var ticker = (JsonConvert.DeserializeObject<Ticker>(objQuery[pair].ToString()));
                ticker.Pair = pair;
                tickers.Add(ticker);
            }

            return tickers;
        }

        public List<OrderBook> GetOrderBooking(List<string> pairs,int limit)
        {
            string strPair = string.Empty;
            var orders = new List<OrderBook>();

            pairs.ForEach(p => {
                strPair = strPair + p + ",";
            });

            if (!string.IsNullOrEmpty(strPair))
            {
                strPair = strPair.Substring(0, strPair.Length - 1);
            }
            var jsonQuery = Query("order_book", new Dictionary<string, string>(), strPair,limit);
            var objQuery = JObject.Parse(jsonQuery.ToString());

            foreach (var pair in pairs)
            {
                var order = (JsonConvert.DeserializeObject<OrderBook>(objQuery[pair].ToString()));
                order.Pair = pair;
                orders.Add(order);
            }

            return orders;
        }

        #endregion

        #region Private user data queries

        public JObject GetUserInfo()
        {
            var jsonQuery = Query("user_info", new Dictionary<string, string>());
            var objQuery = JObject.Parse(jsonQuery.ToString());

            return objQuery;
        }
        #endregion
        #region Helper methods
        public string Sign(string key, string message)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                string sbinary = "";

                for (int i = 0; i < b.Length; i++)
                {
                    sbinary += b[i].ToString("X2"); // hex format
                }
                return (sbinary).ToLowerInvariant();
            }
        }

        private string ToQueryString(IDictionary<string, string> dic)
        {
            var array = (from key in dic.Keys
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(dic[key])))
                .ToArray();
            return string.Join("&", array);
        }

        private byte[] sha256_hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                return result;
            }
        }

        private byte[] getHash(byte[] keyByte, byte[] messageBytes)
        {
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {

                Byte[] result = hmacsha512.ComputeHash(messageBytes);

                return result;

            }
        }
        public NameValueCollection ToNameValueCollection<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dict)
            {
                string value = string.Empty;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                nameValueCollection.Add(kvp.Key.ToString(), value);
            }

            return nameValueCollection;
        }

        public long GetTimestamp()
        {
            var d = DateTime.Now.Ticks;
            return (long)d;
        }

        #endregion
    }
}
