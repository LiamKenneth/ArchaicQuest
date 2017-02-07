using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Head
{
    public class FullPlateHelm
    {
        public static Item.Item HelmOfTyr()
        {

            var HelmOfTyr = new Item.Item
            {
                armourType = Item.Item.ArmourType.PlateMail,
                eqSlot = Item.Item.EqSlot.Head,
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
                slot = Item.Item.EqSlot.Head,
                type = Item.Item.ItemType.Armour,
                name = "Helm of Tyr",
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

            return HelmOfTyr;
        }
    }
}