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

    public class EnhancedDamage: Skill
    {

        public static Skill EnhancedDamageSkill { get; set; }
        public static Skill EnhancedDamageAb()
        {
                  
            if (EnhancedDamageSkill != null)
            {
               return EnhancedDamageSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Enhanced Damage",
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
                        HelpText = "Enhanced Damage help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                EnhancedDamageSkill = skill;
            }

            return EnhancedDamageSkill;
            
        }

    }
}
