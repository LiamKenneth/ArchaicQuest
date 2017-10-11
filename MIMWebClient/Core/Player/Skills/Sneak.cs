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

    public class Sneak: Skill
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
                    LevelObtained = 1,
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

    }
}
