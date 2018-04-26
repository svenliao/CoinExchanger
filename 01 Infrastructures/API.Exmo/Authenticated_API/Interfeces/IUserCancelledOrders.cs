namespace ExmoAPI.Authenticated_API.Interfeces
{
    //Получение отмененных ордеров пользователя
    public interface IUserCancelledOrders
    {
        ulong Date { get;}
        decimal OrderId { get; }
        string OrderType { get; }
        string TradeCouples { get; }
        decimal Price { get;}
        decimal Quantity { get;}
        decimal Amount { get;}
    }
}