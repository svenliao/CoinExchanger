using System;
using System.Collections.Generic;
using ExmoAPI.Authenticated_API.Interfeces;
using Newtonsoft.Json;

namespace ExmoAPI.Authenticated_API.Classes
{
    public class COrderTrades : IOrderTrades
    {
        [JsonProperty("type")]
        public string Type {get;private set;}
        [JsonProperty("in_currency")]
        public string InCurrency {get;private set;}
        [JsonProperty("in_amount")]
        public decimal InAmount {get;private set;}
        [JsonProperty("out_currency")]
        public string OutCurrency {get;private set;}
        [JsonProperty("out_amount")]
        public decimal OutAmount {get;private set;}
        [JsonProperty("trades")]
        public List<CTrade> Trades {get;private set;}
    }
    public class CTrade : ITrade
    {
        [JsonProperty("trade_id")]
        public decimal TradeId {get;private set;}
        [JsonProperty("date")]
        public ulong Date {get;private set;}
        [JsonProperty("type")]
        public string Type {get;private set;}
        [JsonProperty("pair")]
        public string TradeCouples { get;private set;}
        [JsonProperty("order_id")]
        public decimal OrderId {get;private set;}
        [JsonProperty("quantity")]
        public decimal Quantity {get;private set;}
        [JsonProperty("price")]
        public decimal Price {get;private set;}
        [JsonProperty("amount")]
        public decimal Amount {get;private set;}
    }
}