using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player.Skills;

    public class Ranger : PlayerClass
    {
        public static PlayerClass RangerClass()
        {
            var ranger = new PlayerClass
            {
                Name = "Ranger",
                IsBaseClass = false,
                ExperienceModifier = 2000,
                HelpText = new Help(),
                Skills = new List<Skill>(),
                ReclassOptions = new List<PlayerClass>()
            };

            #region  Give fighter punch skill

            var punch = Punch.PunchAb();
            punch.LevelObtained = 2;
            punch.Proficiency = 0.1;
            punch.MaxProficiency = 0.95;
            ranger.Skills.Add(punch);

            #endregion

            return ranger;
        }
    }
}