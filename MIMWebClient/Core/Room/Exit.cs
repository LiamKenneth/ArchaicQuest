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

        public int areaId { get; set; }
        public string area { get; set; }
        public string region { get; set; }
        public string doorName { get; set; }


        //public override Exit GetHashCode()
        //{
        //    return Exit;
        //}
        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as Exit);
        //}
        //public bool Equals(Exit obj)
        //{
        //    return obj != null && obj.areaId == this.areaId && obj.area == this.area && obj.region == this.region;
        //}
    }
}
