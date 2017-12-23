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

            #region  Lvl 2 skills

           
            var cureLight = CureLight.CureLightAb();
            cureLight.Learned = true;
            cureLight.Proficiency = 50;
            cleric.Skills.Add(cureLight);


            #endregion

            #region  Lvl 3 skills


            var detectInvis = DetectInvis.DetectInvisAb();
            detectInvis.Learned = true;
            detectInvis.Proficiency = 50;
            cleric.Skills.Add(detectInvis);


            #endregion


            #region  Lvl 5 skills


            var armour = Armour.ArmourAb();
            armour.Learned = true;
            armour.LevelObtained = 1;
            armour.Proficiency = 50;
            cleric.Skills.Add(armour);

            var flail = Flail.FlailAb();
            flail.Learned = true;
            flail.LevelObtained = 1;
            flail.Proficiency = 50;
            cleric.Skills.Add(flail);

            var mount = Mount.MountAb();
            mount.Learned = true;
            mount.LevelObtained = 1;
            mount.Proficiency = 50;
            cleric.Skills.Add(mount);


            #endregion

            cleric.ReclassOptions.Add(Ranger.RangerClass());

            return cleric;


        }
    }
}