using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Legs
{
    public class ScalemailLegs
    {
        public static Item.Item ScalemailGreaves()
        {
            var ScalemailGreaves = new Item.Item
            {
                name = "Scalemail Greaves",
                Weight = 4,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Legs,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Legs",
                    look = "Scalemail Legs",
                    room = "Scalemail Legs",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailGreaves;
        }
    }
}