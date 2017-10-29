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
    public class rabbit
    {

        public static PlayerSetup.Player SmallRabbit()
        {

           
 
            var smallrabbit = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "White rabbit",
                NPCLongName = "small rabbit is nibbling the grass here.",
                KnownByName = false,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The calico rabbit occasionally stops hopping to look up and nervously scans the area. Nose and ears twitching as it does so before resuming it's hoping.",
                Strength = 30,
                Dexterity = 30,
                Constitution = 30,
                Intelligence = 30,
                Wisdom = 30,
                Charisma = 30,
                MaxHitPoints = 15,
                HitPoints = 15,
                Level = 1,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>()
                {
                    ShortBlades.ShortBladesAb()
                },
                Inventory = new ItemContainer()
                {
                  new Item.Item()
                  {
                      name = "rabbit fur",
                      Gold = 2,
                      Weight = 0.1,
                      description = new Description()
                      {
                          look = "Soft white fur."
                      },
                      type = Item.Item.ItemType.Misc,
                      location = Item.Item.ItemLocation.Inventory
                      
                  }
                },
                Equipment = new Equipment()
                {
      
                },
                Trainer = false,
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>(),
                MobAttackType = PlayerSetup.Player.MobAttackTypes.Bite,
                MobAttackStats = new Stats()
                {
                    damMin = 2,
                    damMax = 6
                }


            };

 

         

            return smallrabbit;
        }

 
    }
}