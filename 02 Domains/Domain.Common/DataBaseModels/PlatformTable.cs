using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Domain.Common.DataBaseModels
{
    public class PlatformTable
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}