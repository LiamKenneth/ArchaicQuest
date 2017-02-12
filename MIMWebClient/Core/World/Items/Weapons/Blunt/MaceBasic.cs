using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.Blunt
{
    public class MaceBasic
    {
        public Item.Item IronMace()
        {
            var ironMace = new Item.Item
            {
                name = "Simple Iron Mace",
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
                    damMax = 8,
                    damMin = 3,
                    damRoll = 0,
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This mace has an iron handle with a leather grip. The iron handle extends up housing a circular ball of metal covered in spikes ",
                    look = "A circular ball of metal covered in spikes sits upon this metal pole",
                    room = "A Simple Iron Mace",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 30 

            };

            return ironMace;
        }
    }
}