using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Body
{
    public class LeatherBody
    {
        public static Item.Item LeatherVest()
        {

            var leatherVest = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Torso,
                description = new Description()
                {
                    look = "A basic looking leather vest.",
                    exam = "A basic looking leather vest.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Torso,
                type = Item.Item.ItemType.Armour,
                name = "Simple leather vest",
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
                    "vest",
                    "leather vest"
                }

            };


            return leatherVest;
        }


    }
}