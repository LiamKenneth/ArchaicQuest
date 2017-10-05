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

    public class Exotic: Skill
    {

        public static Skill ExoticSkill { get; set; }
        public static Skill ExoticAb()
        {
                  
            if (ExoticSkill != null)
            {
               return ExoticSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Exotic",
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
                        HelpText = "Exotic help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ExoticSkill = skill;
            }

            return ExoticSkill;
            
        }

    }
}
