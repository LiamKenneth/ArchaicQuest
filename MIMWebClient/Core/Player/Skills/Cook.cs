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

    public class Cook: Skill
    {

        public static Skill CookSkill { get; set; }
        public static Skill CookAb()
        {
                  
            if (CookSkill != null)
            {
               return CookSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Cook",
                    Alias = CraftType.Cook.ToString(),
                    CoolDown = 0,
                    Delay = 0,
                    Points = 0,
                    SkillType = Type.Crafting,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Cook <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to cook",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                CookSkill = skill;
            }

            return CookSkill;
            
        }

    }
}
