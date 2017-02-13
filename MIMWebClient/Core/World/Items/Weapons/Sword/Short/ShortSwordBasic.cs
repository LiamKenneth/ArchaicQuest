using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Sword.Short
{
    public class ShortSwordBasic
    {
        public static Item.Item ShortIronSword()
        {
            var ShortIronSword = new Item.Item
            {
                name = "Short Iron Sword",
                Weight = 3,
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
                    exam = "This short sword is a dual-edged smooth blade right down to the black handle that holds it",
                    look = "This is a simple short iron sword",
                    room = "A short iron sword",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 35

            };

            return ShortIronSword;
        }
    }
}