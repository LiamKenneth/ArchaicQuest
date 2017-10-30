using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Arms;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Feet;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Hands;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Head;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Legs;
using MIMWebClient.Core.World.Items.Clothing.ClothingBody;
using MIMWebClient.Core.World.Items.MiscEQ.Light;
 

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
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    }
                },
                CreatesItem = ClothingHead.WoolenClothHelmet(),
                MoveCost = 20

            };

      

            return WoolenClothHelmet;

        }

        public static Craft WoolenClothLeggings()
        {

            return new Craft()
            {
                Name = "Woolen cloth leggings",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted some woolen cloth leggings.",
                Description = "To knit some woolen cloth leggings you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    }
                },
                CreatesItem = ClothingLegs.WoolenClothLeggings(),
                MoveCost = 20

            };

        }

        public static Craft WoolenClothSleeves()
        {

            return new Craft()
            {
                Name = "Woolen cloth sleeves",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted some woolen cloth sleeves.",
                Description = "To knit some woolen cloth sleeves you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    }
                },
                CreatesItem = ClothingArms.WoolenClothSleeves(),
                MoveCost = 20

            };

        }

        public static Craft WoolenClothGloves()
        {

            return new Craft()
            {
                Name = "Woolen cloth gloves",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted some woolen cloth gloves.",
                Description = "To knit some woolen cloth gloves you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    } 
                },
                CreatesItem =  ClothingHands.WoolenClothGloves(),
                MoveCost = 20

            };

        }

        public static Craft WoolenClothBoots()
        {

            return new Craft()
            {
                Name = "Woolen cloth boots",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted some woolen cloth boots.",
                Description = "To knit some woolen cloth boots you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    } 
                },
                CreatesItem = ClothingFeet.WoolenClothBoots(),
                MoveCost = 20

            };

        }

        public static Craft WoolenClothShirt()
        {

            return new Craft()
            {
                Name = "Woolen cloth shirt",
                StartMessage = "You take the wool and cast on the needle.",
                FaliureMessage = "...",
                SuccessMessage = "You have knitted some woolen cloth shirt.",
                Description = "To knit some woolen cloth shirt you need a pair of needles and wool yarn.",
                CraftCommand = CraftType.Knitting,
                CraftAppearsInRoom = false,
                CraftingEmotes = new List<string>()
                {
                    "You form a loop with the thread and put the right needle through it, then pull it up so it crosses in front of the left needle, from back to front.",
                    "You repeat with the left needle",
                    "You continue knitting your rows",
                    "You cast off the yarn and secure the loose end"
                },
                Materials = new List<CraftMaterials>()
                {
                    new CraftMaterials()
                    {
                        Name = "Wool",
                        Count = 1
                    } 
                },
                CreatesItem =  ClothingBody.WoolenClothShirt(),
                MoveCost = 20

            };

        }


    }
}