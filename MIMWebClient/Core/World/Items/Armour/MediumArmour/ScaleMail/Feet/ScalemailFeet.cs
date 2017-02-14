using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Feet
{
    public class ScalemailFeet
    {
        public static Item.Item ScalemailBoots()
        {
            var ScalemailBoots = new Item.Item
            {
                name = "Scalemail helm",
                Weight = 4,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Feet,
                slot = Item.Item.EqSlot.Feet,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Boots",
                    look = "Scalemail Boots",
                    room = "Scalemail Boots",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailBoots;
        }
    }
}