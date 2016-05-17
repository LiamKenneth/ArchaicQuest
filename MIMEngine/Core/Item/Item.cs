using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Item
{
    public class Item
    {
        public string type { get; set; }
        public string location { get; set; }
        public bool equipable { get; set; }
        public string slot { get; set; }
        public string name { get; set; }
        public Action actions { get; set; }
        public Description description { get; set; }
        public Stats stats { get; set; }
    }
}
