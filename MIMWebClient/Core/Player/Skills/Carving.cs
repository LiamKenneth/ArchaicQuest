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

    public class Carving: Skill
    {

        public static Skill CarvingSkill { get; set; }
        public static Skill CarvingAb()
        {
                  
            if (CarvingSkill != null)
            {
               return CarvingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Carpentry",
                    CoolDown = 0,
                    Delay = 0,
                    Points = 0,
                    SkillType = Type.Crafting,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Carve <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to carve",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                CarvingSkill = skill;
            }

            return CarvingSkill;
            
        }

    }
}
