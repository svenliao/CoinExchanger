using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class BalanceTable
    {
        public long ID { get; set; }
        public string UID { get; set; }
        public string Coin { get; set; }

        public double Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
