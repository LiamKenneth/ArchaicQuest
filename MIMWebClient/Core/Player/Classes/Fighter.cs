using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Classes.Reclasses;
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;

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
                ExperienceModifier = 500,
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
                StatBonusCon = 1,
               

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
            bash.Proficiency = 1;
            bash.MaxProficiency = 95;
            fighter.Skills.Add(bash);

            var punch = Punch.PunchAb();
            punch.LevelObtained = 1;
            punch.Proficiency = 1;
            punch.MaxProficiency = 95;
            fighter.Skills.Add(punch);

            #endregion

            #region  Give fighter kick skill

            var kick = Kick.KickAb();

            kick.LevelObtained = 1;
            kick.Proficiency = 50;
            kick.MaxProficiency = 95;
            fighter.Skills.Add(kick);

            #endregion

            #region  Give fighter longblade skill

            var longblade = LongBlades.LongBladesAb();
          
            fighter.Skills.Add(longblade);

            #endregion


            //#region  Give fighter secondAttack skill

            var shortBlades = ShortBlades.ShortBladesAb();
            shortBlades.Proficiency = 50;
            fighter.Skills.Add(shortBlades);

            //#endregion


            #region  Give fighter Mount skill

            var mount = Mount.MountAb();
            mount.Proficiency = 95;
            fighter.Skills.Add(mount);

            #endregion

            fighter.ReclassOptions.Add(Ranger.RangerClass());

            return fighter;


        }
    }
}