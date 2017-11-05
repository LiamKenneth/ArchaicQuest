using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Feet;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Arms;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Body;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Hands;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Legs;
using MIMWebClient.Core.World.Items.Consumables.Drinks;
using MIMWebClient.Core.World.Items.Consumables.Food;
using MIMWebClient.Core.World.Items.MiscEQ.Light;
using MIMWebClient.Core.World.Items.Weapons.Axe;
using MIMWebClient.Core.World.Items.Weapons.Blunt;
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;
using MIMWebClient.Core.World.Items.Weapons.Staff;
using MIMWebClient.Core.World.Items.Weapons.Sword.Long;
using MIMWebClient.Core.World.Items.Weapons.Sword.Short;

namespace MIMWebClient.Core.World.Anker.Mobs
{
    public class Horik
    {
        /// <summary>
        /// alice walks from her home to the shop to talk to anika before buying a pie
        /// she then walks back home and puts the pie o nthe table, before sitting at the table
        /// and eating the pie.
        /// She then goes to sleep and repeats
        /// </summary>
        /// <returns></returns>
        public static PlayerSetup.Player HorikNpc()
        {


           return new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Horik",
                NPCLongName = "Horik the lumberjack",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "You see nothing special about him.",
                Strength = 70,
                Dexterity = 72,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 722,
                HitPoints = 722,
                Level = 23,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Trainer = false,
                Greet = true,
                Shop = false,
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>(),
                Region = "Anker",
                Area = "Anker",
                AreaId = 28,
                ArmorRating = 280,
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker Farm",
                    AreaId = 12
                }
           };

        }
    }
}