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

    public class Hide: Skill
    {

        public static Skill HideSkill { get; set; }
        public static Skill HideAb()
        {
                  
            if (HideSkill != null)
            {
               return HideSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Hide",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "hide",
                    HelpText = new Help()
                    {
                        HelpText = "hide help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                HideSkill = skill;
            }

            return HideSkill;
            
        }

    }
}
