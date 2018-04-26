using System.Collections.Generic;
using ExmoAPI.Public_API.Interfaces;
using Newtonsoft.Json;

namespace ExmoAPI.Public_API.Classes
{
    public class COrderBook : IOrderBook
    {
        [JsonProperty("ask_quantity")]
        public decimal AskQuantity { get; private set ; }
        [JsonProperty("ask_amount")]
        public decimal AskAmount { get; private set; }
        [JsonProperty("ask_top")]
        public decimal AskTop { get; private set; }
        [JsonProperty("bid_quantity")]
        public decimal BidQuantity { get; private set; }
        [JsonProperty("bid_amount")]
        public decimal BidAmount { get; private set; }
        [JsonProperty("bid_top")]
        public decimal BidTop { get; private set; }
        [JsonProperty("ask")]
        public List<List<decimal>> Ask { get; private set; }
        [JsonProperty("bid")]
        public List<List<decimal>> Bid { get; private set; }
    }
}