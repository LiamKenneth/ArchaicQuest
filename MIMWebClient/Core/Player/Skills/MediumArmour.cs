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

    public class MediumArmour: Skill
    {

        public static Skill MediumArmourSkill { get; set; }
        public static Skill MediumArmourAb()
        {
                  
            if (MediumArmourSkill != null)
            {
               return MediumArmourSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Medium Armour",
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
                        HelpText = "Medium Armour help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                MediumArmourSkill = skill;
            }

            return MediumArmourSkill;
            
        }

    }
}
