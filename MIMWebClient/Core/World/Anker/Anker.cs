using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public static class Anker
    {
        public static Room VillageSquare()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 0,
                title = "Village Sqaure",
                description = "A round stone well occupies the centre of the grey cobbled square. " +
                "A large oak tree provides shade. It is frequently used by passers-by and the villages main source of fresh water " +
                "<br /> Not far from the well is the village notice board. " +
                "<br /> The local inn is to the north.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            //Room Keywords

            var well = new RoomObject
            {
                name = "Stone well",
                look = "A well used wooden bucket hangs lopsided over the well. On the side is a handle used for lowering and lifting the bucket.",
                examine = "Inscribed in one of the stone blocks of the well is IX-XXVI, MMXVI",
                touch = "The stone fills rough to touch",
                smell = "The water from the well smells somewhat fresh and pleasant"
            };



            var bucket = new RoomObject
            {
                name = "Bucket",
                look ="A well used wooden bucket hangs lopsided over the well. On the side is a handle used for lowering and lifting the bucket.",
                examine = "Inside the bucket you see some gold coins",
                touch = "The bucket is wet to touch",
                smell = "The bucket smells damp"
            };

            var noticeboard = new RoomObject
            {
                name = "Village notice board",
                look = "A notice board is here with only one piece of parchment attached",
                examine = "You take a closer look at the notice board and read the parchment attached <br />"
                + "Welcome to MIM <br /> This is the starting village. You can look, examine obviously. Move using N,E,south etc. look around and let me know what you think...",
                touch = "The notice board is wooden and smooth to touch",
                smell = "The notice board has no obvious smell "
            };

            //add some gold to bucket
            var woodenChestObj = new Item.Item
            {

                name = "Wooden Chest",
                containerItems = new List<Item.Item>(),
                canLock = true,
                containerSize = 10,
                container = true,
                isVisibleToRoom = true,
                location = Item.Item.ItemLocation.Room,
                description = new Item.Description { look = "Small Chest by the well" },
                open = false,
                canOpen = true,
                locked = true,
                keyId = Guid.NewGuid().ToString()
            };

            woodenChestObj.keyValue = woodenChestObj.keyId;
            room.items.Add(woodenChestObj);


            var oddKey = new Item.Item
            {

                name = "Odd looking key",
                isVisibleToRoom = true,
                location = Item.Item.ItemLocation.Room,
                description = new Item.Description { look = "Odd looking Key" },
                keyValue = woodenChestObj.keyId
        };
            room.items.Add(oddKey);


            var bucketObj = new Item.Item();




            var bucketGold = new Item.Item();

            bucketObj.container = true;
            bucketObj.waterContainer = true;
            bucketObj.waterContainerSize = 15;
            bucketObj.containerItems = new List<Item.Item>();
            bucketObj.isVisibleToRoom = false;
            bucketObj.name = "bucket";

            bucketGold.count = 5;
            bucketGold.type = Item.Item.ItemType.Gold;
            bucketGold.name = "Gold Coins";

            bucketObj.containerItems.Add(bucketGold);



            var bench = new RoomObject
            {
                name = "Stone bench",
                look = "A stone bench sits under the shade of the large oak tree",
                examine = "There is nothing more of detail to see",
                touch = "The stone fills rough to touch",
                smell = "The smell of flowers is smelt by the bench"
            };



            room.keywords.Add(noticeboard);
            room.keywords.Add(bucket);
            room.keywords.Add(well);
            room.keywords.Add(bench);

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = "wooden door",
                description = new Item.Description
                {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",

                }
            };

            

            //create items

            room.items.Add(bucketObj);

            room.exits.Add(north);

            //Create Mobs
            var cat = new Player
                          {
                              Name = "Black and White cat", Type = "Mob", Description = "This black cat's fur looks in pristine condition despite being a stray.",
                              Strength = 1, Dexterity = 1, Constitution = 1, Intelligence = 1, Wisdom = 1, Charisma = 1, MaxHitPoints = 100, HitPoints = 100, Level = 1, Status = Player.PlayerStatus.Standing, 
              Skills = new List<Skill>(),

           
        };

            var h2h = new Skill();
            h2h.Name = "Hand to Hand";
            h2h.Proficiency = 0.1;

            cat.Skills.Add(h2h);
            

            room.mobs.Add(cat);



            return room;
        }

        public static Room SquareWalkOutsideTavern()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 1,
                title = "Square walk, outside the drunken sailor",
                description = "To the north is a large Inn, it's exterial walls plastered white. Either side of the large door is two checked windows." +
                " To the south is the Village Square. The square walk continues east and west towards the stables joining the inn.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };




            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 2,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                description = new Item.Description
                {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",

                }
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                description = new Item.Description
                {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",

                }
            };


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                description = new Item.Description
                {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",

                }
            };


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                description = new Item.Description
                {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",

                }
            };

            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);         
            room.exits.Add(west);




            return room;
        }
    }
}