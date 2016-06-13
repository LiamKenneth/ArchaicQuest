using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Room
{
   public class RoomObject
    {
        public string name { get; set; }
        public string look { get; set; }
        public string examine { get; set; }
        public string touch { get; set; }
        public string smell { get; set; }
        public string taste { get; set; }
        public int hp { get; set; }
    }
}
