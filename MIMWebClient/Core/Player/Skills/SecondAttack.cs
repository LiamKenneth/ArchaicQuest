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

    public class SecondAttack: Skill
    {

        public static Skill SecondAttackSkill { get; set; }
        public static Skill SecondAttackAb()
        {
                  
            if (SecondAttackSkill != null)
            {
               return SecondAttackSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Second Attack",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 12,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive",
                    HelpText = new Help()
                    {
                        HelpText = "Ability to attack twice in combat",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                SecondAttackSkill = skill;
            }

            return SecondAttackSkill;
            
        }

    }
}
