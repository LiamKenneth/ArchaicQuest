using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.MiscEQ.Light
{
    public class Light
    {
        public static Item.Item WoodenTorch()
        {
            var woodenTorch = new Item.Item
            {
                name = "Wooden Torch",
                Weight = 1,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Light,
                slot = Item.Item.EqSlot.Light,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This torch looks like a wooden club with rags wrapped around the end and dipped in tar for the flames to last.",
                    look = "A wooden torch with tar covered rags at the end",
                    room = "A small torch",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5

            };

            return woodenTorch;
        }
    }
}