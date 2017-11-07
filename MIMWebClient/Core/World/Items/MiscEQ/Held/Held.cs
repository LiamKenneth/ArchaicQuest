using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.MiscEQ.Held
{
    public class Held
    {
        public static Item.Item TatteredRag()
        {
            return new Item.Item
            {
                name = "Tattered rag",
                Weight = 1,
                equipable = false,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "this tattered rag is stained and ripped in multiple places " +
                           " the only use for this stained cloth is to turn it into something else."
                }
            };

        }

        public static Item.Item RepairHammer()
        {

            return new Item.Item
            {
                name = "Repair Hammer",
                Weight = 1,
                type = Item.Item.ItemType.Repair,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "this large square hammer is used to repair wepons or armour that have been damage"
                },
                Uses = 5
                
                

            };
        }

    public static Item.Item ScrapMetal()
        {
            return new Item.Item
            {
                name = "Scrap Metal",
                Weight = 1,
                equipable = false,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description()
                {
                    look = "A piece of scrap metal."
                }
            };
        }
    }
}