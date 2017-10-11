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

    public class Rescue: Skill
    {

        public static Skill RescueSkill { get; set; }
        public static Skill RescueAb()
        {
                  
            if (RescueSkill != null)
            {
               return RescueSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Rescue",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Save someone from being the target of an attack and become the new target",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                RescueSkill = skill;
            }

            return RescueSkill;
            
        }

    }
}
