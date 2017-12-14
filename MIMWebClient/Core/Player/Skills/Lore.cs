using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player.Skills
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Lore: Skill
    {

        public static Skill LoreSkill { get; set; }
        public static Skill LoreAb()
        {
                  
            if (LoreSkill != null)
            {
               return LoreSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Lore",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Lore",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "Lore help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LoreSkill = skill;
            }

            return LoreSkill;
            
        }

        public static void DoLore(IHubContext context, PlayerSetup.Player player, string item)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, LoreAb().Name);

            if (hasSkill == false)
            {
                context.SendToClient("You don't know that skill.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            if (string.IsNullOrEmpty(item))
            {
                context.SendToClient("You need to specify an item.", player.HubGuid);
                return;
            }

            var hasItem = player.Inventory.FirstOrDefault(x => x.name.Contains(item));

            if (hasItem == null)
            {
                context.SendToClient("You don't have such an item.", player.HubGuid);
                return;
            }


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Lore"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {

                //Itme stats
                HubContext.Instance.SendToClient(" ", player.HubGuid);
              
                Score.UpdateUiAffects(player);
                Score.ReturnScoreUI(player);
            }
            else
            {
                //something random
                HubContext.Instance.SendToClient("  .", player.HubGuid);
    
                Score.ReturnScoreUI(player);
            }
        }

    }
}
