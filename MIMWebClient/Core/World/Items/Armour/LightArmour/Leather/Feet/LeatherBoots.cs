using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Feet
{
    public class LeatherFeet
    {
        public static Item.Item LeatherBoots()
        {

            var leatherHelmet = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Legs,
                description = new Description()
                {
                    look = "A basic pair of leather boots.",
                    exam = "A basic pair of leather boots.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Feet,
                type = Item.Item.ItemType.Armour,
                name = "Basic pair of leather boots",
                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 2,
                equipable = true,
                QuestItem = true,
                keywords = new List<string>()
                {
                    "leather",
                    "boots",
                    "leather boots"
                }

            };


            return leatherHelmet;
        }


    }
}