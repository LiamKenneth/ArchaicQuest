using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Consumables.Food
{
    public class Food
    {
        public static Item.Item Cheese()
        {
            var Cheese = new Item.Item
            {
                name = "Cheese",
                Weight = 1,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,             
                stats = new Stats()
                {                 
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This lump of cheese has a strong potent smell",
                    look = "This lump of cheese has a strong potent smell",
                    room = "A lump of stinky cheese",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5

            };

            return Cheese;
        }
    }
}