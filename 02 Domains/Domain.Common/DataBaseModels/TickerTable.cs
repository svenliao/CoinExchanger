using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class TickerTable
    {
        public long ID { get; set; }

        public string Pair { get; set; }

        public string Coin { get; set; }
        public string Currency { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
