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
    public class Alice
    {
        /// <summary>
        /// alice walks from her home to the shop to talk to anika before buying a pie
        /// she then walks back home and puts the pie o nthe table, before sitting at the table
        /// and eating the pie.
        /// She then goes to sleep and repeats
        /// </summary>
        /// <returns></returns>
        public static PlayerSetup.Player AliceNpc()
        {


            var Alice = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Alice",
                NPCLongName = "Alice",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "You see nothing special about her.",
                Strength = 15,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 12,
                Wisdom = 16,
                Charisma = 18,
                MaxHitPoints = 350,
                HitPoints = 350,
                Level = 10,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
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
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker",
                    AreaId = 28
                },
                PathList = new List<string>()
                {
                    "s",
                    "e",
                    "n",
                    "n",
                    "n",
                    "n",
                    "n",
                    //reverse
                    "s",
                    "s",
                    "s",
                    "s",
                    "s",
                    "w",
                    "n"

                }


            };







            return Alice;
        }
    }
}