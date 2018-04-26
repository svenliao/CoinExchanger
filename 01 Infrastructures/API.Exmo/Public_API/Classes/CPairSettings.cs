using ExmoAPI.Public_API.Interfaces;
using Newtonsoft.Json;

namespace ExmoAPI.Public_API.Classes
{
    public class CPairSettings : IPairSettings
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