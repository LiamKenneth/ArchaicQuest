using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Classes.Reclasses;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player.Skills;

    public class Mage : PlayerClass
    {
        public static PlayerClass MageClass()
        {


            var mage = new PlayerClass
            {
                Name = "Mage",
                IsBaseClass = true,
                ExperienceModifier = 3000,
                HelpText = new Help(),
                Skills = new List<Skill>(),
                ReclassOptions = new List<PlayerClass>(),
                MaxHpGain = 8,
                MinHpGain = 3,
                MaxManaGain =15,
                MinManaGain = 10,
                MaxEnduranceGain = 15,
                MinEnduranceGain = 11,
                StatBonusInt = 2,
                StatBonusWis = 1

            };

            #region  Give fighter punch skill

            var punch = Punch.PunchAb();
            punch.LevelObtained = 2;
            punch.Proficiency = 1;
            punch.MaxProficiency = 95;
            mage.Skills.Add(punch);

            #endregion

            #region  Give mage magic missile skill

            var magicMissile = MagicMissile.MagicMissileAb();

            magicMissile.LevelObtained = 1;
            magicMissile.Proficiency = 50;
            magicMissile.MaxProficiency = 95;
            mage.Skills.Add(magicMissile);

            #endregion


            mage.ReclassOptions.Add(Ranger.RangerClass());

            return mage;


        }
    }
}