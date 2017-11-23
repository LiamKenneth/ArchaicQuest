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

    public class HeavyArmour: Skill
    {

        public static Skill HeavyArmourSkill { get; set; }
        public static Skill HeavyArmourAb()
        {
                  
            if (HeavyArmourSkill != null)
            {
               return HeavyArmourSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Heavy Armour",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 2,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",                  
                    HelpText = new Help()
                    {
                        HelpText = "Heavy Armour help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                HeavyArmourSkill = skill;
            }

            return HeavyArmourSkill;
            
        }

    }
}
