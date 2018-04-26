namespace ExmoAPI.Public_API.Interfaces
{
    //Настройки валютных пар
    public interface IPairSettings
    {
        decimal MinQuantity { get;  }
        decimal MaxQuantity { get; }
        decimal MinPrice { get; }
        decimal MaxPrice { get; }
        decimal MaxAmount { get; }
        decimal MinAmount { get; }
    }
}