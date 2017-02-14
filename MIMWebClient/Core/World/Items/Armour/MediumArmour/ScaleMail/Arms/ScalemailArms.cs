using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Arms
{
    public class ScalemailArms
    {
        public static Item.Item ScalemailSleeves()
        {
            var ScalemailSleeves = new Item.Item
            {
                name = "Scalemail Sleeves",
                Weight = 4,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Arms,
                slot = Item.Item.EqSlot.Arms,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Sleeves",
                    look = "Scalemail Sleeves",
                    room = "Scalemail Sleeves",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailSleeves;
        }
    }
}