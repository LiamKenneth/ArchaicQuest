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

    public class Parry: Skill
    {

        public static Skill ParrySkill { get; set; }
        public static Skill ParryAb()
        {
                  
            if (ParrySkill != null)
            {
               return ParrySkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Parry",
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
                        HelpText = "Parry help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ParrySkill = skill;
            }

            return ParrySkill;
            
        }

    }
}
