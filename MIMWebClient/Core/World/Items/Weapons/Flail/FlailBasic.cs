using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Flail
{
    public class FlailBasic
    {

        public static Item.Item CopperFlail()
        {
            var CopperFlail = new Item.Item
            {
                name = "Copper two headed flail",
                Weight = 5,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 4,
                weaponType = Item.Item.WeaponType.Flail,
                attackType = Item.Item.AttackType.Crush,
                stats = new Stats()
                {
                    damMax = 8,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 5,
                    worth = 10
                },
                description = new Description()
                {
                    look = "A simple copper two headed flail.",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return CopperFlail;
        }

      
    }
}