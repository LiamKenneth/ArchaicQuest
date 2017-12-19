using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Classes.Reclasses;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player.Skills;

    public class Cleric : PlayerClass
    {
        public static PlayerClass ClericClass()
        {


            var cleric = new PlayerClass
            {
                Name = "Cleric",
                IsBaseClass = true,
                ExperienceModifier = 2000,
                HelpText = new Help(),
                Skills = new List<Skill>(),
                ReclassOptions = new List<PlayerClass>(),
                MaxHpGain = 15,
                MinHpGain = 10,
                MaxManaGain = 15,
                MinManaGain = 10,
                MaxEnduranceGain = 15,
                MinEnduranceGain = 11,
                StatBonusStr = 1,
                StatBonusCon = 1

            };

            #region  Lvl 1 skills

            var blunt = BluntWeapons.BluntWeaponsAb();
            blunt.Learned = true;
            blunt.Proficiency = 25;
            cleric.Skills.Add(blunt);



            var causeLight = CauseLight.causeLightAb();
            causeLight.Learned = true;
            causeLight.Proficiency = 50;
            cleric.Skills.Add(causeLight);


            #endregion


            cleric.ReclassOptions.Add(Ranger.RangerClass());

            return cleric;


        }
    }
}