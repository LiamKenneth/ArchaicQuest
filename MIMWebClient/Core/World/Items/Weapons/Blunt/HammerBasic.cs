using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Blunt
{
    public class HammerBasic
    {
        public static Item.Item GreatHammer()
        {
            var GreatHammer = new Item.Item
            {
                name = "Great Hammer",
                Weight = 20,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                weaponSpeed = 10,
                weaponType = Item.Item.WeaponType.Blunt,
                attackType = Item.Item.AttackType.Crush,
                stats = new Stats()
                {
                    damMax = 16,
                    damMin = 1,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A large metal war hammer",
                    look = "A large metal war hammer",
                    room = "A large metal war hammer",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 70

            };

            return GreatHammer;
        }
    }
}
 