namespace ExmoAPI.Public_API.Interfaces
{
    //Cтатистика цен и объемов торгов по валютным парам
    public interface ITicker
    {
        decimal BuyPrice { get; }
        decimal SellPrice { get; }
        decimal LastTrade { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Avg { get; }
        decimal Vol { get; }
        decimal VolCurr { get; }
        ulong Updated { get;  }
    }
}