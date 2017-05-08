using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Consumables.Drinks
{
    public class MagicalSpring
    {
        public static Item.Item MagicalWaterSpring()
        {
            var MagicalSpring = new Item.Item
            {
                name = "Magical Spring",
                Weight = 0,
                count = 1,
                equipable = false,
                stuck = true,             
                location = Item.Item.ItemLocation.Room,
                waterContainer = true,
                waterContainerAmount = 999,
                waterContainerMaxAmount = 999,
                description = new Description()
                {
                    exam = "A magicial spring shoots water up in the air.",
                    look = "A magicial spring shoots water up in the air.",
                    room = "A magicial spring shoots water up in the air.",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 0,
                decayTimer = 24

            };

            return MagicalSpring;
        }
    }
}