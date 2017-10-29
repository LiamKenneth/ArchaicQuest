using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Consumables.Food
{
    public class Food
    {

        public static Item.Item WhiteSnakeroot()
        {
            var whitesnakeroot = new Item.Item
            {
                name = "White Snakeroot",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A multi headead white flower with spikey petals connected to a large green stem.",
                    look = "A multi headead white flower with spikey petals connected to a large green stem.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 1

            };

            return whitesnakeroot;
        }

        public static Item.Item BayLaurel()
        {
            var BayLaurel = new Item.Item
            {
                name = "Bay Laurel",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A leafy thin branch with cream coloured flowers and flower buds that are yet to open.",
                    look = "A leafy thin branch with cream coloured flowers and flower buds that are yet to open.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 1

            };

            return BayLaurel;
        }

        public static Item.Item Lavender()
        {
            var Lavender = new Item.Item
            {
                name = "Lavender",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "The tip of the Lavender stem is covered in small purple petals.",
                    look = "The tip of the Lavender stem is covered in small purple petals.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 1

            };

            return Lavender;
        }

        public static Item.Item Rosemary()
        {
            var Rosemary = new Item.Item
            {
                name = "Rosemary",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A geen needle like stem with small purple flowers.",
                    look = "A geen needle like stem with small purple flowers.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 1

            };

            return Rosemary;
        }



        public static Item.Item Cheese()
        {
            var Cheese = new Item.Item
            {
                name = "Cheese",
                Weight = 1,
                count = 10,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,             
                stats = new Stats()
                {                 
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This lump of cheese has a strong potent smell",
                    look = "This lump of cheese has a strong potent smell",
                    room = "A lump of stinky cheese",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5

            };

            return Cheese;
        }

        public static Item.Item Carrots()
        {
            var Carrots = new Item.Item
            {
                name = "Carrot",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A bunch of green leaves prtrude from this fresh looking orange carrot.",
                    look = "A bunch of green leaves prtrude from this fresh looking orange carrot.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return Carrots;
        }

        public static Item.Item Onions()
        {
            var Onions = new Item.Item
            {
                name = "Onion",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A small white onion, looking at makes you want to cry.",
                    look = "A small white onion.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return Onions;
        }

        public static Item.Item Tomato()
        {
            var Tomato = new Item.Item
            {
                name = "Tomato",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "A juicy red ripe tomato.",
                    look = "A juicy red ripe tomato.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return Tomato;
        }

        public static Item.Item Potato()
        {
            var Potato = new Item.Item
            {
                name = "Potato",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This potato is fresh from the ground and covered in dirt.",
                    look = "This potato is fresh from the ground and covered in dirt.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return Potato;
        }

        public static Item.Item Garlic()
        {
            var Garlic = new Item.Item
            {
                name = "Garlic",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "This is a strong smelling bulb of garlic.",
                    look = "This is a strong smelling bulb of garlic.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return Garlic;
        }

        public static Item.Item wheat()
        {
            var wheat = new Item.Item
            {
                name = "wheat",
                Weight = 1,
                equipable = true,
                eqSlot = Item.Item.EqSlot.Held,
                slot = Item.Item.EqSlot.Held,
                location = Item.Item.ItemLocation.Inventory,
                stats = new Stats()
                {
                    minUsageLevel = 1,
                    worth = 1
                },
                description = new Description()
                {
                    exam = "Fresh cut strand of wheat.",
                    look = "Fresh cut strand of wheat.",
                    room = "",
                    smell = "",
                    taste = "",
                    touch = "",
                },
                Gold = 5,
                ForageRank = 1

            };

            return wheat;
        }
    }
}