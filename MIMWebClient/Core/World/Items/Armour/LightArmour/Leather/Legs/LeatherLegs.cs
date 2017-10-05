using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Legs
{
    public class LeatherLegs
    {
        public static Item.Item LeatherLeggings()
        {

            var leatherLegs = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Legs,
                description = new Description()
                {
                    look = "A basic pair of leather leggings.",
                    exam = "A basic pair of leather leggings",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                name = "Basic pair of leather leggings",
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
                    "leggings",
                    "leather leggings"
                }

            };


            return leatherLegs;
        }


    }
}