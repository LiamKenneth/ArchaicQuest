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
 

            #region  Lvl 1 skills

            var longBlades = LongBlades.LongBladesAb();
            longBlades.Learned = true;
            fighter.Skills.Add(longBlades);

            var shortBlades = ShortBlades.ShortBladesAb();
            shortBlades.Learned = true;
            fighter.Skills.Add(shortBlades);

            var axe = Axe.AxeAb();
            axe.Learned = true;
            fighter.Skills.Add(axe);

            var blunt = BluntWeapons.BluntWeaponsAb();
            blunt.Learned = true;
            fighter.Skills.Add(blunt);

            var polearm = Polearms.PolearmsAb();
            polearm.Learned = true;
            fighter.Skills.Add(polearm);

            var exotic = Exotic.ExoticAb();
            exotic.Learned = true;
            fighter.Skills.Add(exotic);

            var staff = Staff.StaffAb();
            staff.Learned = true;
            fighter.Skills.Add(staff);

            var handToHand = HandToHand.HandToHandAb();
            handToHand.Learned = true;
            fighter.Skills.Add(handToHand);

            var lightArmour = LightArmour.LightArmourAb();
            lightArmour.Learned = true;
            fighter.Skills.Add(lightArmour);



            #endregion

            #region  Lvl 2 skills
            fighter.Skills.Add(HeavyArmour.HeavyArmourAb());
            fighter.Skills.Add(MediumArmour.MediumArmourAb());


            #endregion

            #region Lvl 3 skills
            fighter.Skills.Add(Trip.TripAb());
            #endregion

            #region Lvl 4
            fighter.Skills.Add(FastHealing.FastHealingAb());
            fighter.Skills.Add(Toughness.ToughnessAb());
            #endregion

            #region Lvl 5

            var parry = Parry.ParryAb();
            fighter.Skills.Add(parry);


            #endregion


            #region Lvl 6

            var shieldBlock = ShieldBlock.ShieldBlockAb();
            fighter.Skills.Add(shieldBlock);

            var dodge = Dodge.DodgeAb();
            fighter.Skills.Add(dodge);

            #endregion


            #region Lvl 7

            var dirtKick = DirtKick.DirtKickAb();
            fighter.Skills.Add(dirtKick);

            var kick = Kick.KickAb();
            fighter.Skills.Add(kick);

            #endregion

            #region Lvl 9

            var bash = Bash.BashAb();
            fighter.Skills.Add(bash);



            #endregion


            #region Lvl 10

            var rescue = Rescue.RescueAb();
            rescue.Learned = true;
            rescue.Proficiency = 70;
            fighter.Skills.Add(rescue);



            #endregion



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