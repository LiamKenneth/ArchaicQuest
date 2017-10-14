using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Body
{
    public class ScalemailBody
    {
        public static Item.Item ScalemailBreastPlate()
        {
            var ScalemailBreastPlate = new Item.Item
            {
                name = "Scalemail Breast Plate",
                Weight = 7,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Torso,
                slot = Item.Item.EqSlot.Torso,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Breast Plate",
                    look = "Scalemail Breast Plate",
                    room = "Scalemail Breast Plate",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailBreastPlate;
        }
    }
}