using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Weapons.DaggerBasic
{
    public class DaggerBasic
    {

        public Item.Item IronDagger()
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

    }
}
