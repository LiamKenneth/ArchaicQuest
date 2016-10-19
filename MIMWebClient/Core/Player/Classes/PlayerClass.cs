using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Classes
{
    using MIMWebClient.Core.Events;

    public class PlayerClass
    {
        public string Name { get; set; }
        public bool IsBaseClass { get; set; }
        public List<PlayerClass> ReclassOptions { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Spells> Spells { get; set; }
        public int ExperienceModifier { get; set; }
        /// <summary>
        /// HP/Mana/Endurance min/max gain is the min and max amount player 
        /// can roll before applying Con bonus to value
        /// </summary>
        public int MinHpGain { get; set; }
        public int MaxHpGain { get; set; }
        public int MinManaGain { get; set; }
        public int MaxManaGain { get; set; }
        public int MinEnduranceGain { get; set; }
        public int MaxEnduranceGain { get; set; }
        public Help HelpText { get; set; }
        public int StatBonusStr { get; set; }
        public int StatBonusDex { get; set; }
        public int StatBonusCon { get; set; }
        public int StatBonusInt { get; set; }
        public int StatBonusWis { get; set; }
        public int StatBonusCha { get; set; }

        //add THAC0

        public static Dictionary<string, PlayerClass> ClassList()
        {
            var classList = new Dictionary<string, PlayerClass>
            {
                {"Fighter", Fighter.FighterClass() },
                {"Mage", Mage.MageClass() },
                {"Cleric", Cleric.ClericClass()},
                {"Thief", Thief.ThiefClass() },


            };

            return classList;
        }

    }
}