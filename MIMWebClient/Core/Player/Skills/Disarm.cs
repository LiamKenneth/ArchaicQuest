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

    public class Disarm: Skill
    {

        public static Skill DisarmSkill { get; set; }
        public static Skill DisarmAb()
        {
                  
            if (DisarmSkill != null)
            {
               return DisarmSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Disarm",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Disarm <target>",
                    HelpText = new Help()
                    {
                        HelpText = "disarm help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                DisarmSkill = skill;
            }

            return DisarmSkill;
            
        }

    }
}
