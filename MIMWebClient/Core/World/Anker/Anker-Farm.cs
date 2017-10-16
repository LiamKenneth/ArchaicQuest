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
                areaId = 422,
                title = "Anker road, east of the Anker gate",
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

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 48,
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
                areaId = 48,
                title = "Road through farm",
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

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 49,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 50,
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
                areaId = 54,
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
                areaId = 422,
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
                areaId = 49,
                title = "Farmer O'Neil's House",
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

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 48,
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
                areaId = 50,
                title = "Crop Fields",
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

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 53,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var north = new Exit
            {
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 48,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 51,
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
                areaId = 51,
                title = "Crop Fields",
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

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 52,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 50,
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
                areaId = 52,
                title = "Crop Fields",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 51,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 53,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(north);
            room.exits.Add(east);


            return room;
        }

        public static Room CropFields3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 53,
                title = "Crop Fields",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 50,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 52,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion

            room.exits.Add(north);
            room.exits.Add(west);


            return room;
        }

        public static Room AnkerRoad1()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 54,
                title = "Anker Road",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 55,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 48,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 58,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 56,
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
                areaId = 55,
                title = "The windmill",
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


            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 54,
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
                areaId = 56,
                title = "The riverbank",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 54,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);



            return room;
        }

        public static Room AnkerRoad2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 58,
                title = "Anker Road",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 57,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 54,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 60,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker Farm",
                region = "Anker",
                areaId = 59,
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
                areaId = 59,
                title = "Saw mill",
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
                name = "North",
                area = "Anker Farm",
                region = "Anker",
                areaId = 58,
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
                areaId = 57,
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
                areaId = 58,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion


            room.exits.Add(north);



            return room;
        }

        public static Room AnkerRoad3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker Farm",
                areaId = 60,
                title = "Anker Road",
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


            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 58,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var east = new Exit
            {
                name = "East",
                area = "Anker Farm",
                region = "Anker",
                areaId = 61,
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
                areaId = 61,
                title = "The bridge leading into the goblin forest",
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


            var west = new Exit
            {
                name = "West",
                area = "Anker Farm",
                region = "Anker",
                areaId = 60,
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