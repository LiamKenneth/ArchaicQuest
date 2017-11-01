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

    public class Knitting: Skill
    {

        public static Skill KnittingSkill { get; set; }
        public static Skill KnittingAb()
        {
                  
            if (KnittingSkill != null)
            {
               return KnittingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Knitting",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Knit <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to knit",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                KnittingSkill = skill;
            }

            return KnittingSkill;
            
        }

    }
}
