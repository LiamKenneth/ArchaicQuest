using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World.Items.MiscEQ.Containers
{
    public class Containers
    {
        public static Item.Item LeatherBackpack()
        {
            var backpack = new Item.Item
            {
                name = "Leather Backpack",
                container = true,
                containerSize = 30,
                Weight = 1,
                count = 10,
                equipable = false,
                location = Item.Item.ItemLocation.Inventory,
                canOpen = true,
                Gold = 25
            };

            return backpack;
        }

    }
}