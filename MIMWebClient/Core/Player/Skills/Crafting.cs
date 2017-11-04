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

    public class Crafting: Skill
    {

        public static Skill CraftingSkill { get; set; }
        public static Skill CraftingAb()
        {
                  
            if (CraftingSkill != null)
            {
               return CraftingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Crafting",
                    Alias = CraftType.Craft.ToString(),
                    Points = 0,
                    CoolDown = 0,
                    Delay = 0,
                    SkillType = Type.Crafting,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Craft <item>",
                    HelpText = new Help()
                    {
                        HelpText = " use to general crafting",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                CraftingSkill = skill;
            }

            return CraftingSkill;
            
        }

    }
}
