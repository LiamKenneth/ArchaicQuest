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

        public static Item.Item Saber()
        {
            var Saber = new Item.Item
            {
                name = "Saber",
                Weight = 2,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.ShortBlades,
                attackType = Item.Item.AttackType.Slash,
                stats = new Stats()
                {
                    damMax = 8,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A thin light metal saber",
                    look = "A thin light metal saber",
                    room = "A thin light metal saber",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 30

            };

            return Saber;
        }

        public static Item.Item CopperSword()
        {
            var CopperSword = new Item.Item
            {
                name = "Copper short sword",
                Weight = 5,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 4,
                weaponType = Item.Item.WeaponType.ShortBlades,
                attackType = Item.Item.AttackType.Slash,
                stats = new Stats()
                {
                    damMax = 5,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 5,
                    worth = 10
                },
                description = new Description()
                {
                    look = "A simple copper sword with a short blade",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return CopperSword;
        }

        public static Item.Item RustedShortSword()
        {
            var RustedShortSword = new Item.Item
            {
                name = "Rusted Short Sword",
                Weight = 2,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.ShortBlades,
                attackType = Item.Item.AttackType.Slash,
                stats = new Stats()
                {
                    damMax = 3,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A thin rusted short sword",
                    look = "A thin rusted short sword",
                    room = "A thin rusted short sword",
                    smell = "",
                    taste = "",
                    touch = "",
                }

            };

            return RustedShortSword;
        }
    }
}