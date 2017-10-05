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

    public class Polearms: Skill
    {

        public static Skill PolearmsSkill { get; set; }
        public static Skill PolearmsAb()
        {
                  
            if (PolearmsSkill != null)
            {
               return PolearmsSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Polearms",
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
                        HelpText = "Polearms help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                PolearmsSkill = skill;
            }

            return PolearmsSkill;
            
        }

    }
}
