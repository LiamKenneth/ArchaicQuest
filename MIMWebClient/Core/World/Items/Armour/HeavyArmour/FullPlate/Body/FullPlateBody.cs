using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Body
{
    public class FullPlateBody
    {
        public static Item.Item BreastPlateOfTyr()
        {

            var BreastPlateOfTyr = new Item.Item
            {
                armourType = Item.Item.ArmourType.PlateMail,
                eqSlot = Item.Item.EqSlot.Body,
                description = new Description()
                {
                    look =
                        "This iron breastplate has impressive pectoral muscles embossed on the front and locks in tightly around the wearers shoulders and waist",
                    exam =
                        "This iron breastplate has a blue tinge and shimers slightly at certain angles in the light, it is well crafted as shown by the detailed pectoral muscles embossed on the front. Benedic Tyr has been engraved on the left chest over the heart.",
                    smell = "It doesn't seem to smell",
                    room = "An iron breastplate with a blue tinge lays here on the floor",
                    taste = "It tastes like metal",
                    touch = "It feels cold to touch"
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Body,
                type = Item.Item.ItemType.Armour,
                name = "The breast plate of Tyr",
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

            return BreastPlateOfTyr;
        }
    }
}