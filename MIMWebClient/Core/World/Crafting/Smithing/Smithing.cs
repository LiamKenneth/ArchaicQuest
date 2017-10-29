using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.Weapons.Sword.Long;

namespace MIMWebClient.Core.World.Crafting.Smithing

{
    public class Smithing
    {


        public static Craft Lantern()
        {

            var lantern = new Craft()
            {
                Name = "Lantern",
                StartMessage = "You put all your scrap metal into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a lantern.",
                Description = "To forge a latern you need to be at a furnace",
                CraftCommand = CraftType.Forge,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "The scrap metal begins to melt down.",
                    "You take out the melted metal and pour it into several casts that will make the latern.",
                    "The casts begin to cool down.",
                    "The metal in the casts go hard.",
                    "You crack each cast and begin welding them together."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Scrap metal",
                        Count = 1
                    }
                },
                CreatesItem = new Item.Item()
                {

                    name = "Lantern",
                    equipable = true,
                    slot = Item.Item.EqSlot.Light,
                    eqSlot = Item.Item.EqSlot.Light,
                    description = new Description()
                    {
                        look = "A lantern that takes oil to light the way."
                    },
                    location = Item.Item.ItemLocation.Inventory

                },
                MoveCost = 20

            };

            return lantern;

        }

        public static Craft CopperSword()
        {

            var CopperSword = new Craft()
            {
                Name = "Copper Sword",
                StartMessage = "You put all your copper ore into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a coper long sword.",
                Description = "To forge a copper long sword you need to be at a furnace",
                CraftCommand = CraftType.Forge,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "The copper ore begins to melt down.",
                    "You take out the melted metal and pour it into a cast.",
                    "The casts begin to cool down.",
                    "The metal in the casts go hard.",
                    "You crack the cast open."
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Copper Ore",
                        Count = 2
                    }
                },
                CreatesItem = LongSwordBasic.CopperSword(),
               
                MoveCost = 20

            };

            return CopperSword;

        }

    }
}