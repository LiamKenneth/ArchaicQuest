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

    public class Swim: Skill
    {

        public static Skill SwimSkill { get; set; }
        public static Skill SwimAb()
        {
                  
            if (SwimSkill != null)
            {
               return SwimSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Swim",
                    CoolDown = 0,
                    Delay = 0,
                    SkillType = Type.Skill,
                    Points = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Knit <item>",
                    HelpText = new Help()
                    {
                        HelpText = " swim help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                SwimSkill = skill;
            }

            return SwimSkill;
            
        }

        public static void SwimSuccess(Player player, int swimSkill)
        {

            var chanceOfSuccess = Helpers.Rand(1, 100);

            if (swimSkill >= chanceOfSuccess)
            {
                Player.UpdateMoves(player, 12, false);
            }
            else
            {
                Player.UpdateHitPoints(player, 10, false);
                HubContext.Instance.SendToClient("You struggle to swim and take a gulp of water.",  player.HubGuid);
                Player.UpdateMoves(player, 24, false);

                HubContext.Instance.SendToClient("<p class='SwimSuccess'>You learn from your mistakes and gain 100 experience points.</p>",
                    player.HubGuid);

                player.Skills.FirstOrDefault(x => x.Name.Equals("Swim")).Proficiency += Helpers.Rand(1, 5);

                Score.ReturnScoreUI(player);
            }

        }




    }
}
