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

    public class Dodge: Skill
    {

        public static Skill DodgeSkill { get; set; }
        public static Skill DodgeAb()
        {
                  
            if (DodgeSkill != null)
            {
               return DodgeSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Dodge",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 5,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Dodge help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                DodgeSkill = skill;
            }

            return DodgeSkill;
            
        }

    }
}
