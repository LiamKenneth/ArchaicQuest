using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Ambush
    {
        public static Room.Room TutorialRoom1()
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

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 0,
                title = "Deep in the forest",
                description = "<p>Large trees tower high above all around you and Wilhelm as you tread lightly through the thick woods. No end to the forest is in sight, every way you look is endless amounts of bushes and trees with wide branches reaching out in everything direction blocking the sun.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
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
                area = "Tutorial",
                region = "Tutorial",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };
           


            //Create Mobs
            var wilhelm = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Wilhelm",
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "Wilhelm has matted black hair, sharp hazel eyes and a cropped beard. He wears chain mail and wields a short sword.",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 12,
                Wisdom = 12,
                Charisma = 12,
                MaxHitPoints = 150,
                HitPoints = 150,
                Level = 10,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
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
            dagger.description.exam = "This blunt dagger is better suited to buttering bread than killing";
          
            wilhelm.Inventory.Add(dagger);
           
            var recall = new Recall
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            wilhelm.Recall = recall;
            
            room.mobs.Add(wilhelm);         

            return room;
        }

        public static Room.Room TutorialRoom2()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 0,
                title = "Deep in the forest",
                description = "<p>You run forward, fleeing the ambush. You look back for one last time to see Wilhelm let out an ear piercing scream as he is over run by the goblins. " +
                              "Turning back around you greet a club that whacks you in the face sending you sprawling across the floor, your ears are ringing then everything goes black.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true,


            };
 
     

            return room;
        }
    }
}