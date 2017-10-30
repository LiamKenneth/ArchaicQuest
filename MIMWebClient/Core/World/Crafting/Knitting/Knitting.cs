using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Knitting
{
    public class Knitting
    {
 
 
        public static Craft WoolenClothHelmet()
        {
            
            var WoolenClothHelmet = new Craft()
            {
                Name = "Woolen Cloth Helmet",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted a woolen cloth helmet.",
                Description = "To knit a woolen cloth helmet you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast of the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    },
                      new CraftMaterials()
                    {
                        Name = "Knitting needles",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                     //TODO add armor


                },
                MoveCost = 20

            };

            return WoolenClothHelmet;

        }
 
 
    }
}