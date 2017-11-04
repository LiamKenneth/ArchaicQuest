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
                    Points = 0,
                    SkillType = Type.Crafting,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Fish to cast the line, reel to catch the fish.",
                    HelpText = new Help()
                    {
                        HelpText = "ish to cast the line, reel to catch the fish.",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                FishingSkill = skill;
            }

            return FishingSkill;
            
        }

    }
}
