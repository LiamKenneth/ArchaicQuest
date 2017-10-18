using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Crafting
{
    public class Crafting
    {
        public static List<Craft> CraftList()
        {
            var listOfCrafts = new List<Craft>()
            {
                CampFire()
            };

            return listOfCrafts;
        }



        public static Craft CampFire()
        {

            var CampFire = new Craft()
            {
                Name = "Camp Fire",
                Duration = 6,
                StartMessage = "You begin to place the logs in position.",
                FaliureMessage = "As soon as the flame appears, a gust of wind blows it out.",
                SuccessMessage = "The camp fire bursts to life.",
                Description = "To build a camp fire you require 4 logs.",
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Count = 4,
                        Name = "Logs"
                    }
                },
                CraftAppearsInRoom = true,
                CraftingEmotes = new List<string>()
                {
                    "You attempt to heat up the tinder"
                },
                CreatesItem = new Item.Item()
                {
                    name = "Camp Fire",
                    stuck = true,
                    description = new Description()
                    {
                        look = "A fire!"
                    },
                    location = Item.Item.ItemLocation.Room


                },
                
            };

            return CampFire;

        }
    }
}