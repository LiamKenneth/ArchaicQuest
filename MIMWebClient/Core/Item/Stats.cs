using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    public class Stats
    {
        public bool locked { get; set; }
        public int damMin { get; set; }
        public int damMax { get; set; }
        public int damRoll { get; set; }
        public int minUsageLevel { get; set; }
        public int worth { get; set; }
    }
}
