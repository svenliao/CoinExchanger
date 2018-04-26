using ExmoAPI.Public_API.Interfaces;
using Newtonsoft.Json;

namespace ExmoAPI.Public_API.Classes
{
    public class CTicker : ITicker
    {
        [JsonProperty("buy_price")]
        public decimal BuyPrice { get  ; private set  ; }
        [JsonProperty("sell_price")]
        public decimal SellPrice { get  ; private set  ; }
        [JsonProperty("last_trade")]
        public decimal LastTrade { get  ; private set  ; }
        [JsonProperty("high")]
        public decimal High { get  ; private set  ; }
        [JsonProperty("low")]
        public decimal Low { get  ; private set; }
        [JsonProperty("avg")]
        public decimal Avg { get  ; private set; }
        [JsonProperty("vol")]
        public decimal Vol { get  ; private set  ; }
        [JsonProperty("vol_curr")]
        public decimal VolCurr { get  ; private set  ; }
        [JsonProperty("updated")]
        public ulong Updated { get  ; private set  ; }
    }
}