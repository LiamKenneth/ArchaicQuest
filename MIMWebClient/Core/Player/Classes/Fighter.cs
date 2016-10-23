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

            /* TODO: some skills to add
             * Axe Dagger Polearm  Mace
             * Spear Shield Block
             * staff  sword
             *  bash Whip Enhanced damage
             *  parry rescue swim  scrolls
             *  staves  wands  recall
             *  age  dig
             *  dirt kicking
             *  second atttack
             *  third attack
             *  fouth attack
             *  fast healing
             *  kick
             *  disarm
             *  blind fighting
             *  trip
             *  berserk
             *  dual wield (eek)
             * */

            #region  Give fighter punch skill

            var bash = Punch.PunchAb();
            bash.Name = "Bash";
            bash.LevelObtained = 10;
            bash.Proficiency = 0.1;
            bash.MaxProficiency = 0.95;
            fighter.Skills.Add(bash);

            var punch = Punch.PunchAb();
            punch.LevelObtained = 1;
            punch.Proficiency = 0.1;
            punch.MaxProficiency = 0.95;
            fighter.Skills.Add(punch);

            #endregion

            #region  Give fighter kick skill

            var kick = Punch.PunchAb();
            kick.Name = "Kick";
            kick.LevelObtained = 3;
            kick.Proficiency = 0.1;
            kick.MaxProficiency = 0.95;
            fighter.Skills.Add(kick);

            #endregion

            #region  Give fighter longblade skill

            var longblade = Punch.PunchAb();
            longblade.Name = "Long Blade";
            longblade.LevelObtained = 3;
            longblade.Proficiency = 0.1;
            longblade.MaxProficiency = 0.95;
            fighter.Skills.Add(longblade);

            #endregion

            fighter.ReclassOptions.Add(Ranger.RangerClass());

            return fighter;


        }
    }
}