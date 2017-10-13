using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Razor.Parser.SyntaxTree;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
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
    public class AnkerGuard
    {
        /// <summary>

        /// </summary>
        /// <returns></returns>
        public static PlayerSetup.Player AnkerGuardNpc()
        {


            var Guard = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Lara, the Anker guard",
                NPCLongName = "Lara the guard stands here keeping watch",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "Dark chain mail armour covers the guard head to toe. A long sword is sheathed at her waist",
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
                Skills = new List<Skill>()
                {
                    Dodge.DodgeAb(),
                    ShieldBlock.ShieldBlockAb(),
                    Parry.ParryAb(),
                    SecondAttack.SecondAttackAb(),
                   LongBlades.LongBladesAb(),

                },
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
                AreaId = 37,
                Gender = "Female",
                SelectedClass = "Fighter",
                ArmorRating = 280,
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker",
                    AreaId = 37
                },
                Guard = true,

                PathList = new List<string>()
                {
                    "e",
                    "e",
                    "e",
                    "e",
                    "e",
                    "e",
                    "e",
                    //reverse
                    "w",
                    "w",
                    "w",
                    "w",
                    "w",
                    "w",
                    "w"

                }


            };

            var sword = LongSwordBasic.LongIronSword();
            sword.stats = new Stats()
            {
                damMin = 1,
                damMax = 20
            };
            Guard.Equipment.Wielded = sword.name;
            sword.location = Item.Item.ItemLocation.Wield;
            Guard.Inventory.Add(sword);


            return Guard;
        }

        public static PlayerSetup.Player AnkerGuardNpc2()
        {


            var Guard = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Tarek, the Anker guard",
                NPCLongName = "Tarek the guard stands here keeping watch",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "Dark chain mail armour covers the guard head to toe. A long sword is sheathed at his waist",
                Strength = 17,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 12,
                Wisdom = 16,
                Charisma = 16,
                MaxHitPoints = 522,
                HitPoints = 522,
                Level = 18,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>()
                {
                    Dodge.DodgeAb(),
                    ShieldBlock.ShieldBlockAb(),
                    Parry.ParryAb(),
                    SecondAttack.SecondAttackAb(),
                    LongBlades.LongBladesAb(),
                },
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
                AreaId = 19,
                Guard = true,
                SelectedClass = "Fighter",
                ArmorRating = 280,
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker",
                    AreaId = 19
                },
                PathList = new List<string>()
                {
                    "n",
                    "n",
                    "n",
                    "w",
                    "w",
                    "s",
                    "s",
                    "e",
                    "e",
                    "s",
                    "n",
                    "n",
                    "w",
                    "n",
                    "e",
                    "s",
                    "w",
                    "s",
                    "e",
                    "s",
                    //reverse
                    "n",
                    "w",
                    "w",
                    "n",
                    "e",
                    "n",
                    "w",
                    "e",
                    "s",
                    "s",
                    "n",
                    "w",
                    "s",
                    "e",
                    "e",
                    "s",

                }


            };

            var sword = LongSwordBasic.LongIronSword();
            sword.stats = new Stats()
            {
                damMin = 1,
                damMax = 20
            };
            Guard.Equipment.Wielded = sword.name;
            sword.location = Item.Item.ItemLocation.Wield;
            Guard.Inventory.Add(sword);
 
            return Guard;
        }
    }
}