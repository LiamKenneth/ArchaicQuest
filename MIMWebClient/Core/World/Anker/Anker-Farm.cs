using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.World.Anker.Mobs;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Arms;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Body;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Feet;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Head;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Legs;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Hands;
using MIMWebClient.Core.World.Items.Consumables.Food;
using MIMWebClient.Core.World.Items.Weapons.Axe;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.World.Items.Weapons.Sword.Long;

    public static class AnkerFarm
    {

        public static Room AnkerRoad()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 0,
                title = "Anker road, east of the Anker gate",
                description = "<p> The road extends from the gates of Anker in the west and the farms to the east, the road is marked by the wagons that traverse it regularly, causing small dips in the road that collect water and turn into mud. The smell of the farms drifts in from the east filling the air with the scent of wheat and pigs.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 41,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion
            room.exits.Add(east);
            room.exits.Add(west);


            return room;
        }

        public static Room RoadThroughFarm()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 1,
                title = "Road through farm",
                description = "<p> A house is to the north of here with a plume of smoke coming from the chimney and the smell of the crops being grown floats on the breeze from the south. The road is a little more kept here as if someone tries to keep this section maintained for the farms.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 2,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion
            room.exits.Add(north);
            room.exits.Add(south);
            room.exits.Add(east);
            room.exits.Add(west);


            return room;
        }

        public static Room Farm()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 2,
                title = "Farmer O'Neil's House",
                description = "<p> The small house seems to be built for a life of hard work, the bed is tiny with only a few pelts laid across it for some padding. There are several jars of jerky filling the shelves, with a few bags of produce piled into a corner. The sound of the windmill spinning to the east makes the area more noisier than the rest of the farm.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            room.items = new List<Item.Item>()
            {
                    new Item.Item()
                    {
                        name = "Cooking pot",
                        location = Item.Item.ItemLocation.Room,
                        waterContainer = true,
                        waterContainerAmount = 5,
                        container = true,
                        containerSize = 5,
                        description = new Description(),
                         containerItems = new ItemContainer()
                    },
                                        new Item.Item()
                    {
                        name = "Bed roll",
                        location = Item.Item.ItemLocation.Room,
                        container = true,
                        containerSize = 5,
                        description = new Description(),
                        containerItems = new ItemContainer()
                    },

                new Item.Item()
                {
                name = "Chub",
                location = Item.Item.ItemLocation.Room
            }

        };

            var farmer = FarmerOneil.Farmer();
            farmer.Recall = new Recall()
            {
                Region = room.region,
                AreaId = room.areaId,
                Area = room.area
            };
            room.mobs.Add(farmer);

            #region exits

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(south);


            return room;
        }

        public static Room CropFields()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 3,
                title = "Crop Fields",
                description = "<p>Mounds of dirt decorate the field with the sprouting of carrots erupting from each. They are almost ready to be harvested, but it appears some vermin have decided not to wait as signs of the crops being torn out are evident.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    
                },
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "Rabbits hop around, eating the carrots.",
                    "You hear a small squeak.",
                    "ZZZZzzzZZ, A wasp buzzes around your head.",
                    "You hear a pig snorting.",
                }


            };

            var rabbitmob = rabbit.SmallRabbit();
            var rabbitmob2 = rabbit.SmallRabbit();

            rabbitmob.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            rabbitmob2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            room.mobs.Add(rabbitmob);
            room.mobs.Add(rabbitmob2);

            var carrot = Food.Carrots();
            var carrot2 = Food.Carrots();
            var carrot3 = Food.Carrots();
            var carrot4 = Food.Carrots();
            var carrot5 = Food.Carrots();
            var carrot6 = Food.Carrots();

            room.ForageItems.Add(carrot);
            room.ForageItems.Add(carrot2);
            room.ForageItems.Add(carrot3);
            room.ForageItems.Add(carrot4);
            room.ForageItems.Add(carrot5);
            room.ForageItems.Add(carrot6);


            #region exits

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 6,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };




            #endregion
            room.exits.Add(north);
            room.exits.Add(south);
            room.exits.Add(west);


            return room;
        }

        public static Room CropFields1()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 6,
                title = "Crop Fields",
                description = "<p>A sea of wheat fills this field, no doubt ground into flour by the large windmill to the north east. A few farm hands are working harvesting the wheat with large scythes and pilling them into a wagon pulled by a donkey behind them.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "Rabbits hop around, eating the carrots.",
                    "You hear a small squeak.",
                    "ZZZZzzzZZ, A wasp buzzes around your head.",
                    "You hear a pig snorting.",
                }


            };

            var rabbitmob = rabbit.SmallRabbit();
            var rabbitmob2 = rabbit.SmallRabbit();

            rabbitmob.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            rabbitmob2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            room.mobs.Add(rabbitmob);
            room.mobs.Add(rabbitmob2);


            var pig = Pig.SmallPig();

            pig.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            var pig2 = Pig.SmallPig();

            pig2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            room.mobs.Add(pig);
            room.mobs.Add(pig2);


            var wheat = Food.wheat();
            var wheat2 = Food.wheat();
            var wheat3 = Food.wheat();
            var wheat4 = Food.wheat();
            var wheat5 = Food.wheat();
            var wheat6 = Food.wheat();
            var wheat7 = Food.wheat();
            var wheat8 = Food.wheat();
            var wheat9 = Food.wheat();
            var wheat10 = Food.wheat();
            var wheat11 = Food.wheat();

            room.ForageItems.Add(wheat);
            room.ForageItems.Add(wheat2);
            room.ForageItems.Add(wheat3);
            room.ForageItems.Add(wheat4);
            room.ForageItems.Add(wheat5);
            room.ForageItems.Add(wheat6);
            room.ForageItems.Add(wheat7);
            room.ForageItems.Add(wheat8);
            room.ForageItems.Add(wheat9);
            room.ForageItems.Add(wheat10);
            room.ForageItems.Add(wheat11);

            #region exits

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 5,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(east);
            room.exits.Add(south);


            return room;
        }

        public static Room CropFields2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 4,
                title = "Crop Fields",
                description = "<p>This field Is full of potatoes, their large leaves covering the ground with little flowers sprouting off the tops. Mostly a sea of green with white and yellow spots, they continue to grow, soon the farm hands will come and begin to harvest this field.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "Rabbits hop around, eating the carrots.",
                    "You hear a small squeak.",
                }


            };

            var rabbitmob = rabbit.SmallRabbit();
            var rabbitmob2 = rabbit.SmallRabbit();
            var rabbitmob3 = rabbit.SmallRabbit();
            var rabbitmob4 = rabbit.SmallRabbit();

            rabbitmob.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            rabbitmob2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            rabbitmob3.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            rabbitmob4.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            room.mobs.Add(rabbitmob);
            room.mobs.Add(rabbitmob2);
            room.mobs.Add(rabbitmob3);
            room.mobs.Add(rabbitmob4);


            var potato = Food.Potato();
            var potato2 = Food.Potato();
            var potato3 = Food.Potato();
            var potato4 = Food.Potato();
            var potato5 = Food.Potato();
            var potato6 = Food.Potato();

            room.ForageItems.Add(potato);
            room.ForageItems.Add(potato2);
            room.ForageItems.Add(potato3);
            room.ForageItems.Add(potato4);
            room.ForageItems.Add(potato5);
            room.ForageItems.Add(potato6);
            
            #region exits

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 5,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(north);
            room.exits.Add(west);


            return room;
        }

        public static Room CropFields3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 5,
                title = "Crop Fields",
                description = "<p>Large spots of red cover this field of tomatoes, the fruit so loved by many. Several of the plants have wasp zipping around them possibly using the shadows cast by the nightshade. To the north in the distance is a house with a chimney with smoke pouring out.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                Emotes = new List<string>()
                {
                    "ZZZZzzzZZ, A wasp buzzes around your head.",
                    "You hear a pig snorting.",
                },
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            var tomato = Food.Tomato();
            var tomato2 = Food.Tomato();
            var tomato3 = Food.Tomato();
            var tomato4 = Food.Tomato();
            var tomato5 = Food.Tomato();
            var tomato6 = Food.Tomato();

            room.ForageItems.Add(tomato);
            room.ForageItems.Add(tomato2);
            room.ForageItems.Add(tomato3);
            room.ForageItems.Add(tomato4);
            room.ForageItems.Add(tomato5);
            room.ForageItems.Add(tomato6);

            #region exits

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 6,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var pig = Pig.SmallPig();

            pig.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            var pig2 = Pig.SmallPig();

            pig2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            room.mobs.Add(pig);
            room.mobs.Add(pig2);


            #endregion

            room.exits.Add(north);
            room.exits.Add(east);


            return room;
        }

        public static Room AnkerRoad1()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 7,
                title = "Anker Road",
                description = "<p>Bundled branches hold onto the occasional tree, and a range of flowers, which cling to any space they can find, clash with each other creating a natural mixture of color. A melody of noises, most of which are insects, resonate through the air, yet their sounds are softened almost drowned out by the splashing of fish in the nearby river.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 8,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);


            return room;
        }

        public static Room Windmill()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 8,
                title = "The windmill",
                description = "<p> A large windmill is slowly turning as the breeze floats through the air, the sounds of the millstone crushing the wheat grinding it into flour resound about. It is built from the local pine forest with solid copper braces reinforcing it in certain places.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(south);



            return room;
        }

        public static Room RiverBank()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 9,
                title = "The riverbank",
                terrain = Room.Terrain.Water,
                description = "<p>Several thick reeds are growing in the shallow part of the river, with ripples dotting around indicating a plethora of life is here. A couple of stumps are a few feet in providing obstacles to what would otherwise be an ideal fishing spot, although no problem for an experienced fisherman.</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    new  Item.Item()
                    {
                        name = "basic old fishing rod",
                        
                        location = Item.Item.ItemLocation.Room,
                        slot = Item.Item.EqSlot.Held,
                        eqSlot = Item.Item.EqSlot.Held,
                        description = new Description()
                        {
                            look = "This is an old long wooden fishing rod, looks to be well used. There have been other methods for catching fish, though the use of a rod like this one is the tried and tested, and most successful, method.",
                            room = "A rod is resting here, it's line has been casted out into the water."

                        }

                    }
                },
                mobs = new List<Player>(),
 
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 15,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);
            room.exits.Add(south);



            return room;
        }

        public static Room PalusRiver()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 15,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                needsBoat = true,


            };

            #region exits


            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 16,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);
            room.exits.Add(east);



            return room;
        }

        public static Room PalusRiver0()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 16,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                needsBoat = true,


            };

            #region exits


    
            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 17,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 15,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }
        public static Room PalusRiver1()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 17,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                needsBoat = true,


            };

            #region exits


            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 19,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 18,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 16,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }


        public static Room PalusRiver2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 18,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                needsBoat = true,


            };

            #region exits


            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 20,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

 

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 17,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);
            room.exits.Add(west);



            return room;
        }
        public static Room PalusRiver3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 20,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                needsBoat = true,


            };

            #region exits


            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 18,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 19,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(south);
            room.exits.Add(west);



            return room;
        }

        public static Room HermitsHome()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 19,
                title = "The palus river",
                terrain = Room.Terrain.Water,
                description = "<p>Description to come</p>",
                Emotes = new List<string>()
                {
                    "Plop! You hear something fall into the water.",
                    "A dragon fly skims over the river then flies off.",
                    "A fish surfaces quickly to eat something sitting ontop of the water.",
                    "You hear a frog croak."
                },
                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 20,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 17,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(east);
            room.exits.Add(south);



            return room;
        }

        public static Room AnkerRoad2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 10,
                title = "Anker Road",
                description = "<p>A disordered patch of grass is flanked by luscious hedges and flower bushes, the flower beds are fighting for their place on the sides of the road, providing a pleasant scent to the area that is accompanied by the soft sounds of the river nearby flowing.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 11,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 13,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 12,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);


            return room;
        }

        public static Room SawMill()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 12,
                title = "Saw mill",
                description = "<p> A large water wheel is on the side of an open building, with a set of gears inside moving around powering a long saw blade that slowly moves up and down. The river rushes by to the south pushing the wheel, it creeking as it turns and dumping the water from its paddles back into the river in a steady stream. On the inside of the open building is a chopping block with a pile of wood stacked up.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    new Item.Item()
                    {
                        name = "Chopping block",
                        location = Item.Item.ItemLocation.Room,
                        description = new Description()
                        {
                            look = "A pile of felled pine wood is stacked next to the chopping block. You can be use the chopping block to chop wood that can be used for crafting."
                        },
                        stuck = true,
                        ChoppingBlock = Item.Item.ChoppingBlockType.Pine
                        
                    },
                    
                    
                    
                },
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                type = Room.RoomType.Standard


            };


            var axe = AxeBasic.IronHatchet();
            axe.location = Item.Item.ItemLocation.Room;
            room.items.Add(axe);

            #region exits


            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);



            return room;
        }

        public static Room Pasture()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 11,
                title = "Pasture",
                description = "<p>Description to come</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var north = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            var pig = Pig.SmallPig();

            pig.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            var pig2 = Pig.SmallPig();

            pig2.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            var pig3 = Pig.SmallPig();

            pig3.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            var pig4 = Pig.SmallPig();

            pig4.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };



            room.mobs.Add(pig);
            room.mobs.Add(pig2);
            room.mobs.Add(pig3);
            room.mobs.Add(pig4);


            room.exits.Add(north);



            return room;
        }

        public static Room AnkerRoad3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 13,
                title = "Anker Road",
                description = "<p>There are fields everywhere, their patterns only broken up by the occasional tree left to grow in peace. All around are cows and deer gently grazing in the sunlit pastures by the road, passing field after field runs a weed-ridden, dusty old road that connects the Goblin Forest to the town of Anker.</p>",
                type = Room.RoomType.Standard,

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 14,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }

        public static Room TheBridge()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 14,
                title = "The bridge leading into the goblin forest",
                description = "<p>The canopy overhead covers most of the sky above casting shade onto the bridge entering the Goblin Forest, the river gushes beneath the bridge drowning out the sound of anything that may be near here. In the distance to the east smoke rises from the forest, and to the west is the lights of the town of Anker.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>()
                {

                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


            };

            #region exits


            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 13,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion



            room.exits.Add(west);



            return room;
        }

    }
}