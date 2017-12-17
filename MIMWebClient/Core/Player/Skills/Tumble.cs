using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Core.Player.Skills
{

    public class Tumble : Skill
    {

        public static Skill TumbleSkill { get; set; }
        public static Skill TumbleAb()
        {

            if (TumbleSkill != null)
            {
                return TumbleSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Tumble",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 12,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Tumble",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "tumble help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                TumbleSkill = skill;
            }

            return TumbleSkill;

        }

    }
}
