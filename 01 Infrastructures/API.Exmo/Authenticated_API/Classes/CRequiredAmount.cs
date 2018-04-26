using System;
using ExmoAPI.Authenticated_API.Interfeces;
using Newtonsoft.Json;

namespace ExmoAPI.Authenticated_API.Classes
{
    public class CRequiredAmount : IRequiredAmount
    {
        [JsonProperty("quantity")]
        public decimal Quantity {get;private set;}
        [JsonProperty("amount")]
        public decimal Amount {get;private set;}
        [JsonProperty("avg_price")]
        public decimal AvgPrice {get;private set;}
    }
}