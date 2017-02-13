using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Arms
{
    public class PaddedSleeves
    {
        public static Item.Item PaddedSleeve()
        {
            var paddedSleeves = new Item.Item
            {
                name = "Padded Leather Sleeves",
                Weight = 8,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Arms,
                slot = Item.Item.EqSlot.Arms,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                description = new Description()
                {
                    exam = "These padded sleeves offer basic protection to the arms",
                    look = "These padded sleeves offer basic protection to the arms",
                    room = "Padded leather gloves lay here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return paddedSleeves;
        }
    }
}