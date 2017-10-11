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

    public class DirtKick: Skill
    {

        public static Skill DirtKickSkill { get; set; }
        public static Skill DirtKickAb()
        {
                  
            if (DirtKickSkill != null)
            {
               return DirtKickSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Dirt Kick",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Dirt kick help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                DirtKickSkill = skill;
            }

            return DirtKickSkill;
            
        }

    }
}
