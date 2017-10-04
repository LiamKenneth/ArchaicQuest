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

    public class ShieldBlock: Skill
    {

        public static Skill ShieldBlockSkill { get; set; }
        public static Skill ShieldBlockAb()
        {
                  
            if (ShieldBlockSkill != null)
            {
               return ShieldBlockSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Shield Block",
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
                        HelpText = "Shield Block help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ShieldBlockSkill = skill;
            }

            return ShieldBlockSkill;
            
        }

    }
}
