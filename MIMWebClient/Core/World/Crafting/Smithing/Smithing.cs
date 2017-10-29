using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.Weapons.Axe;
using MIMWebClient.Core.World.Items.Weapons.Blunt;
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;
using MIMWebClient.Core.World.Items.Weapons.Flail;
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
                SuccessMessage = "You have forged a copper long sword.",
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

        public static Craft CopperAxe()
        {

            var CopperAxe = new Craft()
            {
                Name = "Copper Axe",
                StartMessage = "You put all your copper ore into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a copper axe.",
                Description = "To forge a copper axe you need to be at a furnace",
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
                CreatesItem = AxeBasic.CopperAxe(),

                MoveCost = 20

            };

            return CopperAxe;

        }

        public static Craft CopperMace()
        {

            var CopperMace = new Craft()
            {
                Name = "Copper Mace",
                StartMessage = "You put all your copper ore into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a copper mace.",
                Description = "To forge a copper mace you need to be at a furnace",
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
                CreatesItem = MaceBasic.CopperMace(),

                MoveCost = 20

            };

            return CopperMace;

        }

        public static Craft CopperDagger()
        {

            var CopperMace = new Craft()
            {
                Name = "Copper Dagger",
                StartMessage = "You put all your copper ore into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a copper dagger.",
                Description = "To forge a copper dagger you need to be at a furnace",
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
                CreatesItem = DaggerBasic.CopperDagger(),

                MoveCost = 20

            };

            return CopperMace;

        }

        public static Craft CopperFlail()
        {

            var CopperFlail = new Craft()
            {
                Name = "Copper Flail",
                StartMessage = "You put all your copper ore into the furnace",
                FaliureMessage = "...",
                SuccessMessage = "You have forged a copper flail.",
                Description = "To forge a copper flail you need to be at a furnace",
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
                CreatesItem = FlailBasic.CopperFlail(),

                MoveCost = 20

            };

            return CopperFlail;

        }

    }
}