using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Alchemy

{
    public class Alchemy
    {


        public static Craft BurnCream()
        {

            var burnCream = new Craft()
            {
                Name = "Burn Cream",
                StartMessage = "You put all your incredients on to the table and grab an empty glass",
                FaliureMessage = "...",
                SuccessMessage = "You have brewed burn cream.",
                Description = "To brew burn cream you need to be at an Alchemy bench",
                CraftCommand = CraftType.Brew,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You squeeze the aloe vera into the mortar.",
                    "You add the scabious to the mortar and begin grinding it with the pestle.",
                    "You continue to turn the pestle and crush the scabious and aloe vera together.",
                    "The mixture has now turned clear white and gloopy, you carefully pour it into a flask."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Aloe Vera",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "Scabious",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "Empty flask",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Burn Cream potion",

                    description = new Description()
                    {
                        look = "This liquid is thick and the glass flask cold to touch, a label is has been stuck to the flask which reads use this for burns."
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return burnCream;

        }

        public static Craft Antivenom()
        {

            var Antivenom = new Craft()
            {
                Name = "Anti Venom",
                StartMessage = "You put all your incredients on to the table and grab an empty glass",
                FaliureMessage = "...",
                SuccessMessage = "You have brewed an anti venom potion.",
                Description = "To brew anti venom you need to be at an Alchemy bench",
                CraftCommand = CraftType.Brew,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You squeeze the snake root into the mortar.",
                    "You add the Bay Laurel to the mortar and begin grinding it with the pestle.",
                    "You continue to turn the pestle and crush the snake root and Bay Laurel together.",
                    "The mixture has now turned light green, you carefully pour it into a flask."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Bay Laurel",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "White Snake Root",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "Empty flask",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Anti venom potion",

                    description = new Description()
                    {
                        look = "This liquid is light green, a label is has been stuck to the flask which reads use drink this if poisioned."
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return Antivenom;

        }

        public static Craft LavenderPerfume()
        {

            var LavenderPerfume = new Craft()
            {
                Name = "Lavender Perfume",
                StartMessage = "You put all your incredients on to the table and grab an empty glass",
                FaliureMessage = "...",
                SuccessMessage = "You have brewed Lavender perfume.",
                Description = "To brew Lavender perfume you need to be at an Alchemy bench",
                CraftCommand = CraftType.Brew,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You add the Lavender to the mortar and begin grinding it with the pestle.",
                    "You continue to turn the pestle and crush the Lavender.",
                    "The mixture has now turned light purple, you carefully pour it into a flask."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Lavedar",
                        Count = 4
                    },
                    new CraftMaterials()
                    {
                        Name = "Empty flask",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Lavendar Perfume",

                    description = new Description()
                    {
                        look = "This liquid is light purple, a label is has been stuck to the flask which reads use to smell nice."
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return LavenderPerfume;

        }

        public static Craft Antibiotic()
        {

            var Antibiotic = new Craft()
            {
                Name = "Antibiotic",
                StartMessage = "You put all your incredients on to the table and grab an empty glass",
                FaliureMessage = "...",
                SuccessMessage = "You have brewed an Antibiotic potion",
                Description = "To brew an Antibiotic potion you need to be at an Alchemy bench",
                CraftCommand = CraftType.Brew,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You add the garlic to the mortar and begin grinding it with the pestle.",
                    "You continue to turn the pestle and crush the garlic.",
                    "You add the bay leaves to the mortar and begin grinding it with the pestle.",
                    "The mixture has now turned light green, you carefully pour it into a flask."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Garlic",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "Bay Laurel",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "Empty flask",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Antibiotic",

                    description = new Description()
                    {
                        look = "This liquid is light green, a label is has been stuck to the flask which reads use to rid disease."
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return Antibiotic;

        }

        public static Craft Antiseptic()
        {

            var Antiseptic = new Craft()
            {
                Name = "Antiseptic",
                StartMessage = "You put all your incredients on to the table and grab an empty glass",
                FaliureMessage = "...",
                SuccessMessage = "You have brewed an Antiseptic potion",
                Description = "To brew an Antiseptic potion you need to be at an Alchemy bench",
                CraftCommand = CraftType.Brew,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You add the Rosemary to the mortar and begin grinding it with the pestle.",
                    "You continue to turn the pestle and crush the Rosemary.",
                    "The mixture has now turned light green, you carefully pour it into a flask."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Rosemary",
                        Count = 4
                    },
                    new CraftMaterials()
                    {
                        Name = "Empty flask",
                        Count = 1
                    }

                },
                CreatesItem = new Item.Item()
                {

                    name = "Antiseptic",

                    description = new Description()
                    {
                        look = "This liquid is light green, a label is has been stuck to the flask which reads use to rid of poison."
                    },
                    location = Item.Item.ItemLocation.Inventory



                },
                MoveCost = 20

            };

            return Antiseptic;

        }
    }
}