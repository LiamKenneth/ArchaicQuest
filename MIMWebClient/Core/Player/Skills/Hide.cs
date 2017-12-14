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

    public class Hide: Skill
    {

        public static Skill HideSkill { get; set; }
        public static Skill HideAb()
        {
                  
            if (HideSkill != null)
            {
               return HideSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Hide",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "hide",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "hide help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                HideSkill = skill;
            }

            return HideSkill;
            
        }

        public static void DoHide(IHubContext context, PlayerSetup.Player player)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, HideAb().Name);

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


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Hide"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                HubContext.Instance.SendToClient("You step into the shadows.", player.HubGuid);
                player.Effects.Add(Effect.Hide(player));
                Score.UpdateUiAffects(player);
                Score.ReturnScoreUI(player);
            }
            else
            {

                HubContext.Instance.SendToClient("You step into the shadows.", player.HubGuid);
    
                Score.ReturnScoreUI(player);
            }
        }

    }
}
