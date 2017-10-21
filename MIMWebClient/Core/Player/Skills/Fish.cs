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

    public class Fish: Skill
    {

        public static Skill FishingSkill { get; set; }
        public static Skill FishingAb()
        {
                  
            if (FishingSkill != null)
            {
               return FishingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Fishing",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Fishing help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                FishingSkill = skill;
            }

            return FishingSkill;
            
        }

    }
}
