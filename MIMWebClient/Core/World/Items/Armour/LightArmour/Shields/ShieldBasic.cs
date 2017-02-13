using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Shields
{
    public class ShieldBasic
    {

        public static Item.Item WoodenShield()
        {
            var woodenShield = new Item.Item
            {
                name = "Round wooden shield",
                Weight = 3,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Shield,
                slot = Item.Item.EqSlot.Shield,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,

                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 1,
                },
                description = new Description()
                {
                    exam = "This round shield is light weight thanks to it's wooden build, it offers some protection to the wearer even against magic",
                    look = "This round shield is light weight thanks to it's wooden build, it offers some protection to the wearer even against magic",
                    room = "A round wooden shield lays here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return woodenShield;
        }
    }
}