using Newtonsoft.Json;

namespace API.Exmo.Model
{
    public class PairSettings 
    {
        [JsonProperty("min_quantity")]
        public decimal MinQuantity { get; private set; }
        [JsonProperty("max_quantity")]
        public decimal MaxQuantity { get; private set; }
        [JsonProperty("min_price")]
        public decimal MinPrice { get; private set; }
        [JsonProperty("max_price")]
        public decimal MaxPrice { get; private set; }
        [JsonProperty("max_amount")]
        public decimal MaxAmount { get; private set; }
        [JsonProperty("min_amount")]
        public decimal MinAmount { get; private set; }
    }
}