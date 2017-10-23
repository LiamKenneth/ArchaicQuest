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

 
            #region  Give mage magic missile skill

            var magicMissile = MagicMissile.MagicMissileAb();

            magicMissile.LevelObtained = 1;
            magicMissile.Proficiency = 50;
            magicMissile.MaxProficiency = 95;
            mage.Skills.Add(magicMissile);

            #endregion

            #region  Give mage armor skill

            var armour = Armour.ArmourAb();

            armour.LevelObtained = 1;
            armour.Proficiency = 50;
            armour.MaxProficiency = 95;
            mage.Skills.Add(armour);

            #endregion

            #region  Give invis skill

            var invis = Invis.InvisAb();

            invis.LevelObtained = 1;
            invis.Proficiency = 50;
            invis.MaxProficiency = 95;
            mage.Skills.Add(invis);

            #endregion

            #region  Give continual light skill

            var continualLight = ContinualLight.ContinualLightAb();

            continualLight.LevelObtained = 1;
            continualLight.Proficiency = 50;
            continualLight.MaxProficiency = 95;
            mage.Skills.Add(continualLight);

            #endregion

            #region  Give weaken

            var weaken = Weaken.WeakenAb();

            weaken.LevelObtained = 1;
            weaken.Proficiency = 50;
            weaken.MaxProficiency = 95;
            mage.Skills.Add(weaken);

            #endregion

            #region  Give chill touch

            var chillTouch = ChillTouch.ChillTouchAb();

            chillTouch.LevelObtained = 1;
            chillTouch.Proficiency = 50;
            chillTouch.MaxProficiency = 95;
            mage.Skills.Add(chillTouch);

            #endregion

            #region  Give fly

            var fly = Fly.FlyAb();

            fly.LevelObtained = 1;
            fly.Proficiency = 50;
            fly.MaxProficiency = 95;
            mage.Skills.Add(fly);

            #endregion

            #region  Give Faerie Fire

            var faerieFire = FaerieFire.FaerieFireAB();

            faerieFire.LevelObtained = 1;
            faerieFire.Proficiency = 50;
            faerieFire.MaxProficiency = 95;
            mage.Skills.Add(faerieFire);

            #endregion

            #region  Give refresh

            var refresh = Refresh.RefreshAb();

            refresh.LevelObtained = 1;
            refresh.Proficiency = 50;
            refresh.MaxProficiency = 95;
            mage.Skills.Add(refresh);

            #endregion

            #region  Give teleport

            var teleport = Teleport.TeleporAb();

            teleport.LevelObtained = 1;
            teleport.Proficiency = 50;
            teleport.MaxProficiency = 95;
            mage.Skills.Add(teleport);

            #endregion

            #region  Give blindness

            var blindness = Blindness.BlindAb();

            blindness.LevelObtained = 1;
            blindness.Proficiency = 50;
            blindness.MaxProficiency = 95;
            mage.Skills.Add(blindness);

            #endregion

            #region  Give haste

            var haste = Haste.HasteAb();

            haste.LevelObtained = 1;
            haste.Proficiency = 50;
            haste.MaxProficiency = 95;
            mage.Skills.Add(haste);

            #endregion

            #region  Give Shocking grasp

            var shockingGrasp = ShockingGrasp.ShockingGraspAb();

            shockingGrasp.LevelObtained = 1;
            shockingGrasp.Proficiency = 50;
            shockingGrasp.MaxProficiency = 95;
            mage.Skills.Add(shockingGrasp);

            #endregion



            #region  Give create spring

            var createSpring = CreateSpring.CreateSpringAb();

            createSpring.LevelObtained = 1;
            createSpring.Proficiency = 50;
            createSpring.MaxProficiency = 95;
            mage.Skills.Add(createSpring);

            #endregion


            mage.ReclassOptions.Add(Ranger.RangerClass());

            return mage;


        }
    }
}