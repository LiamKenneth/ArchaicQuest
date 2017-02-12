using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Staff
{
    public class StaffBasic
    {
        public Item.Item WoodenQuarterstaff()
        {
            var WoodenQuarterstaff = new Item.Item
            {
                name = "Wooden Quarterstaff",
                Weight = 1,
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
                    damMax = 5,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This is a long wooden staff built to hit people with or just to help you walk difficult paths, whatever you use it for, have fun.",
                    look = "This is a long wooden staff",
                    room = "A long wooden staff",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return WoodenQuarterstaff;
        }

    }
}