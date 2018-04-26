using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExmoAPI.Generic
{
    public interface IHelperPublicAPI<T>
    {
        IList<T> ResultList { get; }
        T ResultMetod { get; }
        Task GetResultAsync(string method, ExmoApi api, string tradeCouples=null, int? limit=null);
    }

    public interface IHelperAuthAPI<T>
    {
        IList<T> ResultList { get; }
        T ResultMetod { get; }
        Task GetResultAsync(string method, ExmoApi api, Dictionary<string, string> dic = null, string tradeCouples = "BTC_USD");
    }

}