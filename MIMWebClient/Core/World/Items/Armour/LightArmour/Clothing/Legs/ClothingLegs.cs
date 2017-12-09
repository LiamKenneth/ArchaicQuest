using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Legs
{
    public class ClothingLegs
    {

        public static Item.Item PlainTrousers()
        {

            var plainTrousers = new Item.Item
            {
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Legs,
                description = new Description()
                {
                    look =
                        "This pair of trousers looks very cheap made out of low quality cotton. The only protection they provide is from the elements.",
                    exam =
                        "This top looks like it has had many previouse owners it's a dull yellow stained pair of trousers.",
                    smell = "The trousers don't smell clean or pleasant.",
                    room = "A pair of trousers has been left crumpled on the floor",
                    taste = "You wouldn't dare put these horrible looking rags in your mouth.",
                    touch = "Despite being well worn the cotton is still rather soft and warm."
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                name = "Pair of plain trousers",
                ArmorRating = new ArmourRating()
                {
                    Armour = 1,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 2,
                equipable = true

            };

            return plainTrousers;
        }



        public static Item.Item WoolenClothLeggings()
        {

            return new Item.Item
            {
                name = "Woolen Cloth Leggings",
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Legs,
                description = new Description()
                {
                    look = "This is a simple woolen leggings which provides basic protection and warmth.",
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                Weight = 2,
                equipable = true,
                stats = new Stats()
                {
                    minUsageLevel = 5
                }

            };

        }
    }
}