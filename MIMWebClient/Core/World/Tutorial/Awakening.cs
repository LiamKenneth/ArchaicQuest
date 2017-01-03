using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                description = "<p>A circular blue mosaic covers the centre of the temple with a gold fist and star underneath in the centre. Above is a dome roof with yellow tinted glass giving the area a golden glow. An Alter to Tyr is at the back with a large blue banner behind hanging from the wall with the same golden fist above the star. To the south the entrance to the Temple</p>",

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

            return room;
        }
    }
}