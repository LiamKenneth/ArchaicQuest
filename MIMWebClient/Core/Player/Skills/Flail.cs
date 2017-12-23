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

    public class Flail: Skill
    {

        public static Skill FlailSkill { get; set; }
        public static Skill FlailAb()
        {
                  
            if (FlailSkill != null)
            {
               return FlailSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Flail",
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
                        HelpText = "Flail help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                FlailSkill = skill;
            }

            return FlailSkill;
            
        }

    }
}
