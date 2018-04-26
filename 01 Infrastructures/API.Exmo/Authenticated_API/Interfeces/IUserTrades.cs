namespace ExmoAPI.Authenticated_API.Interfeces
{
    //Получение сделок пользователя
    public interface IUserTrades
    {
        decimal TradeId { get;}
        ulong  Date { get;}
        string Type { get; }
        string TradeCouples { get; }
        decimal OrderId { get; }
        decimal Quantity { get; }
        decimal Price { get; }
        decimal Amount { get; }
    }
}