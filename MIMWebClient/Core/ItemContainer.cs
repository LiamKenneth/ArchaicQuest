using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core
{

    public class ItemContainer : List<Item.Item>
    {
        public IEnumerable<String> List()
        {
            return this.List(null, false);
        }

        public IEnumerable<String> List(bool article)
        {
            return this.List(null, article);
        }

        public IEnumerable<String> List(Func<Item.Item, bool> filter)
        {
            return this.List(filter, false);
        }

        public IEnumerable<String> List(Func<Item.Item, bool> filter, bool article)
        {
            var list = this.AsEnumerable();
            if(filter != null) list = list.Where(filter);

            return list.GroupBy(t => t.name).Select(group => new
            {
                Name = group.Key,
                Count = group.Count()
            }).Select(x =>
            {
                var itemString = x.Name + ((x.Count > 1) ? " x" + x.Count : "");
                if (article)
                {
                    var result = AvsAnLib.AvsAn.Query(x.Name);
                    itemString = result.Article + " " + itemString;
                }

                return itemString;
            });
        }
    }
}