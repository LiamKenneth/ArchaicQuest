using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World.Anker
{
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
                "<br /> Not far from the well is the village notice board. Covered in parchments of all sizes " +
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

            var well = new RoomObject();

            well.name = "Stone well";
            well.look = "A well used wooden bucket hangs lopsided over the well. On the side is a handle used for lowering and lifting the bucket.";
            well.examine = "Inscribed in one of the stone blocks of the well is IX-XXVI, MMXVI";
            well.touch = "The stone fills rough to touch";
            well.smell = "The water from the well smells somewhat fresh and pleasant";


            var bucket = new RoomObject();

            bucket.name = "Bucket";
            bucket.look = "A well used wooden bucket hangs lopsided over the well. On the side is a handle used for lowering and lifting the bucket.";
            bucket.examine = "Inside the bucket you see some gold coins";
            bucket.touch = "The bucket is wet to touch";
            bucket.smell = "The bucket smells damp";

            //add some gold to bucket
            var bucketObj = new Item.Item();
            var bucketGold = new Item.Item();

            bucketObj.container = true;
            bucketObj.waterContainer = true;
            bucketObj.waterContainerSize = 15;
            bucketObj.containerItems = new List<Item.Item>();
            bucketObj.isVisibleToRoom = false;

            bucketGold.count = 5;
            bucketGold.type = Item.Item.ItemType.Gold;
            bucketGold.name = "Gold Coins";

            bucketObj.containerItems.Add(bucketGold);

            

            var bench = new RoomObject();

            bench.name = "Stone bench";
            bench.look = "A stone bench sits under the shade of the large oak tree";
            bench.examine = "There is nothing more of detail to see";
            bench.touch = "The stone fills rough to touch";
            bench.smell = "The smell of flowers is smelt by the bench";



            room.keywords.Add(well);
            room.keywords.Add(bench);

            // Create Exits
            var North = new Exit {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,                
                description = new Item.Description {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",
                    
                }
            };

            //create items

            room.exits.Add(North);

            //Create Mobs
            var cat = new Player { Name = "Black and White cat" , Type = "Mob", Description = "This black cat's fur looks in pristine condition despite being a stray."};

            room.mobs.Add(cat);



            return room;
        }

       
    }
}