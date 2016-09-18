using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player.Skills
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class ShortBlades: Skill
    {

        public static Skill ShortBladesSkill { get; set; }
        public static Skill ShortBladesAb()
        {
           
           
            if (ShortBladesSkill != null)
            {
               return ShortBladesSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Short Blades",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command"
                };


                skill.HelpText = skill.Syntax + " Some help text about Short Bladess";

                ShortBladesSkill = skill;
            }

            return ShortBladesSkill;
            
        }

    }
}
