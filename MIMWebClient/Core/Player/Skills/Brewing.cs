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

    public class Brewing: Skill
    {

        public static Skill BrewingSkill { get; set; }
        public static Skill BrewingAb()
        {
                  
            if (BrewingSkill != null)
            {
               return BrewingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Brewing",
                    CoolDown = 0,
                    Delay = 0,
                    Points = 0,
                    SkillType = Type.Crafting,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Brew <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to brew",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                BrewingSkill = skill;
            }

            return BrewingSkill;
            
        }

    }
}
