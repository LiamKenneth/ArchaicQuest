using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Arms
{
    public class LeatherArms
    {
        public static Item.Item LeatherSleeves()
        {

            var leatherSleeves = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Arms,
                description = new Description()
                {
                    look = "A basic pair of leather sleeves.",
                    exam = "A basic pair of leather sleeves.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Arms,
                type = Item.Item.ItemType.Armour,
                name = "A basic pair of leather sleeves.",
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
                keywords = new List<string>()
                {
                    "leather",
                    "sleeves",
                    "leather sleevs"
                }

            };


            return leatherSleeves;
        }


    }
}