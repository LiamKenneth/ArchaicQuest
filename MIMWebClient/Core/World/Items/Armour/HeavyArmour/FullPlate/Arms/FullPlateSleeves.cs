using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Arms
{
    public class FullPlateSleeves
    {
        public static Item.Item SteelSleevesOfTyr()
        {

            var SteelSleevesOfTyr = new Item.Item
            {
                armourType = Item.Item.ArmourType.PlateMail,
                eqSlot = Item.Item.EqSlot.Arms,
                description = new Description()
                {
                    look =
                        "",
                    exam =
                        "",
                    smell = "It doesn't seem to smell",
                    room = "",
                    taste = "It tastes like metal",
                    touch = "It feels cold to touch"
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Arms,
                type = Item.Item.ItemType.Armour,
                name = "Steel sleeves of Tyr",
                ArmorRating = new ArmourRating()
                {
                    Armour = 20,
                    Magic = 7
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable,
                    Item.Item.ItemFlags.glow,
                    Item.Item.ItemFlags.bless,
                },
                Weight = 15,
                equipable = true

            };

            return SteelSleevesOfTyr;
        }
    }
}