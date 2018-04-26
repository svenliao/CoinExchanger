namespace ExmoAPI.Authenticated_API.Interfeces
{
    //Отмена ордера
    public interface IOrderCancel
    {
        bool Result { get;}
        string Error { get;}
    }
}