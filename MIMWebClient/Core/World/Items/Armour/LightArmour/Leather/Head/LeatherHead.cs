using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Head
{
    public class LeatherHead
    {
        public static Item.Item LeatherHelmet()
        {

            var leatherHelmet = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Head,
                description = new Description()
                {
                    look = "A basic looking leather helmet.",
                    exam = "A basic looking leather helmet.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Head,
                type = Item.Item.ItemType.Armour,
                name = "Simple leather helmet",
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
                    "helmet",
                    "leather helmet"
                }

            };


            return leatherHelmet;
        }


    }
}