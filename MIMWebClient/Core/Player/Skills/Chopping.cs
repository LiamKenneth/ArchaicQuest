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

    public class Chopping: Skill
    {

        public static Skill ChoppingSkill { get; set; }
        public static Skill ChoppingAb()
        {
                  
            if (ChoppingSkill != null)
            {
               return ChoppingSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Chopping",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Standing",
                    Syntax = "Chop <material>",
                    HelpText = new Help()
                    {
                        HelpText = " use to chop wood",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ChoppingSkill = skill;
            }

            return ChoppingSkill;
            
        }

    }
}
