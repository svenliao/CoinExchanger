namespace ExmoAPI.Public_API.Interfaces
{
    // Список сделок по валютной паре
    public interface ITrades
    {
        int TradeId { get;}
        string Type { get; }
        decimal Price { get; }
        double Quantity { get;}
        double Amount { get; }
        ulong Date { get; }
    }
    
}
