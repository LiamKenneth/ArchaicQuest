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

    public class ThirdAttack : Skill
    {

        public static Skill ThirdAttackSkill { get; set; }
        public static Skill ThirdAttackAb()
        {

            if (ThirdAttackSkill != null)
            {
                return ThirdAttackSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Third Attack",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 20,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive",
                    HelpText = new Help()
                    {
                        HelpText = "Ability to attack three times in combat",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ThirdAttackSkill = skill;
            }

            return ThirdAttackSkill;

        }

    }
}
