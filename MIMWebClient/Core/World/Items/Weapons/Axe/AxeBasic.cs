using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Axe
{
    public class AxeBasic
    {

        public static Item.Item CopperAxe()
        {
            var CopperAxe = new Item.Item
            {
                name = "Copper Axe",
                Weight = 5,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 4,
                weaponType = Item.Item.WeaponType.Axe,
                attackType = Item.Item.AttackType.Chop,
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
                    look = "A simple copper half bladed axe.",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 25

            };

            return CopperAxe;
        }

        public static Item.Item IronHatchet()
        {
            var IronHatchet = new Item.Item
            {
                name = "Simple Iron Axe",
                Weight = 5,
                keywords = new List<string>()
                {
                    "Axe"
                },
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 1,
                weaponType = Item.Item.WeaponType.Axe,
                attackType = Item.Item.AttackType.Chop,
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
                    exam = "A small iron hatchet, with a crudely sharpened metal head and a sturdy wooden handle. ",
                    look = "A small iron hatchet, with a crudely sharpened metal head and a sturdy wooden handle. ",
                    room = "A simple Iron Axe",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 35

            };

            return IronHatchet;
        }

        public static Item.Item DoubleAxe()
        {
            var DoubleAxe = new Item.Item
            {
                name = "Double sided Axe",
                Weight = 5,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 3,
                weaponType = Item.Item.WeaponType.Axe,
                attackType = Item.Item.AttackType.Chop,
                stats = new Stats()
                {
                    damMax = 10,
                    damMin = 1,
                    damRoll =1,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A Double sided Axe",
                    look = "A Double sided Axe",
                    room = "A Double sided Axe",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 40

            };

            return DoubleAxe;
        }
    }
}