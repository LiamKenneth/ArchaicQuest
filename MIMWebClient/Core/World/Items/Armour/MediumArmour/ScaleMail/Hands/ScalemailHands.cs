using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Hands
{
    public class ScalemailHands
    {
        public static Item.Item ScalemailGauntlets()
        {
            var ScalemailGauntlets = new Item.Item
            {
                name = "Scalemail Gauntlets",
                Weight = 4,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Hands,
                slot = Item.Item.EqSlot.Hands,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 1,

                },
                description = new Description()
                {
                    exam = "Scalemail Gauntlets",
                    look = "Scalemail Gauntlets",
                    room = "Scalemail Gauntlets",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 60

            };

            return ScalemailGauntlets;
        }
    }
}