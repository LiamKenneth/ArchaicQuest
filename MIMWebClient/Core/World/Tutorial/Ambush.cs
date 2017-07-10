using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;
using Action = MIMWebClient.Core.Item.Action;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Ambush
    {
        public static Room.Room TutorialRoom1()
        {

    
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
                EventLook = "tutorial",


            };

        
            room.Emotes.Add("You hear the sound of bushes and trees rustling all around you");
            room.Emotes.Add("*SNAP* You hear the distinctive sound of a stick snapping");
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
                KnownByName = true,
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
                Inventory = new List<Item.Item>(),
                DialogueTree = new List<DialogTree>(),
                Greet = false,
                GreetMessage = "I don't think we have much further to go, ",
                Emotes = new List<string>(),
                EventOnComunicate = new Dictionary<string, string>(),
                  EventOnEnter = "tutorial"

            };

            wilhelm.Emotes.Add("looks around nervously");
        
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

            var didYouHearThat = new DialogTree()
            {
                Id = "tut1",
                Message = "Did you hear that?",
                PossibleResponse = new List<Responses>(),
                
            };

            var tut1a = new Responses()
            {
                QuestionId = "tut1",
                Response = "It sounded like a twig"
            };

            var tut1b = new Responses()
            {
                QuestionId = "tut1",
                Response = "No you're just hearing things"
            };


            didYouHearThat.PossibleResponse.Add(tut1a);
            didYouHearThat.PossibleResponse.Add(tut1b);

          //  wilhelm.DialogueTree.Add(didYouHearThat);Fw


         
            wilhelm.EventOnComunicate.Add("tutorial", "yes");

            var attack = new Responses()
            {
                QuestionId = "tut1",
                MatchPhrase = tut1a.Response,               
                Response = "AAAHG-ATACK!!!!",
                Keyword = new List<string>()
               
            };

            attack.Keyword.Add(tut1a.Response);

            var attackb = new Responses()
            {
                QuestionId = "tut1",
                MatchPhrase = tut1b.Response,
                Response = "AAAHG-ATACK!!!!",
                Keyword = new List<string>()

            };

            attackb.Keyword.Add(tut1b.Response);

         
            room.exits.Add(north);
          
            room.mobs.Add(wilhelm);         

            return room;
        }

        public static Room.Room TutorialRoom2()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 1,
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
                EventOnEnter = "rescue"


            };
 
     

            return room;
        }
    }
}