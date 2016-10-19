using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Classes.Reclasses;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player.Skills;

    public class Fighter : PlayerClass
    {
        public static PlayerClass FighterClass()
        {


            var fighter = new PlayerClass
            {
                Name = "Fighter",
                IsBaseClass = true,
                ExperienceModifier = 1500,
                HelpText = new Help(),
                Skills = new List<Skill>(),
                ReclassOptions = new List<PlayerClass>(),
                MaxHpGain = 15,
                MinHpGain = 10,
                MaxManaGain = 8,
                MinManaGain = 4,
                MaxEnduranceGain = 15,
                MinEnduranceGain = 11,
                StatBonusStr = 1,
                StatBonusCon = 1

            };

            #region  Give fighter punch skill

            var punch = Punch.PunchAb();
            punch.LevelObtained = 2;
            punch.Proficiency = 0.1;
            punch.MaxProficiency = 0.95;
            fighter.Skills.Add(punch);

            #endregion

            fighter.ReclassOptions.Add(Ranger.RangerClass());

            return fighter;


        }
    }
}