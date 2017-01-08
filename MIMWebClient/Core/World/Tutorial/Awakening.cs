using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.World.Items.Clothing;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Awakening
    {

        public static Room.Room TempleOfTyr()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 3,
                title = "Temple of Tyr",
                description = "<p>A circular blue mosaic covers the centre of the temple with a gold fist and star underneath in the centre. Above is a dome roof with yellow tinted glass giving the area a golden glow. An Alter to Tyr is at the back with a large blue banner hanging from the wall with the same golden fist above the star. To the south the entrance to the Temple</p>",

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

            var mortem = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Mortem",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "A blue cape of Tyr hangs down Mortems back who is covered in full platemail except for his heads and hands. A golden mace hangs upside down from his belt.",
                Strength = 14,
                Dexterity = 16,
                Constitution = 18,
                Intelligence = 12,
                Wisdom = 18,
                Charisma = 14,
                MaxHitPoints = 300,
                HitPoints = 300,
                Level = 20,
                Gold = 450,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
                DialogueTree = new List<DialogTree>(),
                Greet = false,
                Emotes = new List<string>(),
                EventOnComunicate = new Dictionary<string, string>(),
                EventWake = "awakening awake"

            };

            var top = Clothing.PlainTop();

            mortem.Inventory.Add(top);

            var intro = new DialogTree()
            {
                GiveQuest = true,
                QuestId = 0,
                Message = ""
            };

              

            // create item for platemail / cape / mace / set to worn
            //top and trousers for player
           

            room.mobs.Add(mortem);

            return room;
        }
    }
}