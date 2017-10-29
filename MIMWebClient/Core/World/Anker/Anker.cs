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
using MIMWebClient.Core.World.Items.Consumables.Drinks;
using MIMWebClient.Core.World.Items.MiscEQ.Held;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.World.Items.Weapons.Sword.Long;

    public static class Anker
    {
        public static Room VillageSquare()
        {

            /*
             *  Region: the province the area is in
             *  Area : Name of area in Region
             *  AreaId: Must be unique and used to for finding the room. Entering a room using Region + area + id
             *  Title: Title of room
             *  description: Description of room can use HTML (No defined classes yet for colour output)
             *  exits = new List<Exit>(), 
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true, sets the room as untouched. gets set to false with interation like get, mob death etc
             * 
             * 
             */

            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 0,
                title = "Village Square",
                description = "<p>A cross is formed by dirt tracks running through the village green from" +
                            "Square walk to the centre, circling a stone well. Low hedges follow the " +
                            "path either side. A lantern hangs from a wooden signpost in the centre. " +
                            "The village notice board has been hammered into a large oak tree near the " +
                            "path to the centre.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                ForageItems = new ItemContainer()
                {
                    Held.TatteredRag()
                }


            };

            //Room Keywords

            /*
             *  All items mentioned in the description should have keywords
             *  name: name of object, this is used to find the object.
             *  look: basic description
             *  examine: more in depth description
             *  touch: touch description
             *  smell: smell description
             * 
             */

            var well = new RoomObject
            {
                name = "Stone well",
                look = "A well used wooden bucket hangs lopsided by a rope swinging over the well. On the side of the well is a handle used for lowering and lifting the bucket.",
                examine = "Inscribed in one of the stone blocks of the well is IX-XXVI, MMXVI",
                touch = "The stone fills rough to touch",
                smell = "The water from the well smells somewhat fresh and pleasant"
            };

            var signpost = new RoomObject
            {
                name = "Signpost",
                look = "The signpost points:<br /> " +
                       "<span class='RoomExits'>North</span><br /> The Red Lion<br />" +
                       "<span class='RoomExits'>North East</span><br />  Odds and sods <br /> Black smith<br />" +
                       "<span class='RoomExits'>East</span><br />Village hall<br />" +
                       "<span class='RoomExits'>South East</span><br />Church<br />" +
                       "<span class='RoomExits'>North West</span><br /> Stables.",
                examine = "The signpost points:<br /> " +
                       "<span class='RoomExits'>North</span><br /> The Red Lion<br />" +
                       "<span class='RoomExits'>North East</span><br />  Odds and sods <br /> Black smith<br />" +
                       "<span class='RoomExits'>East</span><br />Village hall<br />" +
                       "<span class='RoomExits'>South East</span><br />Church<br />" +
                       "<span class='RoomExits'>North West</span><br /> Stables.",
                touch = "The signpost is finely crafted and smooth to touch",
                smell = "The signpost has no obvious smell"
            };



            var bucket = new RoomObject
            {
                name = "Bucket",
                look = "A well used wooden bucket hangs lopsided over the well. On the side is a handle used for lowering and lifting the bucket.",
                examine = "Inside the bucket you see some gold coins",
                touch = "The bucket is wet to touch",
                smell = "The bucket smells damp"
            };

            var noticeboard = new RoomObject
            {
                name = "Village notice board",
                look = "A notice board has been hammered into the oak tree with only one piece of parchment attached",
                examine = "You take a closer look at the notice board and read the parchment attached <br />"
                + "Welcome to MIM <br /> This is the starting village. You can look, examine obviously. Move using N,E,south etc. look around and let me know what you think...",
                touch = "The notice board is wooden and smooth to touch",
                smell = "The notice board has no obvious smell "
            };

            /* Adding Items
             *  Name: of Item
             *  Conainer Items: is a list of Item
             *  contianer: true, means it's a container
             *  container size: how many items it can fit
             *  Can lock: true, means lockable/unlockable
             *  isvisible: can the player see it? This is good if you want items to be hidden unless a player examines say a stool and finds a lock pick under it.
             *  location: has to be room, if it's in a room. Inventory for if it's being carried, wield if it's wielded and worn if the player/mob is wearing it
             *  description: Look, exam etc same as room descriptions
             *  open: for doors and containers. false means shut.
             *  canOpen: Means it's a container that's openable
             *  locked: true = locked.
             *  Keyid: is a newGuid, and the generated ID is then given to a keyvalue on another item which is used to unlock the item
             *  keyvalue: = keyId if set
             */

            //add some gold to bucket
            //var woodenChestObj = new Item.Item
            //{

            //    name = "Wooden Chest",
            //    containerItems = new ItemContainer(),
            //    canLock = true,
            //    containerSize = 10,
            //    container = true,
            //    location = Item.Item.ItemLocation.Room,
            //    description = new Item.Description { look = "Small Chest by the well" },
            //    open = false,
            //    canOpen = true,
            //    locked = true,
            //    keyId = Guid.NewGuid().ToString(),
            //    stuck = true
            //};

            //woodenChestObj.keyValue = woodenChestObj.keyId;
            //room.items.Add(woodenChestObj);


            //var oddKey = new Item.Item
            //{

            //    name = "Odd looking key",
            //    location = Item.Item.ItemLocation.Room,
            //    description = new Item.Description { look = "Odd looking Key" },
            //    keyValue = woodenChestObj.keyId
            //};
            //room.items.Add(oddKey);


            var bucketObj = new Item.Item();




            var bucketGold = new Item.Item();

            bucketObj.container = true;
            bucketObj.waterContainer = true;
            bucketObj.waterContainerAmount = 15;
            bucketObj.containerItems = new ItemContainer();
            bucketObj.isHiddenInRoom = true;
            bucketObj.name = "bucket";
            bucketObj.location = Item.Item.ItemLocation.Room;

            bucketGold.location = Item.Item.ItemLocation.Room;
            bucketGold.Gold = 75;
            bucketGold.type = Item.Item.ItemType.Gold;
            bucketGold.name = "Gold Coins";

            bucketObj.containerItems.Add(bucketGold);



            var bench = new RoomObject
            {
                name = "Stone bench",
                look = "A stone bench sits under the conopy of the large oak tree",
                examine = "There is nothing more of detail to see",
                touch = "The stone fills rough to touch",
                smell = "The smell of flowers is smelt by the bench"
            };


            room.keywords.Add(signpost);
            room.keywords.Add(noticeboard);
            room.keywords.Add(bucket);
            room.keywords.Add(well);
            room.keywords.Add(bench);

            /*
             * 
             * name: "North", East, South, West. List must be added in that order. To have another exit suchas a portal or hidden crevice we need an enter commande: Enter portal for example
                area = "Anker", - The area the exit leads to
                region = "Anker", - The region the exit leads to
                areaId = 1, - THe room id the exit leads too
                keywords = new List<string>(), - this may be obsolete or should be used as description below does not work
                hidden = false, - is the exit hidden?
                locked = false, - is the exit locked?
                canLock = true, - can it be locked?
                canOpen = true, - is it openable?
                open = true, - is it open
                doorName = "wooden door", - name of door/exit
                description = new Item.Description - doesn't seem to work
                {
                    look = "To the north you see the inn of The Red Lion.", //return mobs / players?
                    exam = "To the north you see the inn of The Red Lion.",

                },
             * 
             */

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
                doorName = "wooden door"

            };
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 8,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 6,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true
            };


            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true
            };


            //create items

            room.items.Add(bucketObj);

            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);

            //Create Mobs
            var cat = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Black and White cat",
                NPCLongName = "Black and White cat",
                Type = Player.PlayerTypes.Mob,
                Description = "This black cat's fur looks in pristine condition despite being a stray.",
                AreaId = 0,
                Area = "Anker",
                Region = "Anker",
                Strength = 30,
                Dexterity = 50,
                Constitution = 12,
                Intelligence = 1,
                Wisdom = 1,
                Charisma = 1,
                MaxHitPoints = 10,
                HitPoints = 10,
                Level = 1,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker",
                    AreaId = 0,
                },
                PathList = new List<string>()
                {
                    "w",
                    "n",
                    "e",
                    "s"
                }


            };

            var cat2 = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Black and White cat",
                NPCLongName = "Black and White cat",
                Type = Player.PlayerTypes.Mob,
                Description = "This black cat's fur looks in pristine condition despite being a stray.",
                Strength = 30,
                Dexterity = 50,
                Constitution = 12,
                Intelligence = 1,
                Wisdom = 1,
                Charisma = 1,
                MaxHitPoints = 10,
                HitPoints = 10,
                Level = 1,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer()


            };


            var dagger = new Item.Item
            {
                actions = new Item.Action(),
                name = "Blunt dagger",
                eqSlot = Item.Item.EqSlot.Wielded,
                weaponType = Item.Item.WeaponType.ShortBlades,
                stats = new Item.Stats { damMin = 2, damMax = 4, minUsageLevel = 1 },
                type = Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Item.Item.AttackType.Pierce,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Room,
                description = new Description(),
                itemFlags = new List<Item.Item.ItemFlags>()

            };

            dagger.itemFlags.Add(Item.Item.ItemFlags.glow);

            dagger.description.look = "This is just a blunt dagger";
            dagger.description.exam = "This is an extremely blunt dagger";




            var dagger2 = new Item.Item
            {
                actions = new Item.Action(),
                name = "Flaming dagger",
                eqSlot = Item.Item.EqSlot.Wielded,
                weaponType = Item.Item.WeaponType.ShortBlades,
                stats = new Item.Stats { damMin = 21, damMax = 44, minUsageLevel = 1 },
                type = Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Item.Item.AttackType.Pierce,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Room
            };





            room.items.Add(dagger);
            // room.items.Add(dagger3);

            cat.Inventory.Add(dagger);

            /* how to add skills but think this needs rethinking */
            //var h2h = Skill.Skills().Find(x => x.Name.Equals(Skill.HandToHand));

            //h2h.Proficiency = 1;

            //cat.Skills.Add(h2h);          

            var recall = new Recall
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };


            cat.Recall = recall;
            cat2.Recall = recall;

            room.mobs.Add(cat);
            room.mobs.Add(cat2);

            return room;
        }

        public static Room SquareWalkOutsideTavern()
        {
            #region room setup
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 1,
                title = "Square walk, outside the Red Lion",
                description = "<p>The Red Lion occupies the north western path of Square walk. It's large oval wooden door is kept closed keeping the warmth inside as well as the hustle and bustle hidden from the outside. " +
                              "Large windows sit either side of the door to the black and white timber building. The inn carries on to the west where the stables reside. " +
                              "The dirt track of square walk continues west and east towards the general store.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                type = Room.RoomType.Guild

            };

            #endregion




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
                locked = false
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
                locked = false
            };


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false,

            };


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion


            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);

            var lance = Lance.VillageElderLance();

            lance.Recall = new Recall()
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            room.mobs.Add(lance);




            return room;
        }

        public static Room SquareWalkOutsideStables()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 3,
                title = "Square walk, outside the stables of the Red Lion",
                description = "<p>This corner of Square walk gives access to the stables of the Red lion. Mainly used by travelers to house their mounts." +
                              "bits of hay and manure litter the northern entrance to the stables. Square walk continues south and east to the entrance of The Red Lion. </p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 14,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };




            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);



            return room;
        }

        public static Room SquareWalkWestOfCentre()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 4,
                title = "Square walk, west of the centre",
                description = "<p>This part of the square walk leads north to the Red Lion stables, the large village centre green is to the east. The dirt track continues south. Small wild flowers dot the grass either side of the dirt track. To the west you see rolling green hills off in to the distance.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false
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
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 5,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };





            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);



            return room;
        }

        public static Room SquareWalkSouthWestOfCentre()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 5,
                title = "Square walk, south west of the centre",
                description = "<p>This dirt track here leads north and curves here to the west. A lantern has been placed on the corner patch of grass to the light at night. Wild flowers dot either side of the dusty path. In the distance towards the south you see the back of some houses.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 6,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };





            #endregion
            room.exits.Add(north);
            room.exits.Add(east);



            return room;
        }

        public static Room SquareWalkSouthOfCentre()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 6,
                title = "Square walk, south of the centre",
                description = "<p>The centre of the square is to the north a large green space enjoyed by everyone who comes to Anker. Wild flowers dot the perimeter of the dirt track. East and west continues to the square walk. A lantern is by the north path to light the way at night. You see the rear of some houses towards the south in the distance.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 5,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };



            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }

        public static Room SquareWalkEntrance()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 7,
                title = "Square walk, Entrance",
                description = "<p>A smooth grey arched stone with writing inscribed sits to the side of the path leading north towards the general store and inn." +
                              "The centre of the square is towards the north west. The packed dirt path also leads east towards the Temple and continues west along Square walk.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var stone = new RoomObject
            {
                name = "smooth grey arched stone",
                look = "The stone reads: Welcome to Anker. The world is a book, and those who don't travel only read one page.",
                examine = "The stone reads: Welcome to Anker. The world is a book, and those who don't travel only read one page.",
                touch = "The stone fills rough to touch"
            };


            var idiot = new Player
            {
                Name = "The Village idiot",
                NPCLongName = "The Village idiot",
                KnownByName = true,
                Region = "Anker",
                Area = "Anker",
                AreaId = 7,
                Type = Player.PlayerTypes.Mob,
                HitPoints = 120,
                MaxHitPoints = 120,
                Level = 3,
                Strength = 30,
                Dexterity = 30,
                Constitution = 30,
                Wisdom = 30,
                Intelligence = 30,
                Charisma = 30,
                MaxStrength = 30,
                MaxDexterity = 30,
                MaxConstitution = 30,
                MaxWisdom = 30,
                MaxIntelligence = 30,
                MaxCharisma = 30,
                Recall = new Recall
                {
                    Region = "Anker",
                    Area = "Anker",
                    AreaId = 7
                },
                Gold = 10, 

            };


            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 8,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 15,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 19,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 6,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };




            room.keywords.Add(stone);
            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);
            room.mobs.Add(idiot);


            return room;
        }

        public static Room SquareWalkEastOfCentre()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 8,
                title = "Square walk, east of the centre",
                description = "<p>The most frequent path of square walk with the centre to the west and the large Village hall to the east the only stone building in Anker. " +
                              "Visited by any who seek the village Elder for wisdom and advice " +
                              "The path continues north towards the General store and the black smith. south leads to the entrance and the church.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 0,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };





            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(west);
            room.exits.Add(south);



            return room;
        }

        public static Room SquareWalkCommerceCorner()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 9,
                title = "Square walk, commerce corner",
                description = "<p>The only shop in Anker is to the north providing everything from food, clothing and basic adventuring equipment. " +
                              "To the east is the black smith providing basic metal work for the village. The dirt path runs west towards to the Red Lion inn and south to Square walk.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 12,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 13,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 8,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };





            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);




            return room;
        }

        public static Room GeneralStore()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 12,
                title = "Odds and sods shoppe",
                description = "<p>Candle light fills the shop with a warm glow, flickering off the wooden walls adding a shine to the various items sitting on the shelfs around the shop. A carpentry work bench can be found to the west of the room." +
                              "A neat pile of mismatch garments sit folded on a table in the centre of the room.</p> " +
                              "<p>To the north you see a closed door behind a wooden counter and a few old looking weapons" +
                              " hanging overhead on the wall. On the counter is a sign and a large red leather book, " +
                              "a couple of candles rest on the counter along side an ink pot and quill. " +
                              "The door south leads out to the Square walk.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    new Item.Item()
                    {
                        name =  "Carpentry work bench",
                        isHiddenInRoom = true,
                        container = true,
                        containerSize = 11,
                        description = new Description()
                        {
                            look = "Various wood working tools such as saws, chisels and oil lay upon the desk ready for use.",

                            exam =  "Various wood working tools such as saws, chisels and oil lay upon the desk ready for use.",
                        },
                        location = Item.Item.ItemLocation.Room,
                        stuck = true,
                        containerItems = new ItemContainer()
                        {
                          
                        }


                    }
                },
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                type = Room.RoomType.Shop
                

            };


            var sign = new RoomObject
            {
                name = "Sign",
                look = "Welcome to Odds and sods, Let me know if you want me to list my wares but don't touch anything.",
                examine = "Welcome to Odds and sods, Let me know if you want me to list my wares but don't touch anything.",

            };


            room.keywords.Add(sign);

            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };






            #endregion

            room.exits.Add(south);

            room.mobs.Add(Anika.OddsNSodsShopKeeper());



            return room;
        }


        public static Room MetalMedley()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 13,
                title = "Metal Medley",
                description = "<p>A shovel protrudes from a mountain of coal leaning against a large roaring forge of hot yellow flames. Sacks of raw materials and broken metalwork rest in the corner off to the left.</p>" +

                              "<p> Not far from the forge is a large anvil and workbench. Aprons, metal tongs, hammers, chisels and a sledge hammer rest on the table in easy reach of the anvil. Racks fill the eastern wall housing an assortment of swords, daggers and spears. The other side are suits of basic chain mail armour and various sizes of shields on display either side of the exit leading west to commerce corner.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    new Item.Item()
                    {
                        name =  "Furnace",
                        hidden = true, 
                        isHiddenInRoom = true,
                        description = new Description()
                        {
                            look = "A large roaring forge of hot yellow flames."
                        },
                        location = Item.Item.ItemLocation.Room,
                        stuck = true,
 
                    }
                },
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                type = Room.RoomType.Shop

            };


            //var sm = Held.ScrapMetal();
            //    sm.location = Item.Item.ItemLocation.Room;

            //MetalMedley().items.Add(sm);
   

            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 9,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };






            #endregion

            room.exits.Add(west);

            var ferron = Ferron.MetalMedleyBlacksmith();

            ferron.Recall = new Recall()
            {
                Area = room.area,
                Region = room.region,
                AreaId = room.areaId
            };

            room.mobs.Add(ferron);



            return room;
        }

        public static Room DrunkenSailor()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 2,
                title = "The Red Lion",
                description = "The inside of the tavern is a single, low-roofed room. Rancid oil lamps emit a gloomy light." +
                " Only a handful of people can be seen through the smoke-filled air. A small door to the west leads out to the stables." +
                " A bad-tempered looking barkeeper seems to be cleaning the counter. A large door south leads out to Square walk",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            var modo = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Modo",
                NPCLongName = "Modo",
                KnownByName = true,
                Type = Player.PlayerTypes.Mob,
                Description = "The owner of The Red Lion is a tall and intimidating appearance. This long-bearded man immediately makes you feel uncomfortable. He does not seem to notice you.",
                Strength = 60,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 2000,
                HitPoints = 2000,
                Level = 50,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Dialogue = new List<Responses>(),
                Emotes = new List<string>(),
                Shop = true,
                itemsToSell = new List<Item.Item>(),
                sellerMessage = "Why of course, here is what I can sell you.",
                Recall = new Recall()
                {
                    Area = room.area,
                    Region = room.region,
                    AreaId = room.areaId
                }


        };

            var beer = new Item.Item
            {
                name = "Light Beer",
                type = Item.Item.ItemType.Drink,
                Gold = 3,
                description = new Description
                {
                    look = "A weak looking flat beer bubbles in a bottle",
                    room = "A beer has been left on the floor"
                },
                slot = Item.Item.EqSlot.Hands
            };


            modo.itemsToSell.Add(beer);

            var beerTalk = new Responses
            {
                Keyword = new List<string>
                {
                    "beer",
                    "drink",
                     "ale",
                },
                Response = "Beer? What would you like?"
            };

            var helloTalk = new Responses
            {
                Keyword = new List<string>
                {
                    "hello",
                    "hi",
                     "greetings",
                },
                Response = "Hello there to you too $playerName come, grab a seat and I'll get you a Beer"
            };



            modo.Dialogue.Add(beerTalk);

            modo.Emotes.Add("wipes down the bar keeping it clean");
            modo.Emotes.Add("grabs a wet glass and starts drying it with a stained towel");
            modo.Emotes.Add("strokes his beard and looks around");
            modo.Emotes.Add("anyone for a pint?");

            modo.Dialogue.Add(helloTalk);

            var dyten = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Dyten",
                NPCLongName = "Dyten",
                KnownByName = true,
                Type = Player.PlayerTypes.Mob,
                Description = "This weathered old man probably never leaves this place. His cloudy eyes seem to seek something at the bottom of his glass.",
                Strength = 60,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 1000,
                HitPoints = 1000,
                Level = 30,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Emotes = new List<string>()
            };

            dyten.Emotes.Add("picks up his glass and stares down into it");
            dyten.Emotes.Add("grabs his glass and lifts it to his lips taking a large gulp of beer");

            var recall = new Recall
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            modo.Recall = recall;
            dyten.Recall = recall;



            room.mobs.Add(modo);
            room.mobs.Add(dyten);
            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 14,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion

            room.exits.Add(west);
            room.exits.Add(south);

            var counter = new RoomObject
            {
                name = "Wooden Counter",
                look = "The surface is full of suspicious smudges. You better not touch it.",
                examine = "There is nothing more of detail to see.",
                touch = "The wood feels sticky.",
                smell = "It smells like endless nights of drinking and smoking."
            };

            var table = new RoomObject
            {
                name = "A sturdy table",
                look = "A small lamp is placed in its center. Scratches tell of wild nights in the past.",
                examine = "There is nothing more of detail to see.",
                touch = "The wood feels sticky.",
                smell = "It smells like endless nights of drinking and smoking."
            };


            room.keywords.Add(table);
            room.keywords.Add(counter);



            return room;
        }

        public static Room RedLionStables()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 14,
                title = "Stables of The Red Lion",
                description = "Hay scatter the floor here with the occasional mound of manure, Several posts under a roof allow you to secure a horse here." +
                              "To the east a small door leads in to the Red Lion. South leads out from the wide doors to Square walk.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var stableBoy = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Stable boy",
                Type = Player.PlayerTypes.Mob,
                Description = "A rough dirty looking stable boy",
                Strength = 60,
                Dexterity = 80,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 700,
                HitPoints = 700,
                Level = 5,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer()
            };

            var blackhorse = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Sleek Black Horse",
                Type = Player.PlayerTypes.Mob,
                Description = "A sleek strong looking black horse",
                Strength = 72,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 750,
                HitPoints = 750,
                Level = 15,
                Status = Player.PlayerStatus.Busy,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                IsMount = true
            };

            var recall = new Recall
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            stableBoy.Recall = recall;
            blackhorse.Recall = recall;



            room.mobs.Add(stableBoy);
            room.mobs.Add(blackhorse);
            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 2,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
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

        public static Room VillageHallEntrance()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 10,
                title = "Village Hall Entrance",
                description = "A large circular window above the western double doors gives a grand appearance to the empty entrance way." +
                              " The floor is paved with smooth sandstone leading further inside under an arch way to the east." +
                              " A large sign sits on a wooden desk beside the door.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 11,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 8,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion

            room.exits.Add(east);
            room.exits.Add(west);




            return room;
        }

        public static Room VillageHall()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 11,
                title = "The Village Hall",
                description = "High back chairs surround a circular table that fills most of the room." +
                              " This is a meeting point for the villagers and the Elder to discuss issues surrounding Anker." +
                              " Torches hang either side of the archway. A wooden door is to the north, a plaque has been added to the door.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 18,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion

            room.exits.Add(north);
            room.exits.Add(west);




            return room;
        }

        public static Room VillageHallEldersRoom()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 18,
                title = "The Village Hall, Elder Chamber",
                description = "A grand wooden desk faces the entrance taking over most of the room." +
                              " A large sword is attached to the wall above the desk. To the east a bookcase covers the wall filled with books of all sizes and colours." +
                              " A fabric cloth hangs on the western wall bearing the crest of Anker. Torches wither side of the door light the room.",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 15,
                keywords = new List<string>(),
                hidden = true,
                locked = false,
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 11,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            #endregion

            //  room.exits.Add(east);
            room.exits.Add(south);




            return room;
        }

        public static Room TempleRoad()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 15,
                title = "Temple Walk",
                description = "<p>The path here leads east at an incline through a hill, the path curving up towards the temple of Tyr. To the West is the entrance to the square where the hustle and bustle of the town erupts into a symphony of sound, as the townsfolk go about their daily routine.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 42,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            #endregion
            room.exits.Add(east);
            room.exits.Add(west);

            return room;
        }

        public static Room TempleRoad3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 42,
                title = "Temple Walk",
                description = "<p>The path here continues east, the incline getting steeper the higher you go towards the temple of Tyr. To the west is the path leading towards the entrance to the square. High up on the pathway the top of the tree line shows itself, revealing distant landmarks, to the north in the distance appears to be some kind of large house and to the east are fires burning from a village in the forest.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 15,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 16,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            #endregion
            room.exits.Add(east);
            room.exits.Add(west);

            return room;
        }

        public static Room TempleRoad4()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 43,
                title = "Temple Walk",
                description = "<p>The muddy path slopes down cut into the hill snaking round steeply. To the north is the temple of Tyr. To the west the path even outs and leads through to Anker.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 16,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var north = new Exit
            {
                name = "North",
                area = "Anker",
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

        public static Room TempleRoad2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 16,
                title = "Temple Walk",
                description = "<p>The path here continues east, the incline getting steeper the higher you go towards the temple of Tyr. To the west is the path sloping down towards the village. A small puddle of water has collected here in a dip in the path as water slowly runs down from higher up the walkway.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 42,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 43,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            #endregion
            room.exits.Add(east);
            room.exits.Add(west);
         

            return room;
        }

        public static Room TempleEntrance()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 17,
                title = "Temple Entrance",
                description = "<p>The temple of Tyr watches over Anker from the large hill it rests on. Two monolithic support columns stand either side of the temple archway entrance. There is no doors to the northern entrance so you can see right inside the golden glow of the temple chamber. To the east and west is a path that leads around the temple and it's graves. To the south is the path leading down the hill.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };


            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 43,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 65,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 47,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 44,
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

        public static Room TempleGraveyard()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 47,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the west is the entrance to the Temple of Tyr, to the north the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


        
            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 46,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var west = new Exit
            {
                name = "West",
                area = "Anker",
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

        public static Room TempleGraveyard1()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 46,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the north and south the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 47,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 64,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
 


            #endregion
            room.exits.Add(north);
           
            room.exits.Add(south);
       


            return room;
        }

        public static Room TempleGraveyard2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 64,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the west and south the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 63,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 46,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(west);

            room.exits.Add(south);



            return room;
        }


        public static Room TempleGraveyard3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 63,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the east and west the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 64,
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
                areaId = 62,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(east);

            room.exits.Add(west);


            
            return room;
        }

        public static Room TempleGraveyard4()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 62,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the east and south the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 63,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 45,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(east);

            room.exits.Add(south);



            return room;
        }

        public static Room TempleGraveyard5()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 45,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the north and south the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 62,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 44,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(north);

            room.exits.Add(south);



            return room;
        }

        public static Room TempleGraveyard6()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 44,
                title = "Temple Graveyard",
                description = "<p>Gravestones new and old stand facing the dirt path on either side. The older headstones are covered in mould, some are cracked or crumbling. Alot of the engravings are hard to read. To the north and east the path continues through the graveyard.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "Gravestone"
                    }
                },
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "You feel someone is watching you.",
                    "You feel a hand touch your shoulder.",
                    "You hear children giggling."
                }

            };


            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 45,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 17,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(north);

            room.exits.Add(east);



            return room;
        }



        public static Room TempleOfTyr()
        {

            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 65,
                title = "Temple of Tyr",
                description = "<p>A circular blue mosaic covers the centre of the temple with a gold fist and star underneath in the centre. Above is a dome roof with yellow tinted glass giving the area a golden glow. An Alter to Tyr is at the back with a large blue banner hanging from the wall with the same golden fist above the star. To the south the entrance to the Temple</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true,

            };

            var mortem = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Mortem",
                NPCLongName = "Mortem",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "A blue cape of Tyr hangs down Mortems back who is covered in full plate mail except for his heads and hands. A golden mace hangs upside down from his belt.",
                Strength = 72,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 70,
                Wisdom = 70,
                Charisma = 60,
                MaxHitPoints = 2890,
                HitPoints = 2890,
                Level = 51,
                Gold = 8050,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                DialogueTree = new List<DialogTree>(),
                Greet = false,
                Emotes = new List<string>(),
                EventOnComunicate = new Dictionary<string, string>(),
                EventWake = "awakening awake",
                EventWear = "awakening awake",
                Recall = new Recall()
                {
                    Area = room.area,
                    AreaId = room.areaId,
                    Region = room.region
                }

            };

           
            var breastPlateTyr = FullPlateBody.BreastPlateOfTyr();
            breastPlateTyr.location = Item.Item.ItemLocation.Worn;

 
            mortem.Inventory.Add(breastPlateTyr);

            mortem.Equipment.Body = breastPlateTyr.name;
 

            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 17,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            room.exits.Add(south);


            // create item for platemail / cape / mace / set to worn
            //top and trousers for player


            room.mobs.Add(mortem);

            return room;
        }


        //Anker Lane
        //TODO: confirm id, and find a better way to do that
        public static Room PathToTheSquare()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 19,
                title = "A path to the square",
                description = "<p>This dirt path connects the Anker Lane, the main road through the village to the busy village square." +
                              " The path is flanked by cut grass and a border of wild flowers. You can also see the backs of some of the villagers houses from here.</p>" +
                              "<p>To the north east in the distance upon a hill over looking Anker is the village temple.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 7,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            //TODO : create ANker lane to connect this to
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 20,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };



            #endregion
            room.exits.Add(north);
            room.exits.Add(south);

            room.mobs.Add(AnkerGuard.AnkerGuardNpc2());
     

            return room;
        }


        public static Room AnkerLane()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 20,
                title = "Middle of Anker Lane",
                description = "<p>The mud trodden path forks off to the north leading to the main hub of Anker. A tree towers over the fork in the road with a sign attached to it. The main path continues east and west containing houses in both directions.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var sign = new RoomObject()
            {
                name = "sign",
                look = "The sign reads: East to Anker Forest and farms, West to Ester.",
                examine = "The sign reads: East to Anker Forest and farms, West to Ester.",
                touch = "The sign is smooth but weathered wood with engraved writing"
            };

            room.keywords.Add(sign);



            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 19,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            // Create Exits
            //TODO : create ANker lane to connect this to
            var west = new Exit
            {
                name = "West",
                area = "Anker",
                region = "Anker",
                areaId = 21,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };

            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 22,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
            };


            //TODO add exits east and west


            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(west);


            return room;
        }

        #region westAnker
        public static Room AnkerLaneWest21()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 21,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The occasional chimney puffs smoke up in the air.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 28,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 20,
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
                areaId = 25,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 29,
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

        public static Room AnkerHome()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 28,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 28,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var bed = new Item.Item()
            {
                name = "bed",
                description = new Description()
                {
                    room = "A small bed up against the right wall",
                    look = "The bed has a simple cotton blanket and a simple pillow"
                },
                stuck = true
            };

            room.items.Add(bed);
 
            room.mobs.Add(villager);
            room.mobs.Add(Alice.AliceNpc());
 

            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 21,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);

            // room.exits.Add(south);


            return room;
        }


        public static Room AnkerHome2()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 29,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 29,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 29,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 21,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);

            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerLaneWest25()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 25,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The occasional chimney puffs smoke up in the air.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
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
                areaId = 26,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 21,
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
                areaId = 37,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 27,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };




            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);
            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome3()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 26,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 26,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 26,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager3 = new Player()
            {
                Name = "Little boy",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 5,
                HitPoints = 60,
                MaxHitPoints = 60,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A little boy",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 26,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);
            room.mobs.Add(villager3);
            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 25,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);

            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome4()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 27,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 27,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 27,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager3 = new Player()
            {
                Name = "Little girl",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 5,
                HitPoints = 60,
                MaxHitPoints = 60,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A little girl",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 27,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);
            room.mobs.Add(villager3);
            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 25,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);

            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerLaneWest37()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 37,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The occasional chimney puffs smoke up in the air. The cottage to the east here looks derelict.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            room.mobs.Add(AnkerGuard.AnkerGuardNpc());


            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 25,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 38,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 39,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };



            #endregion
            room.exits.Add(north);

            room.exits.Add(east);

            room.exits.Add(south);


            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome6()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 38,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 38,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 38,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager3 = new Player()
            {
                Name = "Little boy",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 5,
                HitPoints = 60,
                MaxHitPoints = 60,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A little boy",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 38,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);
            room.mobs.Add(villager3);
            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 37,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);

            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome5()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 39,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 39,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 39,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager3 = new Player()
            {
                Name = "Little girl",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 5,
                HitPoints = 60,
                MaxHitPoints = 60,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A little girl",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 39,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);
            room.mobs.Add(villager3);
            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 37,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);

            // room.exits.Add(south);


            return room;
        }
        #endregion
        #region EastAnker
        public static Room AnkerLaneEast22()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 22,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The house to the north seems to be puffing purple smoke from it's chimney unlike the others.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };




            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 23,
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
                areaId = 20,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 31,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var South = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 32,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(South);
            room.exits.Add(west);


            return room;
        }

        public static Room AnkerHome31()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 31,
                title = "An Alchemists home",
                description = "<p>A large ornate rug covers the wooden floor of the room, In the centre a cauldron boils and bubbles away. Tall shelving units line almost every wall, All filled with vials of all shapes, sizes and colours. The older trinkets on the shelves are covered in dusty cobwebs. A few thick leather books also sit upon the shelves. To the left is what looks like the alchemists work station by the mortor and pestle and empty vials. Else where in the room you notice a couple of purple cusions that are used for sitting but no sign of any bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>()
                {
                    new Item.Item()
                    {
                      name =  "Alchemists work bench",
                      isHiddenInRoom = true,
                      container = true,
                      containerSize = 11,
                      description = new Description()
                      {
                          look = "Various glass tubes and flasks sit upon the desk, several candles heat a suspended vial causing the liquid to evaporate up the tube and decant into a flask further along the desk. " +
                                 "A mortar and pestle used for grinding fresh ingridents sits beside an open alchemy book.",

                          exam = "Various glass tubes and flasks sit upon the desk, several candles heat a suspended vial causing the liquid to evaporate up the tube and decant into a flask further along the desk. " +
                                 "A mortar and pestle used for grinding fresh ingridents sits beside an open alchemy book."
                      },
                      location = Item.Item.ItemLocation.Room,
                      stuck = true,
                      containerItems = new ItemContainer()
                      {
                          new Item.Item()
                          {
                              name =  "Aloe Vera",
                              description = new Description()
                              {
                                  look = "A thick green spikey branch of aloe vera, a clear liquid oozes from where it's been cut at the base.",
                              },
                              location = Item.Item.ItemLocation.Room,
                            
                          },
                          new Item.Item()
                          {
                              name =  "Scabious",
                              description = new Description()
                              {
                                  look = "A beautiful purple flower with a long green stem.",
                                  smell = "You sniff strongly at the flower, it's smell is pleasant."
                              },
                              location = Item.Item.ItemLocation.Room,

                          }
                          ,
                           drink.Flask()
                      }
                      

                    }
                },
                mobs = new List<Player>(),
                terrain = Room.Terrain.Inside,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The cauldron bubbles away",
                    "Squeak..."

                }

            };

            var villager = new Player()
            {
                Name = "Ramon Llull",
                KnownByName = true,
                NPCLongName = "Ramon Llull the Alchemist is here, staring at vials of different shapes and colours.",
                Gender = "Male",
                Strength = 60,
                Dexterity = 60,
                Constitution = 60,
                Wisdom = 60,
                Intelligence = 60,
                Charisma = 60,
                Level = 14,
                HitPoints = 356,
                MaxHitPoints = 356,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "An tall bald man with a large white beard is here. A simple light red robe covers his body.",
                Type = Player.PlayerTypes.Mob,
                Emotes = new List<string>()
                {
                    "Says, Hmm, where did I put my glasses?"
                }

            };
 

            room.mobs.Add(villager);
 

            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 22,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome32()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 32,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 22,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);


            return room;
        }

        public static Room AnkerLaneEast23()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 23,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The occasional chimney puffs smoke up in the air.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };




            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 24,
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
                areaId = 22,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 33,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var South = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 34,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            #endregion
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(South);
            room.exits.Add(west);
            // room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome33()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 33,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 23,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome34()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 34,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 23,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);


            return room;
        }


        public static Room AnkerLaneEast24()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 24,
                title = "Anker Lane",
                description = "<p>The wide path runs through the heart of Anker formed from years of use. " +
                              "Small cottages line the road on both sides and continue" +
                              "to do so both east and west. Wild flowers line most of the" +
                              " path with a few small trees here and there. " +
                              "The occasional chimney puffs smoke up in the air.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };




            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 40,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 35,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };


            // Create Exits
            var South = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 36,
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
                areaId = 23,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };




            #endregion
            room.exits.Add(east);
            room.exits.Add(north);
            room.exits.Add(South);
            room.exits.Add(west);



            return room;
        }
        #endregion

        public static Room AnkerHome35()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 35,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 35,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 35,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var south = new Exit
            {
                name = "South",
                area = "Anker",
                region = "Anker",
                areaId = 24,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(south);


            return room;
        }

        public static Room AnkerHome36()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 36,
                title = "A small cosy home",
                description = "<p>A large rug covers the wooden floor of the room, to the left is a couple of chairs in front of a small fireplace warming the room. To the right is a small bed.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,
                Emotes = new List<string>()
                {
                    "The wood in the burning fireplace crackles and pops."
                }

            };

            var villager = new Player()
            {
                Name = "Male villager",
                Gender = "Male",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 7,
                HitPoints = 156,
                MaxHitPoints = 156,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A male villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 36,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            var villager2 = new Player()
            {
                Name = "Female villager",
                Gender = "Female",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Wisdom = 12,
                Intelligence = 12,
                Charisma = 12,
                Level = 10,
                HitPoints = 210,
                MaxHitPoints = 210,
                MovePoints = 200,
                MaxMovePoints = 200,
                Description = "A female villager",
                Type = Player.PlayerTypes.Mob,
                Recall = new Recall()
                {
                    AreaId = 36,
                    Area = "Anker",
                    Region = "Anker"
                }

            };

            room.mobs.Add(villager);
            room.mobs.Add(villager2);

            #region exits


            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Anker",
                region = "Anker",
                areaId = 24,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };
            #endregion
            // Create Exits

            room.exits.Add(north);


            return room;
        }


        public static Room AnkerLaneEast40()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 40,
                title = "Anker Lane west of the gate",
                description = "<p>The wide path here dotted by wild flowers and trees leads east towards the arched gate of anker. To the west the path leads through the homes of Anker. In the far distance you see the path turn north.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };




            #region exits


            // Create Exits
            var east = new Exit
            {
                name = "East",
                area = "Anker",
                region = "Anker",
                areaId = 41,
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
                areaId = 24,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };

 


            #endregion
            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }

        public static Room AnkerLaneGate()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 41,
                title = "Arched gate of Anker",
                description = "<p>The dirt road leads through the grey stoned archway which towers over the road. The archway signals the start of Anker as there is no gate to stop intruders from entering. Some engravings and carved into the stone. The Anker farm is to the east, further in the distance you can make out a forest of some kind. The western path through the archway leads to Anker.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.City,
                keywords = new List<RoomObject>()
                {
                    new RoomObject()
                    {
                        name = "engravings",
                        look = "The engraving reads: \"Welcome to Anker\"",
                        examine = "Looking closely at the engraving you see: \"Welcome to Anker\" underneath in small writing it reads: \"Malleus and Gamia was here XVI-X-MMXVII\"."
                    }
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
                areaId = 0,
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
                areaId = 40,
                keywords = new List<string>(),
                hidden = false,
                locked = false
            };




            #endregion
            room.exits.Add(east);
            room.exits.Add(west);



            return room;
        }

 
    }
}