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

    public class Axe: Skill
    {

        public static Skill AxeSkill { get; set; }
        public static Skill AxeAb()
        {
                  
            if (AxeSkill != null)
            {
               return AxeSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Axe",
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
                        HelpText = "Axe help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                AxeSkill = skill;
            }

            return AxeSkill;
            
        }

    }
}
