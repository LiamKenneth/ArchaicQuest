using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Body
{
    public class PaddedBodyBasic
    {
        public static Item.Item PaddedBreastPlate()
        {
            var paddedBreastPlate = new Item.Item
            {
                name = "Padded Leather Breastplate",
                Weight = 10,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Torso,
                slot = Item.Item.EqSlot.Torso,
                type = Item.Item.ItemType.Armour,
                location = Item.Item.ItemLocation.Inventory,
              
                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                description = new Description()
                {
                    exam = "Simple leather breastplate is padded to protect the wearers front.",
                    look = "Simple leather breastplate is padded to protect the wearers front.",
                    room = "A padded leather breastplate lays here",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 20

            };

            return paddedBreastPlate;
        }
    }
}