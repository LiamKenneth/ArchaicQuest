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

    public class Harvest: Skill
    {

        public static Skill HarvestSkill { get; set; }
        public static Skill HarvestAb()
        {
                  
            if (HarvestSkill != null)
            {
               return HarvestSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Foraging",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "forage.",
                    HelpText = new Help()
                    {
                        HelpText = " ",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                HarvestSkill = skill;
            }

            return HarvestSkill;
            
        }

    }
}
