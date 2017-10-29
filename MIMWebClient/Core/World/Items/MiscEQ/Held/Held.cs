using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.MiscEQ.Held
{
    public class Held
    {
        public static Item.Item TatteredRag()
        {
            var TatteredRag = new Item.Item
            {
                name = "Tattered rag",
                Weight = 1,
                equipable = false,
                eqSlot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "this tattered rag is stained and ripped in multiple places " +
                           " the only use for this stained cloth is to turn it into something else."
                }
            };

            return TatteredRag;
        }

        public static Item.Item ScrapMetal()
        {
            var ScrapMetal = new Item.Item
            {
                name = "Scrap Metal",
                Weight = 1,
                equipable = false,
                eqSlot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "A piece of scrap metal."
                }
            };

            return ScrapMetal;
        }
    }
}