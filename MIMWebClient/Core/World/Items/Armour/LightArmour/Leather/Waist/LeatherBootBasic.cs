using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Feet
{
    public class LeatherBootBasic
    {
        public static Item.Item WornLeatherBoots()
        {
            var wornLeatherBoots = new Item.Item
            {
                name = "Worn Leather Boots",
                Weight = 2,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Feet,
                slot = Item.Item.EqSlot.Feet,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                description = new Description()
                {
                    exam = "These worn leather boots offer basic protection to the feet",
                    look = "Theseworn leather boots offer basic protection to the feet",
                    room = "Worn leather boots lay here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return wornLeatherBoots;
        }
    }
}