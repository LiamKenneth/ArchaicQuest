using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Consumables.Drinks
{
    public class drink
    {
        public static Item.Item WaterSkin()
        {
            var waterSkin = new Item.Item
            {
                name = "Leather Waterskin",
                Weight = 3,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {               
                    minUsageLevel = 1,
                    worth = 3
                },
                waterContainer = true,
                waterContainerAmount = 4,
                description = new Description()
                {
                    exam = "This water skin is a thin hard oval shape covered in leather",
                    look = "This water skin is a thin hard oval shape covered in leather",
                    room = "A small leather water skin",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 10

            };

            return waterSkin;
        }

        public static Item.Item Flask()
        {
           return  new Item.Item
            {
                name = "Empty Flask",
                Weight = 2,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 3
                },
                waterContainer = true,
                waterContainerAmount = 2,
                description = new Description()
                {
                    exam = "This clear flask is empty and can be used to store any liquid.",
                    look = "This clear flask is empty and can be used to store any liquid.",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 10

            };
 
        }
    }
}