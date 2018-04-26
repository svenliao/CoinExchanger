using System.Collections.Generic;

namespace ExmoAPI.Public_API.Interfaces
{
    //Книга ордеров по валютной паре
    public interface IOrderBook
    {
        decimal AskQuantity { get; }
        decimal AskAmount { get; }
        decimal AskTop { get; }
        decimal BidQuantity { get; }
        decimal BidAmount { get; }
        decimal BidTop { get; }
        List<List<decimal>> Ask { get; }
        List<List<decimal>> Bid { get; }
    }
}