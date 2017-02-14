using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Head
{
    public class ScalemailHead
    {
        public static Item.Item ScalemailHelm()
        {
            var ScalemailHelm = new Item.Item
            {
                name = "Scalemail helm",
                Weight = 4,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Head,
                slot = Item.Item.EqSlot.Head,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Helm",
                    look = "Scalemail Helm",
                    room = "Scalemail Helm",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailHelm;
        }
    }
}