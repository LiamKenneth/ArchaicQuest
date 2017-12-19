using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Classes.Reclasses;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player.Skills;

    public class Thief : PlayerClass
    {
        public static PlayerClass ThiefClass()
        {


            var thief = new PlayerClass
            {
                Name = "Thief",
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

            var longBlades = LongBlades.LongBladesAb();
            longBlades.Learned = true;
            thief.Skills.Add(longBlades);

            var shortBlades = ShortBlades.ShortBladesAb();
            shortBlades.Learned = true;
            thief.Skills.Add(shortBlades);


            var staff = Staff.StaffAb();
            staff.Learned = true;
            thief.Skills.Add(staff);

            var handToHand = HandToHand.HandToHandAb();
            handToHand.Learned = true;
            thief.Skills.Add(handToHand);

            var lightArmour = LightArmour.LightArmourAb();
            lightArmour.Learned = true;
            thief.Skills.Add(lightArmour);

            #endregion

            #region  Lvl 2 skills
            var dodge = Player.Skills.Dodge.DodgeAb();
            dodge.LevelObtained = 2;
            thief.Skills.Add(dodge);

            #endregion

            #region  Lvl 3 skills
            var sneak = Player.Skills.Sneak.SneakAb();
            sneak.LevelObtained = 3;
            thief.Skills.Add(dodge);

            var exotic = Exotic.ExoticAb();
            exotic.LevelObtained = 3;
            thief.Skills.Add(exotic);

            #endregion

            #region  Lvl 4 skills
            var kick = Player.Skills.Kick.KickAb();
            kick.LevelObtained = 5;
            thief.Skills.Add(kick);

            #endregion


            #region Lvl 5 skills

            var blunt = BluntWeapons.BluntWeaponsAb();
            blunt.LevelObtained = 5;
            thief.Skills.Add(blunt);


            #endregion
 

            #region Lvl 7 skills

            var trip = Trip.TripAb();
            trip.LevelObtained = 7;
            thief.Skills.Add(trip);
            #endregion

            #region Lvl 8 skills

            var lore = Lore.LoreAb();
            thief.Skills.Add(lore);
            #endregion

            #region Lvl 9 skills

            var hide = Hide.HideAb();
            hide.LevelObtained = 9;
            thief.Skills.Add(hide);
            #endregion

            #region Lvl 10 skills

            var skick = SpinKick.KickAb();
            thief.Skills.Add(skick);

            var peak = Peak.PeakAb();
            thief.Skills.Add(peak);
            #endregion

            #region Lvl 11 skills

            var steal = Steal.StealAb();
            thief.Skills.Add(steal);

       
            #endregion


            #region Lvl 12 skills

            var tumble = Tumble.TumbleAb();
            thief.Skills.Add(tumble);
            #endregion


            #region Lvl 13 skills

            var picklock = LockPick.LockPickAb();
            thief.Skills.Add(picklock);
            #endregion

            #region Lvl 14 skills

            var dkick = DirtKick.DirtKickAb();
            dkick.LevelObtained = 14;
            thief.Skills.Add(dkick);
            #endregion


            #region Lvl 15 skills

            var bstab = Backstab.BackstabAb();
            thief.Skills.Add(bstab);
            #endregion

            #region Lvl 16 skills

            var parry = Parry.ParryAb();
            parry.LevelObtained = 16;
            thief.Skills.Add(parry);
            #endregion


            #region Lvl 18 skills

            var disarm = Disarm.DisarmAb();
            disarm.LevelObtained = 18;
            thief.Skills.Add(disarm);
            #endregion

            #region Lvl 20 skills

            var feint = Feint.FeintAb();
           
            thief.Skills.Add(feint);
            #endregion



            thief.ReclassOptions.Add(Ranger.RangerClass());

            return thief;


        }
    }
}