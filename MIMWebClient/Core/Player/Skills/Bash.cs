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

    public class Bash: Skill
    {

        public static Skill BashSkill { get; set; }
        public static Skill BashAb()
        {
                  
            if (BashSkill != null)
            {
               return BashSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Bash",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Bash <target>",
                    HelpText = new Help()
                    {
                        HelpText = "bash help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                BashSkill = skill;
            }

            return BashSkill;
            
        }

    }
}
