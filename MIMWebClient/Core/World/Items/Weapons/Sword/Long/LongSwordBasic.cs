using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Sword.Long
{
    public class LongSwordBasic
    {
        public static Item.Item LongIronSword()
        {
            var LongIronSword = new Item.Item
            {
                name = "Long Iron Sword",
                Weight = 12,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 4,
                weaponType = Item.Item.WeaponType.ShortBlades,
                attackType = Item.Item.AttackType.Pierce,
                stats = new Stats()
                {
                    damMax = 12,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This long sword is a dual-edged smooth blade right down to the black handle that holds it",
                    look = "A traditional sharp iron long sword",
                    room = "A long iron sword",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return LongIronSword;
        }

        public static Item.Item BastardSword()
        {
            var BastardSword = new Item.Item
            {
                name = "Bastard Sword",
                Weight = 7,
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
                    damMax = 12,
                    damMin = 1,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 10
                },
                description = new Description()
                {
                    exam = "A large iron sword",
                    look = "A large iron sword",
                    room = "A large iron sword",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 70

            };

            return BastardSword;
        }

        public static Item.Item Katana()
        {
            var Katana = new Item.Item
            {
                name = "Katana",
                Weight = 5,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.LongBlades,
                attackType = Item.Item.AttackType.Slash,
                stats = new Stats()
                {
                    damMax = 8,
                    damMin = 1,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 10
                },
                description = new Description()
                {
                    exam = "A katana",
                    look = "A katana",
                    room = "A katana",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 40

            };

            return Katana;
        }

    }
}