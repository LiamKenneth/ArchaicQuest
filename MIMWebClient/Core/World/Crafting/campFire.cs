using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Cooking;

namespace MIMWebClient.Core.World.Crafting
{
    public class Crafting
    {
        public static List<Craft> CraftList()
        {
            var listOfCrafts = new List<Craft>()
            {
                CampFire(),
                PineLog(),
                Recipes.SmokedChub(),
                Recipes.BoiledCarp(),
                Recipes.Bread(),
                Recipes.FishStew(),
                Recipes.FriedEel(),
                Recipes.FriedTrout(),
                Recipes.FrogLegs(),
                Recipes.PeasantStew(),
                Recipes.SeasonedBream(),
                Recipes.SmokedChub(),
                Recipes.TurtleSoup(),
                Alchemy.Alchemy.BurnCream(),
                Alchemy.Alchemy.Antibiotic(),
                Alchemy.Alchemy.Antiseptic(),
                Alchemy.Alchemy.Antivenom(),
                Alchemy.Alchemy.LavenderPerfume(),
                Smithing.Smithing.Lantern(),
                Smithing.Smithing.CopperSword(),
                Smithing.Smithing.CopperAxe(),
                Smithing.Smithing.CopperDagger(),
                Smithing.Smithing.CopperFlail(),
                Smithing.Smithing.CopperMace(),
                Carve.Carve.WoodenRaft(),
                Carve.Carve.WoodenTorch(),
                Carve.Carve.WoodenChest(),
                Knitting.Knitting.WoolenClothHelmet(),
                Knitting.Knitting.WoolenClothBoots(),
                Knitting.Knitting.WoolenClothGloves(),
                Knitting.Knitting.WoolenClothLeggings(),
                Knitting.Knitting.WoolenClothShirt(),
                Knitting.Knitting.WoolenClothSleeves()
            };

            return listOfCrafts;
        }

        public static Craft PineLog()
        {

            var pinelog = new Craft()
            {
                Name = "wood",
                StartMessage = "You grip the shaft of your axe, ready to chop the felled tree into a smaller chunk.",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "You swing the axe down with all your might but miss the wood."
                    },
                      new CraftFailMessage()
                    {
                        Message =  "You miss hit the wood casuing it to split.",
                    }
                },
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

        public static Craft FishingRod()
        {

            var fishingRod = new Craft()
            {
                Name = "Fishing Rod",
                StartMessage = "You take the log and begin carving",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "The wood splits and cracks as you attempt to shape it into a pole.",
                        BreakMaterial = true
                    },
                      new CraftFailMessage()
                    {
                        Message =  "You apply too much force an the wood snaps in two",
                        BreakMaterial = true
                    },
                      new CraftFailMessage()
                    {
                        Message =  "You discard your attempt at a fishing pole in the corner of the room.",
                    }
                },
                SuccessMessage = "You have crafted a fishing rod!",
                Description = "To make a fishing rod you need to be at a work bench.",
                CraftCommand = CraftType.Chop,
                CraftAppearsInRoom = true,
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Count = 6,
                        Name = "Log"
                    }
                },
                CraftingEmotes = new List<string>()
                {
                    "You carve the log into a long wooden pole.",
                    "You rub the pole, so it's smooth to touch all over",

                },
                CreatesItem = new Item.Item()
                {
                    name = "basic fishing rod",
                    location = Item.Item.ItemLocation.Inventory,
                    slot = Item.Item.EqSlot.Held,
                    eqSlot = Item.Item.EqSlot.Held,
                    description = new Description()
                    {
                        look = "This is an old long wooden fishing rod, looks to be well used. There have been other methods for catching fish, though the use of a rod like this one is the tried and tested, and most successful, method.",

                    }

                },
                MoveCost = 20

            };

            return fishingRod;

        }

        public static Craft CampFire()
        {

            var CampFire = new Craft()
            {
                Name = "Camp Fire",
                Duration = 6,
                StartMessage = "You begin to place the logs in position.",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "As soon as the flame appears, a gust of wind blows it out.",
                    },
                      new CraftFailMessage()
                    {
                        Message =  "You fail to get a spark.",
                    } 
                },
            
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