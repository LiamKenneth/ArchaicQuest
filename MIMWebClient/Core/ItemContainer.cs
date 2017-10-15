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
            return List(this, false);
        }

        public IEnumerable<String> List(bool article)
        {
            return List(this, article);
        }

        public static IEnumerable<String> List(IEnumerable<Item.Item> items)
        {
            return List(items, false);
        }

        public static IEnumerable<String> List(IEnumerable<Item.Item> items, bool article)
        {
            return items.GroupBy(t => t.name).Select(group => new
            {
                Name = group.Key,
                Count = group.Count()
            }).OrderBy(x => x.Name).Select(x =>
            {
                var itemString = x.Name + ((x.Count > 1) ? " x" + x.Count : "");
                if (article)
                {
                    var result = AvsAnLib.AvsAn.Query(x.Name);
                    itemString = Helpers.FirstLetterToUpper(result.Article) + " " + itemString;
                }

                return itemString;
            });
        }
    }
}