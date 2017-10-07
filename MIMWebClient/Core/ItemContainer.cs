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

        public IEnumerable<String> List(Func<Item.Item, bool> predicate)
        {
            return this.List(predicate, false);
        }

        public IEnumerable<String> List(Func<Item.Item, bool> predicate, bool article)
        {
            var list = this.AsEnumerable();
            if(predicate != null) list = list.Where(predicate);

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