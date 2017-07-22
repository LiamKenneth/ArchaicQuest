using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Hands
{
    public class LeatherHands
    {
        public static Item.Item LeatherGloves()
        {

            var leatherGloves = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Hands,
                description = new Description()
                {
                    look = "A basic pair leather gloves.",
                    exam =
                        "A basic pair leather gloves.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Hands,
                type = Item.Item.ItemType.Armour,
                name = "A basic pair leather gloves",
                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 0.2,
                equipable = true,
                QuestItem = true,
                keywords = new List<string>()
                {
                    "leather",
                    "gloves",
                    "leather gloves"
                }

            };


            return leatherGloves;
        }


    }
}