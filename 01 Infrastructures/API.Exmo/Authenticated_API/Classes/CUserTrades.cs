using System;
using ExmoAPI.Authenticated_API.Interfeces;
using Newtonsoft.Json;

namespace ExmoAPI.Authenticated_API.Classes
{
    public class CUserTrades : IUserTrades
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