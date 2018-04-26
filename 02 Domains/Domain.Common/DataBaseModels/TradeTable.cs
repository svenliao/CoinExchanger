using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class TradeTable
    {
        public long ID { get; set; }
        public PlatformTable Platform { get; set; }
        public string TransactionID { get; set; }
        public DateTime Executed { get; set; }
        public string OrderTxid { get; set; }
        public string Status { get; set; }
        public double AvgPrice { get; set; }
        public double Volume { get; set; }
        public double Cost { get; set; }
        public double Fee { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
