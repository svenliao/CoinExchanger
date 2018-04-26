using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Management;
using ExmoAPI.Public_API.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExmoAPI.Generic
{
    public class CHelperPublicAPI<T> : IHelperPublicAPI<T>
    {
        public IList<T> ResultList { get; private set; } = null;
        public T ResultMetod { get; private set; } = default(T);

        public async Task GetResultAsync(string method, ExmoApi api=null, string tradeCouples="BTC_USD", int? limit = null)
        {
            api = api ?? (new ExmoApi("", ""));
            var jsonQuery = await api.ApiQueryAsync(method, new Dictionary<string, string>(), tradeCouples, limit);
            var objQuery = JObject.Parse(jsonQuery.ToString());
            if (method == "trades")
                ResultList = (JsonConvert.DeserializeObject<T[]>(objQuery[tradeCouples].ToString())).ToList();
            
            else
                ResultMetod = JsonConvert.DeserializeObject<T>(objQuery[tradeCouples].ToString());
                
        }
    }

    public class CHelperAuthAPI<T> : IHelperAuthAPI<T>
    {
        public IList<T> ResultList { get; private set; } = null;

        public T ResultMetod { get; private set; } = default(T);
        
        public async Task GetResultAsync(string method, ExmoApi api, Dictionary<string, string> dic = null, string tradeCouples = "BTC_USD")
        {
            if (dic == null)
                dic=new Dictionary<string, string>();
            string jsonQuery = default;
            JObject objQuery = default;

            switch (method)
            {
                case "user_info":
                    await AssistingMethod(method, api, dic);
                    return;

                case "order_create":
                    await AssistingMethod(method, api, dic);
                    return;

                case "order_cancel":
                    await AssistingMethod(method, api, dic);
                    return;

                case "user_open_orders":
                    jsonQuery = await api.ApiQueryAsync(method, dic);
                    objQuery = JObject.Parse(jsonQuery.ToString());
                    IList<T> resultUserOpenOrders = new List<T>();
                    await CCurrency.GetCurrencyPairListAsync(null);
                    foreach (var c in CCurrency.CurrencyPairList)
                    {
                        if (objQuery.ToString().Contains(c))
                        {
                            IList<T> tempList = JsonConvert.DeserializeObject<T[]>(objQuery[c].ToString());
                            foreach (var tmp in tempList)
                                resultUserOpenOrders.Add(tmp);
                        }
                    }
                    ResultList = resultUserOpenOrders;
                    return;

                case "user_trades":
                    List<T> resultUserTrades = new List<T>();
                    tradeCouples = dic["pair"];
                    tradeCouples = tradeCouples.Replace(" ", string.Empty).Trim().Replace(" ", string.Empty);
                    string[] pairs = tradeCouples.Split(',');
                    foreach (var p in pairs)
                    {
                        dic["pair"] = p;
                        jsonQuery = await api.ApiQueryAsync(method, dic);
                        objQuery = JObject.Parse(jsonQuery.ToString());
                        IList<T> tempList = JsonConvert.DeserializeObject<T[]>(objQuery[p].ToString());
                        resultUserTrades.AddRange(tempList);
                        dic.Remove("nonce");
                    }
                    ResultList = resultUserTrades;
                    return;

                case "user_cancelled_orders":
                    jsonQuery = await api.ApiQueryAsync(method, dic);
                    var jarray = JArray.Parse(jsonQuery);
                    ResultList = JsonConvert.DeserializeObject<T[]>(jarray.ToString());
                    return;
                default:
                    //return;
                    break;

            }

#if DEBUG


            
                jsonQuery = await api.ApiQueryAsync(method, dic);
                //JArray ttt=JArray.Parse(jsonQuery);
                objQuery = JObject.Parse(jsonQuery.ToString());
                
            
    
            
                //var jsonQuery = await api.ApiQueryAsync(method, dic);
                //var objQuery = JObject.Parse(jsonQuery.ToString());
                ResultMetod = JsonConvert.DeserializeObject<T>(objQuery.ToString());
            //ResultMetod = objResult;
#endif

        }

        private async Task AssistingMethod(string method, ExmoApi api, Dictionary<string, string> dic)
        {
            string jsonQuery;
            JObject objQuery;
            jsonQuery = await api.ApiQueryAsync(method, dic);
            objQuery = JObject.Parse(jsonQuery.ToString());
            ResultMetod = JsonConvert.DeserializeObject<T>(objQuery.ToString());
        }
    }
}