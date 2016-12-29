using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

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
                terrain = Room.Terrain.Field,
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
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true,


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
            var woodenChestObj = new Item.Item
            {

                name = "Wooden Chest",
                containerItems = new List<Item.Item>(),
                canLock = true,
                containerSize = 10,
                container = true,
                location = Item.Item.ItemLocation.Room,
                description = new Item.Description { look = "Small Chest by the well" },
                open = false,
                canOpen = true,
                locked = true,
                keyId = Guid.NewGuid().ToString(),
                stuck = true
            };

            woodenChestObj.keyValue = woodenChestObj.keyId;
            room.items.Add(woodenChestObj);


            var oddKey = new Item.Item
            {

                name = "Odd looking key",
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
            bucketObj.isHiddenInRoom = true;
            bucketObj.name = "bucket";

            bucketGold.count = 75;
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
                Type = Player.PlayerTypes.Mob,
                Description = "This black cat's fur looks in pristine condition despite being a stray.",

                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 1,
                Wisdom = 1,
                Charisma = 1,
                MaxHitPoints = 50,
                HitPoints = 50,
                Level = 2,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>()


            };

            var cat2 = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Black and White cat",
                Type = Player.PlayerTypes.Mob,
                Description = "This black cat's fur looks in pristine condition despite being a stray.",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 1,
                Wisdom = 1,
                Charisma = 1,
                MaxHitPoints = 50,
                HitPoints = 50,
                Level = 2,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>()


            };


            var dagger = new Item.Item
            {
                actions = new Item.Action(),
                name = "Blunt dagger",
                eqSlot = Item.Item.EqSlot.Wield,
                weaponType = Item.Item.WeaponType.ShortBlades,
                stats = new Item.Stats { damMin = 2, damMax = 4, minUsageLevel = 1 },
                type = Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Item.Item.AttackType.Pierce,
                slot = Item.Item.EqSlot.Wield,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description(),

            };

            dagger.description.look = "This is just a blunt dagger";
            dagger.description.exam = "This is an extremly blunt dagger";




            var dagger2 = new Item.Item
            {
                actions = new Item.Action(),
                name = "Flaming dagger",
                eqSlot = Item.Item.EqSlot.Wield,
                weaponType = Item.Item.WeaponType.ShortBlades,
                stats = new Item.Stats { damMin = 21, damMax = 44, minUsageLevel = 1 },
                type = Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Item.Item.AttackType.Pierce,
                slot = Item.Item.EqSlot.Wield,
                location = Item.Item.ItemLocation.Inventory
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
            room.mobs.Add(cat);


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
                terrain = Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<Player>(),
                players = new List<Player>(),
                fighting = new List<string>(),
                clean = true

            };

            #endregion

            #region NPC setup
            var trainer = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Lance",
                KnownByName = true,
                Type = Player.PlayerTypes.Mob,
                Description = "The elder of the village anker",
                Strength = 15,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 12,
                Wisdom = 16,
                Charisma = 18,
                MaxHitPoints = 350,
                HitPoints = 350,
                Level = 15,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
                Trainer = true,
                Greet = true,
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


            };

            #endregion


            var welcomePlayers = new DialogTree
            {
                Id = "lance1",
                Message = "Greetings and welcome to Anker, I am the village Elder and I can help you if you need it.",
                PossibleResponse = new List<Responses>()
            };

            var lance1a = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1a",
                Response = "What is this place?"
            };

            var lance1b = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1b",
                Response = "What is there to do?"
            };

            var lance1c = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1c",
                Response = "Help I am stuck and confused"
            };

            var lance1d = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1d",
                Response = "It looks great, nice to meet you."
            };

            var lance1e = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1e",
                Response = "Anything I can help you with?",
                QuestId = 1
                
            };

            var lance1aAnswer = new DialogTree()
            {
               Id = "lance1a",
                MatchPhrase = lance1a.Response,
                Message = "This is Anker a small fishing village, the 1st of many interesting places built on this world. It will take time but this place will feel alive soon. I am talking aien't I?",
                PossibleResponse = new List<Responses>()
                 
            };

            lance1aAnswer.PossibleResponse.Add(lance1a);
            lance1aAnswer.PossibleResponse.Add(lance1b);
            lance1aAnswer.PossibleResponse.Add(lance1c);
            lance1aAnswer.PossibleResponse.Add(lance1d);
            lance1aAnswer.PossibleResponse.Add(lance1e);

            var lance1bAnswer = new DialogTree()
            {
                Id = "lance1b",
                MatchPhrase = lance1b.Response,
                Message = "Hmmm... The world is young $playerName but you can go north to the inn, Modo and Dysten may surprise you. North East houses the General shoppe and the blacksmith. Other than that you can walk around square walk. More changes happen everyday...",
                PossibleResponse = new List<Responses>()

            };

            lance1bAnswer.PossibleResponse.Add(lance1a);
            lance1bAnswer.PossibleResponse.Add(lance1b);
            lance1bAnswer.PossibleResponse.Add(lance1c);


            var lance1cAnswer = new DialogTree()
            {
                Id = "lance1c",
                MatchPhrase = lance1c.Response,
                Message = "Stuck? Oh, This can be a daunting place for newcommers. You know how to move right? you got here anyway. Move by typing the exit typically north, east, south etc can be shorten to just n,e,s,w. You can get or drop items. Wield or wear equipment and kill things. Stick to the cats.",
                PossibleResponse = new List<Responses>()

            };


            lance1cAnswer.PossibleResponse.Add(lance1a);
            lance1cAnswer.PossibleResponse.Add(lance1b);
            lance1cAnswer.PossibleResponse.Add(lance1c);

            var lance1dAnswer = new DialogTree()
            {
                Id = "lance1d",
                MatchPhrase = lance1d.Response,
                Message = "Thankyou $playerName, I am glad you like it. Run along now and enjoy your time here. The Gods will appreciate any feedback. Just send a prayer to liam.kenneth@hotmail.co.uk",
                PossibleResponse = new List<Responses>()

            };

            var lance1eAnswer = new DialogTree()
            {
                Id = "lance1d",
                MatchPhrase = lance1e.Response,
                Message = "Yes $playerName, I would like you to buy me a beer from Modo please. You may need some gold.",
                PossibleResponse = new List<Responses>(),
                GivePrerequisiteItem = true,
                GiveQuest = true,
                QuestId = 1


            };

            var whosModo = new DialogTree
            {
                Id = "lance2",
                Message = "He is the owner of the Red Lion, best beer in Anker I might add.",
                PossibleResponse = new List<Responses>()
            };

             

           
        
            trainer.DialogueTree.Add(welcomePlayers);
            trainer.DialogueTree.Add(lance1aAnswer);
            trainer.DialogueTree.Add(lance1cAnswer);
            trainer.DialogueTree.Add(lance1bAnswer);
            trainer.DialogueTree.Add(lance1dAnswer);
            trainer.DialogueTree.Add(lance1eAnswer);



            welcomePlayers.PossibleResponse.Add(lance1a);
            welcomePlayers.PossibleResponse.Add(lance1b);
            welcomePlayers.PossibleResponse.Add(lance1c);
            welcomePlayers.PossibleResponse.Add(lance1e);


            //quest

            var getBeer = new Quest()
            {
                Id = 1,
                Description = "I would like some beer from Modo please",
                Name = "Buy beer from modo and give to lance",
                Type = Quest.QuestType.Find,
                QuestItem = new Item.Item
                {
                    name = "Light Beer",
                    type = Item.Item.ItemType.Drink,
                    Gold = 3,
                    description = new Description
                    {
                        look = "A weak looking flat beer bubbles in a bottle",
                        room = "A beer has been left on the floor"
                    },
                    slot = Item.Item.EqSlot.Hand
                },
                RewardXp = 200,
                QuestGiver = trainer.Name,
                PrerequisiteItemEmote = "puts his hands in his money pounch and hands you some gold.",
                AlreadyOnQuestMessage = "Yeah I told you to get me some beer from Modo.",
                RewardDialog = new DialogTree()
                {
                    Message = "Ah Thank you so much!",
                    PossibleResponse = new List<Responses>()
                }



            };

            trainer.Quest.Add(getBeer);
             

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

            room.mobs.Add(trainer);




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
                description = "<p>This corner of Square walk gives access to the stables of the Red lion. Mainly used by travellers to house their mounts." +
                              "bits of hay and manure litter the northern entrance to the stables. Square walk continues south and east to the entrance of The Red Lion. </p>",

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
                description = "<p>This dirt track here leads north and curves here to the west. A latern has been placed on the corner patch of grass to the light at night. Wild flowers dot either side of the dusty path. In the distance towards the south you see the back of some houses.</p>",

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
                terrain = Room.Terrain.Field,
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
            //var east = new Exit
            //{
            //    name = "East",
            //    area = "Anker",
            //    region = "Anker",
            //    areaId = 10,
            //    keywords = new List<string>(),
            //    hidden = false,
            //    locked = false,
            //};

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
            //  room.exits.Add(east);
            room.exits.Add(west);



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
                description = "<p>Candle light fills the shop with a warm glow, flickering off the wooden walls adding a shine to the various items sitting on the shelfs around the shop. " +
                              "A neat pile of mismatch garments sit folded on a table in the centre of the room.</p> " +
                              "<p>To the north you see a closed door behind a wooden counter and a few old looking weapons" +
                              " hanging overhead on the wall. On the counter is a sign and a large red leather book, " +
                              "a couple of candles rest on the counter along side an ink pot and quill. " +
                              "The door south leads out to the Square walk.</p>",

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

                              "<p> Not far from the forge is a large anvil and workbench. Approns, metal tongs, hammers, chisels and a sledge hammer rest on the table in easy reach of the anvil. Racks fill the eastern wall housing an assortment of swords, daggers and spears. The other side are suits of basic chain mail armour and various sizes of shields on display either side of the exit leading west to commerce corner.</p>",

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
                KnownByName = true,
                Type = Player.PlayerTypes.Mob,
                Description = "The owner of The Red Lion is a tall and intimidating appearance. This long-bearded man immediatly makes you feel uncomfortable. He does not seem to notice you.",
                Strength = 15,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 9,
                Wisdom = 11,
                Charisma = 8,
                MaxHitPoints = 100,
                HitPoints = 100,
                Level = 10,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
                Dialogue = new List<Responses>(),
                Emotes = new List<string>(),
                Shop = true,
                itemsToSell = new List<Item.Item>(),
                sellerMessage = "Why of course, here is what I can sell you."
               

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
                slot = Item.Item.EqSlot.Hand
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
                Response = "Hello there to you too $playerName come, grab a seat and i'll get you a Beer"
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
                KnownByName = true,
                Type = Player.PlayerTypes.Mob,
                Description = "This weathered old man probably never leaves this place. His cloudy eyes seem to seek something at the bottom of his glass.",
                Strength = 1,
                Dexterity = 2,
                Constitution = 2,
                Intelligence = 2,
                Wisdom = 5,
                Charisma = 1,
                MaxHitPoints = 100,
                HitPoints = 100,
                Level = 1,
                Status = Player.PlayerStatus.Busy,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
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
                description = "Hay scatter the floor here with the occasional moud of manure, Several posts under a roof allow you to secure a horse here." +
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
                Strength = 11,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 9,
                Wisdom = 11,
                Charisma = 8,
                MaxHitPoints = 100,
                HitPoints = 100,
                Level = 5,
                Status = Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>()
            };

            var blackhorse = new Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Sleek Black Horse",
                Type = Player.PlayerTypes.Mob,
                Description = "A sleek strong looking black horse",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 12,
                Wisdom = 15,
                Charisma = 18,
                MaxHitPoints = 500,
                HitPoints = 500,
                Level = 15,
                Status = Player.PlayerStatus.Busy,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>()
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

            #endregion

            room.exits.Add(north);
            room.exits.Add(east);




            return room;
        }

        public static Room VillageHallEldersRoom()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 12,
                title = "The Village Hall, Elder Chamber",
                description = "A grand wooden desk faces the entrance taking over most of the room." +
                              " A large sword is attacheed to the wall above the desk. To the east a bookcase covers the wall filled with books of all sizes and colours." +
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

            room.exits.Add(east);
            room.exits.Add(south);




            return room;
        }

    }
}