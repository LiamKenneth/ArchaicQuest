using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Axe
{
    public class AxeBasic
    {
        public static Item.Item IronHatchet()
        {
            var IronHatchet = new Item.Item
            {
                name = "Simple Iron Hatchet",
                Weight = 5,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.ShortBlades,
                attackType = Item.Item.AttackType.Pierce,
                stats = new Stats()
                {
                    damMax = 8,
                    damMin = 4,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A small iron hatchet, with a crudely sharpened metal head and a sturdy wooden handle. ",
                    look = "A small iron hatchet, with a crudely sharpened metal head and a sturdy wooden handle. ",
                    room = "A simple Iron Hatchet",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 35

            };

            return IronHatchet;
        }
    }
}