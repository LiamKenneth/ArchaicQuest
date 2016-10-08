using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Player.Skills;

    public class Fighter : PlayerClass
    {
        public static PlayerClass FighterClass()
        {


            var fighter = new PlayerClass
            {
                Name = "Fighter",
                ExperienceModifier = 0,
                HelpText = "Help text about fighter class",
                Skills = new List<Skill>(),
                ReclassOptions = new List<PlayerClass>()
            
            };

            var punch = Punch.PunchAb();

            fighter.Skills.Add(punch);

            return fighter;


        }
    }
}