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

    public class Mount: Skill
    {

        public static Skill MountSkill { get; set; }
        public static Skill MountAb()
        {
                  
            if (MountSkill != null)
            {
               return MountSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Mount",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Mount <Mob>",
                    HelpText = new Help()
                    {
                        HelpText = "Ability to mount a mob",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                MountSkill = skill;
            }

            return MountSkill;
            
        }

    }
}
