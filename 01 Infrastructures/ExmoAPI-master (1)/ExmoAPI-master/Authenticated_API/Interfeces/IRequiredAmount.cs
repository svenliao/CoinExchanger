namespace ExmoAPI.Authenticated_API.Interfeces
{
    //Подсчет в какую сумму обойдется покупка определенного кол-ва валюты по конкретной валютной паре
    public interface IRequiredAmount
    {
        decimal Quantity { get;}
        decimal Amount { get; }
        decimal AvgPrice { get; }
    }
}