using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class BookingTable
    {
        public long ID { get; set; }
        public PlatformTable Platform { get; set; }
        public string Pair { get; set; }

        public string Coin { get; set; }
        public string Currency { get; set; }
        public string Asks { get; set; }
        public string Bids { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
