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

        //ill make a txt file
        //    Smoked Chub - 1x Chub

        //Boiled Carp - 1x Carp, 1x Potato

        //Peasant Stew - 1x Potato, 1x Carrot, 1x Garlic, 1x Onion

        //Seasoned Bream - 1x Bream, 1x Garlic

        //Fish Stew - 1x Perch, 1x Onion

        //Bread - 1x Flour

        //Fried Trout - 1x Brown Trout, 1x Flour

        //Turtle Soup - 1x Snapping Turtle, 1x Tomato

        //Fried Eel - 1x Eel, 1x Flour

        //Frog Legs - 1x Frog, 1x Flour, 1x Tomato, 1x Garlic
        //anything else you think should be added to it?

        public static Craft SmokedChub()
        {
            
            var SmokedChub  = new Craft()
            {
                Name = "Smoked Chub ",
                StartMessage = "You take out a dagger and begin cutting the fish.",
                FaliureMessage = "...",
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
                    location = Item.Item.ItemLocation.Room
                   


                },
                MoveCost = 20

            };

            return SmokedChub;

        }
    }
}