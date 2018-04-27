using Newtonsoft.Json;

namespace API.Exmo.Model
{
    public class Trades 
    {
        [JsonProperty("trade_id")]
        public int TradeId { get; private set; }
        [JsonProperty("type")]
        public string Type { get ; private set; }
        [JsonProperty("price")]
        public decimal Price { get ; private set ; }
        [JsonProperty("quantity")]
        public double Quantity { get ; private set ; }
        [JsonProperty("amount")]
        public double Amount { get ; private set; }
        [JsonProperty("date")]
        public ulong Date { get ; private set ; }
    }
}
