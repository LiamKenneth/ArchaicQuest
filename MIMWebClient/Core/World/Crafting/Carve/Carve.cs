using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.MiscEQ.Light;
using MIMWebClient.Core.World.Items.Weapons.Sword.Long;

namespace MIMWebClient.Core.World.Crafting.Carve

{
    public class Carve
    {


        public static Craft WoodenRaft()
        {

            var lantern = new Craft()
            {
                Name = "Wooden Raft",
                StartMessage = "You gather your logs and begin to make the raft",
                FaliureMessage = "...",
                SuccessMessage = "You have crafted a wooden raft",
                Description = "To craft a raft you need to be at a Carpentry work bench",
                CraftCommand = CraftType.Carve,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You sand the logs down making them smooth to touch",
                    "You bind the logs together to form the platform of the raft",
                    "You  oil the raft so it's water proof.",
                   
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Pine logs",
                        Count = 5
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Wooden Raft",
                    equipable = false,                    
                    description = new Description()
                    {
                        look = "A simple wooden raft only suitable for slow rivers."
                    },
                    location = Item.Item.ItemLocation.Inventory

                },
                MoveCost = 20

            };

            return lantern;

        }

        public static Craft WoodenTorch()
        {

            var WoodenTorch = new Craft()
            {
                Name = "Wooden torch",
                StartMessage = "You gather your logs and begin to make the torch",
                FaliureMessage = "...",
                SuccessMessage = "You have crafted a wooden torch.",
                Description = "To craft a wooden torch you need Carpentry work bench",
                CraftCommand = CraftType.Forge,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You sand the logs down making them smooth to touch",
                    "You carve the log into a club shape.",
                    "You try the rag around the torch and dip it into oil.",
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "log",
                        Count = 1
                    },
                    new CraftMaterials()
                    {
                        Name = "rag",
                        Count = 1
                    }
                },
                CreatesItem = Light.WoodenTorch(),
               
                MoveCost = 20

            };

            return WoodenTorch;

        }

    }
}