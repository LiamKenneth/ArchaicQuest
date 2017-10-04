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

    public class BluntWeapons: Skill
    {

        public static Skill BluntWeaponsSkill { get; set; }
        public static Skill BluntWeaponsAb()
        {
                  
            if (BluntWeaponsSkill != null)
            {
               return BluntWeaponsSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Blunt Weapons",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Blunt Weapons help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                BluntWeaponsSkill = skill;
            }

            return BluntWeaponsSkill;
            
        }

    }
}
