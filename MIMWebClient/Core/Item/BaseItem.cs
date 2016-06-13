using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    public class BaseItem
    {
        public string name { get; set; }
        public List<string> keywords { get; set; }
        public bool locked { get; set; }
        public int? keyValue { get; set; }
        public bool hidden { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public bool equipable { get; set; }
        public string slot { get; set; }

        public Action actions { get; set; }
        public Description description { get; set; }
        public Stats stats { get; set; }

    }
}
