using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataBaseModels
{
    public class AccountTable
    {
        public long ID { get; set; }   
        public string UID { get; set; }
        public PlatformTable Platform { get; set; }
        public int ApiVersion { get; set; }
        public string Secret { get; set; }
        public string Key { get; set; }
        public int Default { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
