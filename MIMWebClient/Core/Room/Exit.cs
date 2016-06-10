using MIMWebClient.Core.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Room
{
    public class Exit : BaseItem
    {

        public int keyId { get; set; }
        public int areaId { get; set; }
        public string area { get; set; }
        public string region { get; set; }

    }
}
