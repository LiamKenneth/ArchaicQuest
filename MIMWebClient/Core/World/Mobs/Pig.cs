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


namespace MIMWebClient.Core.World.Anker.Mobs
{
    public class Pig
    {

        public static PlayerSetup.Player SmallPig()
        {

           
 
            var SmallPig = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "pink pig",
                NPCLongName = "large pig is laying in the mud",
                KnownByName = false,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "A large pig is laying in the mud",
                Strength = 45,
                Dexterity = 30,
                Constitution = 30,
                Intelligence = 30,
                Wisdom = 30,
                Charisma = 30,
                MaxHitPoints = 75,
                HitPoints = 75,
                Level = 5,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Trainer = false,
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>(),
                MobAttackType = PlayerSetup.Player.MobAttackTypes.Charge,
                MobAttackStats = new Stats
                {
                    damMin = 3,
                    damMax = 6
                }
 
            };


            return SmallPig;
        }

 
    }
}