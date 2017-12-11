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

    public class Sneak : Skill
    {

        public static Skill SneakSkill { get; set; }
        public static Skill SneakAb()
        {

            if (SneakSkill != null)
            {
                return SneakSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Sneak",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 12,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Sneak",
                    HelpText = new Help()
                    {
                        HelpText = "sneak help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                SneakSkill = skill;
            }

            return SneakSkill;

        }

        public static void DoSneak(IHubContext context, PlayerSetup.Player player)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, SneakAb().Name);

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
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Sneak"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                HubContext.Instance.SendToClient("You become more stealthy.", player.HubGuid);
                player.Effects.Add(Effect.Sneak(player));
                Score.UpdateUiAffects(player);
            }
            else
            {
              
                HubContext.Instance.SendToClient("You fail to sneak.", player.HubGuid);
                PlayerSetup.Player.LearnFromMistake(player, SneakAb(), 250);


                Score.ReturnScoreUI(player);
            }
        }



    }

}
