using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Body
{
    public static class RobeBasic
    {
        public static Item.Item TravelerRobe()
        {
            var TravelerRobes = new Item.Item
            {
                name = "Traveler's Robe",
                Weight = 3,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Body,
                slot = Item.Item.EqSlot.Body,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 1,
                    Magic = 3,
                    
                },
                description = new Description()
                {
                    exam = "This robe is made of brown basic materials and covers the entire body. It has a small amount of magical protection.",
                    look = "This robe is made of brown basic materials and covers the entire body. It has a small amount of magical protection.",
                    room = "A brown dirty robe lays here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return TravelerRobes;
        }
    }
}