using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Body;
using MIMWebClient.Core.World.Items.Weapons.Sword.Short;


namespace MIMWebClient.Core.World.Anker.Mobs.Easy
{
    public class Goblin
    {

        public static PlayerSetup.Player WeakGoblin()
        {

            var leatherVest = LeatherBody.LeatherVest();
            leatherVest.location = Item.Item.ItemLocation.Worn;

            var rustedSword = ShortSwordBasic.RustedShortSword();
            rustedSword.location = Item.Item.ItemLocation.Wield;
 
            var WeakGoblin = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "A skinny hunched Goblin",
                NPCLongName = "A skinny hunched goblin snarls at you",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "A weak goblin",
                Strength = 50,
                Dexterity = 30,
                Constitution = 30,
                Intelligence = 30,
                Wisdom = 30,
                Charisma = 30,
                MaxHitPoints = 10,
                HitPoints = 10,
                Level = 1,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>()
                {
                    ShortBlades.ShortBladesAb()
                },
                Inventory = new ItemContainer()
                {
                    leatherVest,
                    rustedSword
                },
                Equipment = new Equipment()
                {
                    Body = leatherVest.name,
                    Wielded = rustedSword.name
                },
                Trainer = false,
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


            };

 

         

            return WeakGoblin;
        }

        public static PlayerSetup.Player GoblinWarrior()
        {

            var leatherVest = LeatherBody.LeatherVest();
            leatherVest.location = Item.Item.ItemLocation.Worn;

            var rustedSword = ShortSwordBasic.RustedShortSword();
            rustedSword.location = Item.Item.ItemLocation.Wield;
            rustedSword.stats = new Stats()
            {
                damMax = 12,
                damMin = 2,
                damRoll = 1,
                minUsageLevel = 1,
                worth = 1
            };

            var WeakGoblin = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "A goblin warrior",
                NPCLongName = "A strong goblin snarls at you",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "A strong goblin",
                Strength = 20,
                Dexterity = 13,
                Constitution = 12,
                Intelligence = 4,
                Wisdom = 5,
                Charisma = 2,
                MaxHitPoints = 100,
                HitPoints = 100,
                Level = 5,
                Aggro = true,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer()
                {
                    leatherVest,
                    rustedSword
                },
                Equipment = new Equipment()
                {
                    Body = leatherVest.name,
                    Wielded = rustedSword.name
                },
                Trainer = false,
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


            };





            return WeakGoblin;
        }
    }
}