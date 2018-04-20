using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class OrderTable
    {
        public long ID { get; set; }
        public string OrderTxid { get; set; }
        /// <summary>
        /// Buy/Sell
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Limit/Market
        /// </summary>
        public string LimitType { get; set; }
        public string Status { get; set; }
        public string MainTxid { get; set; }
        public string Pair { get; set; }
        public string Coin { get; set; }
        public string Currency { get; set; }
        public double Volume { get; set; }
        public double LimitPrice { get; set; }
        public double Price { get; set; }
        public DateTime Opened { get; set; }
        public DateTime Closed { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
