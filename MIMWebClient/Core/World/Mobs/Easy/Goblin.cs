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
                Strength = 9,
                Dexterity = 8,
                Constitution = 12,
                Intelligence = 4,
                Wisdom = 5,
                Charisma = 2,
                MaxHitPoints = 3,
                HitPoints = 3,
                Level = 1,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>()
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