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

    public class Staff: Skill
    {

        public static Skill StaffSkill { get; set; }
        public static Skill StaffAb()
        {
                  
            if (StaffSkill != null)
            {
               return StaffSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Staff",
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
                        HelpText = "Staff help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                StaffSkill = skill;
            }

            return StaffSkill;
            
        }

    }
}
