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

    public class HandToHand: Skill
    {

        public static Skill HandToHandSkill { get; set; }
        public static Skill HandToHandAb()
        {
           
            Skill HandToHand = null;

            if (HandToHandSkill != null)
            {
                HandToHand = HandToHandSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Hand To Hand",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Passive command"
                };


                skill.HelpText = skill.Syntax + " Some help text about Hand to Hand";

                HandToHandSkill = skill;
            }

            return HandToHandSkill;
            
        }

    }
}
