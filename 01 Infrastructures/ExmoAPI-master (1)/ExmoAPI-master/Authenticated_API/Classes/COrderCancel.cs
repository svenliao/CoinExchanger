using System;
using ExmoAPI.Authenticated_API.Interfeces;
using Newtonsoft.Json;

namespace ExmoAPI.Authenticated_API.Classes
{
    public class COrderCancel : IOrderCancel
    {
        [JsonProperty("result")]
        public bool Result { get; private set; }
        [JsonProperty("error")]
        public string Error { get; private set; }
    }
}