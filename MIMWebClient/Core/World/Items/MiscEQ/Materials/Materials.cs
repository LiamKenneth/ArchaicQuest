using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.MiscEQ.Materials
{
    public class Materials
    {
        public static Item.Item CopperOre()
        {
            var CopperOre = new Item.Item
            {
                name = "Copper Ore",
                Weight = 2,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "A lump of copper ore",
                },
                Gold = 25

            };

            return CopperOre;
        }
    }
}