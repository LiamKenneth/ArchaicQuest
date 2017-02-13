using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Hands
{
    public class PaddedHandsBasic
    {
        public static Item.Item PaddedGloves()
        {
            var paddedGloves = new Item.Item
            {
                name = "Padded Leather Gloves",
                Weight = 3,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Hands,
                slot = Item.Item.EqSlot.Hands,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                description = new Description()
                {
                    exam = "These padded gloves offer basic protection to the hands",
                    look = "These padded gloves offer basic protection to the hands",
                    room = "Padded leather gloves lay here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return paddedGloves;
        }
    }
}