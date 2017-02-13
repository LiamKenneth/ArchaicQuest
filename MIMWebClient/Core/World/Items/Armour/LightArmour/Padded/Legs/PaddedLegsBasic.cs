using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Legs
{
    public class PaddedLegsBasic
    {
        public static Item.Item PaddedGreaves()
        {
            var paddedGreaves = new Item.Item
            {
                name = "Padded Leather Greaves",
                Weight = 6,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Legs,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                description = new Description()
                {
                    exam = "These padded greaves offer basic protection to the legs",
                    look = "These padded greaves offer basic protection to the legs",
                    room = "Padded leather greaves lay here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return paddedGreaves;
        }
    }
}