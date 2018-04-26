namespace ExmoAPI.Authenticated_API.Interfeces
{
    public interface IOrderCreate
    {
        bool Result { get;}
        string Error { get; }
        decimal OrderId { get;}
    }
}