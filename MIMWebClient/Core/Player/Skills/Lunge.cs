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

    public class Lunge: Skill
    {

        public static Skill LungeSkill { get; set; }
        public static Skill LungeAb()
        {
                  
            if (LungeSkill != null)
            {
               return LungeSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Lunge",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Lunge <target>",
                    HelpText = new Help()
                    {
                        HelpText = "lunge help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LungeSkill = skill;
            }

            return LungeSkill;
            
        }

    }
}
