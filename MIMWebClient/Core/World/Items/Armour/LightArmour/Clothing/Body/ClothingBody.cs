using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Clothing.ClothingBody
{
    public class ClothingBody
    {
        public static Item.Item PlainTop()
        {

            var plainTop = new Item.Item
            {
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Body,
                description = new Description()
                {
                    look =
                        "This dull looking top is made out of low quality cotton. The only protecction it provides is from the elements.",
                    exam =
                        "This top looks like it has had many previouse owners it's a dull yellow stained beige shirt.",
                    smell = "The top doesn't smell clean or pleasant.",
                    room = "A top has been left crumpled on the floor",
                    taste = "You wouldn't dare put this horrible looking rag in your mouth.",
                    touch = "Despite being well worn the cotton is still rather soft and warm."
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Body,
                type = Item.Item.ItemType.Armour,
                name = "A plain top",
                ArmorRating = new ArmourRating()
                {
                    Armour = 1,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 0.2,
                equipable = true

            };


            return plainTop;
        }
    }
}