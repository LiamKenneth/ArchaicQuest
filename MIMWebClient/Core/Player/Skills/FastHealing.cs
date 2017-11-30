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

    public class FastHealing: Skill
    {

        public static Skill FastHealingSkill { get; set; }
        public static Skill FastHealingAb()
        {
                  
            if (FastHealingSkill != null)
            {
               return FastHealingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Fast Healing",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 4,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "All",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Fast Healing help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                FastHealingSkill = skill;
            }

            return FastHealingSkill;
            
        }

    }
}
