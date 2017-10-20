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
                CampFire(),
                PineLog()
            };

            return listOfCrafts;
        }

        public static Craft PineLog()
        {

            var pinelog = new Craft()
            {
                Name = "wood",
                StartMessage = "You grip the shaft of your axe, ready to chop the felled tree into a smaller chunk.",
                FaliureMessage = "...",
                SuccessMessage = "You have chopped a pine log from a felled tree",
                Description = "To make a log you need to be at a chopping block.",
                CraftCommand = CraftType.Chop,
                CraftAppearsInRoom = true,
                CraftingEmotes = new List<string>()
                {
                    "You raise the axe above your head.",
                    "You swing the axe down with all your might. *CRACK* A log lands heavy on the floor."
                },
                CreatesItem = new Item.Item()
                {

                    name = "Pine log",
               
                    description = new Description()
                    {
                        look = "A pine log"
                    },
                    location = Item.Item.ItemLocation.Room


                },
                MoveCost = 20

            };

            return pinelog;

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
                        Name = "Log"
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
                    location = Item.Item.ItemLocation.Room,
                    Duration = 6


                },
                MoveCost = 10

            };

            return CampFire;

        }
    }
}