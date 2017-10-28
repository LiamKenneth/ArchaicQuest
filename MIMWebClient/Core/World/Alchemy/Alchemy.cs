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
                    "You add the scabious to the mortal and being grinding it with the pestle.",
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
    }
}