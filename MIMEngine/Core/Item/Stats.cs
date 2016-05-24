using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Item
{
    public class Stats
    {
        public int damMin { get; set; }
        public int damMax { get; set; }
        public bool poisoned { get; set; }
        public bool flaming { get; set; }
        public bool frozen { get; set; }
        public bool acid { get; set; }
        public bool lightning { get; set; }
        public bool cursed { get; set; }
        public bool blessed { get; set; }
        public bool invisible { get; set; }
        public int enchantedLevel { get; set; }
        public int minUsageLevel { get; set; }
        public int worth { get; set; }
    }
}
