using System;
using Newtonsoft.Json;

namespace ExmoAPI.Authenticated_API.Classes
{
    public class CUserOpenOrders : IUserOpenOrders
    {
        [JsonProperty("order_id")]
        public decimal OrderId{get;private set;}
        [JsonProperty("created")]
        public ulong CreatedTime{get;private set;}
        [JsonProperty("type")]
        public string Type{get;private set;}
        [JsonProperty("pair")]
        public string TradeCouples { get;private set;}
        [JsonProperty("price")]
        public decimal Price{get;private set;}
        [JsonProperty("quantity")]
        public decimal Quantity{get;private set;}
        [JsonProperty("amount")]
        public decimal Amount{get;private set;}
    }
}