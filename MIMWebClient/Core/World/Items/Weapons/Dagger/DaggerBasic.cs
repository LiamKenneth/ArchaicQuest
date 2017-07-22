using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.DaggerBasic
{
    public class DaggerBasic
    {

        public static Item.Item IronDagger()
        {
            var ironDagger = new Item.Item
            {
                name = "Simple Iron Dagger",
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
                    damMax = 4,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This is a small simple iron hunting knife. The dark iron blade is partly serrated, and the wooden handle curves inward for a better grip. ",
                    look = "This is a small simple iron hunting knife. The dark iron blade is partly serrated, and the wooden handle curves inward for a better grip. ",
                    room = "A small serrated iron blade",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20
                
            };

            return ironDagger;
        }

        public static Item.Item HuntingKnife()
        {
            var HuntingKnife = new Item.Item
            {
                name = "Hunting Knife",
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
                    damMax = 6,
                    damMin = 1,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 10
                },
                description = new Description()
                {
                    exam = "A hunting knife",
                    look = "A hunting knife",
                    room = "A hunting knife",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return HuntingKnife;
        }

        public static Item.Item HiddenBlade()
        {
            var HiddenBlade = new Item.Item
            {
                name = "Hidden Blade",
                Weight = 4,
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
                    damMax = 6,
                    damMin = 1,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 10
                },
                description = new Description()
                {
                    exam = "A Hidden Blade",
                    look = "A Hidden Blade",
                    room = "A Hidden Blade",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 120

            };

            return HiddenBlade;
        }

        public static Item.Item BronzeCurvedDagger()
        {
            var BronzeCurvedDagger = new Item.Item
            {
                name = "A bronze curved dagger",
                Weight = 4,
                count = 1,
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
                    damMin = 1,
                    damRoll = 1,
                    minUsageLevel = 1,
                    worth = 15
                },
                description = new Description()
                {
                    exam = "A bronze curved dagger",
                    look = "A bronze curved dagger",
                    room = "A bronze curved dagger",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 120

            };

            return BronzeCurvedDagger;
        }

    }
}
