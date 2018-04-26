using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExmoAPI.Generic;
using Newtonsoft.Json.Linq;

namespace ExmoAPI.Public_API.Classes
{
    //Cписок валют биржи
    public static class CCurrency
    {
        public static IList<string> CurrencyList { get; private set; }
        public static IList<string> CurrencyPairList { get; private set; }

        //Получить имеющиеся валюты на бирже
        public static async Task<IList<string>> GetCurrencyAsync(ExmoApi api)
        {
            api = api ?? (new ExmoApi("", ""));
            var jsonCurrency = await api.ApiQueryAsync("currency", new Dictionary<string, string>());
            var parserStrings = jsonCurrency.ToString()
                .Replace("[", "")
                .Replace("]", "")
                .Replace("\"", "")
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return CurrencyList = parserStrings.Cast<string>().ToList();
        }

        //Получить имеющиеся валютные пары
        public static async Task<IList<string>> GetCurrencyPairListAsync(ExmoApi api)
        {
            api = api ?? (new ExmoApi("", ""));
            var jsonPairSettings =await api.ApiQueryAsync("pair_settings", new Dictionary<string, string>());
            var objPairSettings = JObject.Parse(jsonPairSettings.ToString());
            var tempReplace = objPairSettings.ToString()
                .Replace("{", "")
                .Replace("}", "")
                .Replace("\"", "")
                .Replace(":", "")
                .Replace("min_quantity", "")
                .Replace("max_quantity", "")
                .Replace("min_price", "")
                .Replace("max_price", "")
                .Replace("max_amount", "")
                .Replace("min_amount", "")
                .Replace(".", " ").Replace(",", " ");
            var tempArray = tempReplace.ToCharArray().Where(n => !char.IsDigit(n)).ToArray();
            var resultString = (new string(tempArray)).Replace(System.Environment.NewLine, "")
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return CurrencyPairList = resultString.ToList();
        }

    }
}