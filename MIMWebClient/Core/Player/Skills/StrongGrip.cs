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

    public class StrongGrip: Skill
    {

        public static Skill StrongGripSkill { get; set; }
        public static Skill StrongGripAb()
        {
                  
            if (StrongGripSkill != null)
            {
               return StrongGripSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Strong grip",
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
                        HelpText = "strong grip help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                StrongGripSkill = skill;
            }

            return StrongGripSkill;
            
        }

    }
}
