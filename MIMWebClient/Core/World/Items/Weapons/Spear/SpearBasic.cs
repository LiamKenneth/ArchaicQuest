using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Spear
{
    public class SpearBasic
    {
        public static Item.Item BoarSpear()
        {
            var BoarSpear = new Item.Item
            {
                name = "Boar Spear",
                Weight = 7,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.Spear,
                attackType = Item.Item.AttackType.Pierce,
                stats = new Stats()
                {
                    damMax = 6,
                    damMin = 2,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 10
                },
                description = new Description()
                {
                    exam = "A Boar Spear",
                    look = "A Boar Spear",
                    room = "A Boar Spear",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 50

            };

            return BoarSpear;
        }
    }
}