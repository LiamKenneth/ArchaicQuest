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

    public class LongBlades: Skill
    {

        public static Skill LongBladesSkill { get; set; }
        public static Skill LongBladesAb()
        {
                  
            if (LongBladesSkill != null)
            {
               return LongBladesSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Long Blades",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "long blade help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LongBladesSkill = skill;
            }

            return LongBladesSkill;
            
        }

    }
}
