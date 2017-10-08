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

    public class Toughness: Skill
    {

        public static Skill ToughnessSkill { get; set; }
        public static Skill ToughnessAb()
        {
                  
            if (ToughnessSkill != null)
            {
               return ToughnessSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Toughness",
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
                        HelpText = "Toughness help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ToughnessSkill = skill;
            }

            return ToughnessSkill;
            
        }

    }
}
