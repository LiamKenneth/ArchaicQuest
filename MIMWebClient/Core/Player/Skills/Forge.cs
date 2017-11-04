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

    public class Forge: Skill
    {

        public static Skill ForgeSkill { get; set; }
        public static Skill ForgeAb()
        {
                  
            if (ForgeSkill != null)
            {
               return ForgeSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Forging",
                    CoolDown = 0,
                    Delay = 0,
                    SkillType = Type.Crafting,
                    Points = 200,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Forge <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to forge",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ForgeSkill = skill;
            }

            return ForgeSkill;
            
        }

    }
}
