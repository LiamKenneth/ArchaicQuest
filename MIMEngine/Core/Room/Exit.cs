using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Room
{
    public class Exit
    {
        public string name { get; set; }
        public bool locked { get; set; }
        public bool hidden { get; set; }
        public int areaId { get; set; }
        public string area { get; set; }
        public string region { get; set; }

    }
}
