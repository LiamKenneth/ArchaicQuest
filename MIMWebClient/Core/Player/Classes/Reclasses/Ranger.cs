using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Classes.Reclasses
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
                ReclassOptions = new List<PlayerClass>(),
                MaxHpGain = 20,
                MinHpGain = 13,
                MaxManaGain = 15,
                MinManaGain = 5,
                MaxEnduranceGain = 40,
                MinEnduranceGain = 15,
                StatBonusCon = 1,
                StatBonusDex = 2
            };

            #region  Give fighter punch skill

            var punch = Punch.PunchAb();
            punch.LevelObtained = 2;
            punch.Proficiency = 1;
            punch.MaxProficiency = 95;
            ranger.Skills.Add(punch);

            #endregion

            return ranger;
        }
    }
}