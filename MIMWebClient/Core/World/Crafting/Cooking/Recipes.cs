using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Cooking
{
    public class Recipes
    {
 
 
        public static Craft SmokedChub()
        {
            
            var SmokedChub  = new Craft()
            {
                Name = "Smoked Chub ",
                StartMessage = "You take out a dagger and begin cutting the fish.",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "You have cooked smoked chub.",
                Description = "To cook a smoked chub you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                { 
                    "You take out the insides of the fish.",
                    "You slide the fish on to your dagger and put it over the heat and smoke.",
                    "You continue to turn the dagger and fish in the smoke, cooking it evenly"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Chub",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Smoked Chub",

                    description = new Description()
                    {
                        look = "A Smoked Chub"
                    },
                    location = Item.Item.ItemLocation.Inventory
                   


                },
                MoveCost = 20

            };

            return SmokedChub;

        }

        public static Craft BoiledCarp()
        {

            var BoiledCarp = new Craft()
            {
                Name = "Boiled Carp",
                StartMessage = "You take out a dagger and begin cutting the fish.",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "You have boild carp and potatoes.",
                Description = "You have boild carp and potatoes",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You take out the insides of the fish and slice up the potatoes.",
                    "You put the fish and patatoes into the boiling pot of water.",
                    "You continue to stir the pot."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Carp",
                        Count = 1
                    },
                     new CraftMaterials()
                    {
                        Name = "Potato",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Boild Carp with Potatoes",

                    description = new Description()
                    {
                        look = "Boild Carp with Potatoes"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return BoiledCarp;
            
        }

        public static Craft PeasantStew()
        {

            var PeasantStew = new Craft()
            {
                Name = "Peasant Stew",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook a Peasant Stew you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Potato",
                        Count = 1
                    },
                     new CraftMaterials()
                    {
                        Name = "Carrot",
                        Count = 1
                    },

                    new CraftMaterials()
                    {
                        Name = "Garlic",
                        Count = 1
                    },
                     new CraftMaterials()
                    {
                        Name = "Onion",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Peasant Stew",

                    description = new Description()
                    {
                        look = "Peasant Stew"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return PeasantStew;

        }

        public static Craft SeasonedBream()
        {

            var SeasonedBream = new Craft()
            {
                Name = "Seasoned Bream",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook a Peasant Stew you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Bream",
                        Count = 1
                    },
                     new CraftMaterials()
                    {
                        Name = "Garlic",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Seasoned Bream",

                    description = new Description()
                    {
                        look = "Seasoned Bream"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return SeasonedBream;

        }

        public static Craft FishStew()
        {

            var FishStew = new Craft()
            {
                Name = "Fish Stew",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook a Fish Stew you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Perch",
                        Count = 1
                    },
                     new CraftMaterials()
                    {
                        Name = "Onion",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Fish Stew",

                    description = new Description()
                    {
                        look = "Fish Stew"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return FishStew;

        }

        public static Craft Bread()
        {

            var Bread = new Craft()
            {
                Name = "Bread",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook bread you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Flour",
                        Count = 1
                    } 
                },
                CreatesItem = new Item.Item()
                {

                    name = "Fresh bread",

                    description = new Description()
                    {
                        look = "Fresh bread"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return Bread;

        }

        public static Craft FriedTrout()
        {

            var FriedTrout = new Craft()
            {
                Name = "Fried Trout",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook Fried Trout you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Brown Trout",
                        Count = 1
                    },
                         new CraftMaterials()
                    {
                        Name = "Flour",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Fried Trout",

                    description = new Description()
                    {
                        look = "Fried Trout"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return FriedTrout;

        }

        public static Craft TurtleSoup()
        {

            var TurtleSoup = new Craft()
            {
                Name = "Turtle Soup",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook Turtle Soup you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Snapping Turtle",
                        Count = 1
                    },
                         new CraftMaterials()
                    {
                        Name = "Tomato",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Turtle Soup ",

                    description = new Description()
                    {
                        look = "Turtle Soup "
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return TurtleSoup;

        }

        public static Craft FriedEel()
        {

            var FriedEel = new Craft()
            {
                Name = "Fried Eel",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook Fried Eel you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Eel",
                        Count = 1
                    },
                         new CraftMaterials()
                    {
                        Name = "Flour",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Fried Eel",

                    description = new Description()
                    {
                        look = "Fried Eel"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return FriedEel;

        }

        public static Craft FrogLegs()
        {

            var FrogLegs = new Craft()
            {
                Name = "Frog Legs",
                StartMessage = "start message",
                FailureMessages = new List<CraftFailMessage>()
                {
                    new CraftFailMessage()
                    {
                        Message =  "",
                    },

                },
                SuccessMessage = "success message",
                Description = "To cook Fried Eel you need a pot over the fire and a chub.",
                CraftCommand = CraftType.Cook,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "Step 1",
                    "Step 2.",
                 },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Frog",
                        Count = 1
                    },
                         new CraftMaterials()
                    {
                        Name = "Flour",
                        Count = 1
                    },
                            new CraftMaterials()
                    {
                        Name = "Tomato",
                        Count = 1
                    },
                         new CraftMaterials()
                    {
                        Name = "Garlic",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Frog Legs",

                    description = new Description()
                    {
                        look = "Frog Legs"
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return FrogLegs;

        }
    }
}