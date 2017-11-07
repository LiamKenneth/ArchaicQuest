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

    public class Repair: Skill
    {

        public static Skill RepairSkill { get; set; }
        public static Skill RepairAb()
        {
                  
            if (RepairSkill != null)
            {
               return RepairSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Repair",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Passive",
                    HelpText = new Help()
                    {
                        HelpText = "Repair items and armour",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                RepairSkill = skill;
            }

            return RepairSkill;
            
        }

    }
}
