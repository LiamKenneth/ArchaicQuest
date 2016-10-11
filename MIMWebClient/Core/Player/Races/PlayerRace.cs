using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Player.Classes;

namespace MIMWebClient.Core.Player.Races
{
    public class PlayerRace
    {
        public string Name { get; set; }
        public int ExperienceModifier { get; set; }     
        public List<Skill> InnateSkills { get; set;}
        public List<Spells> InnateSpells { get; set; }
        public Help HelpText { get; set; }
        public int StatBonusStr { get; set; }
        public int StatBonusDex { get; set; }
        public int StatBonusCon { get; set; }
        public int StatBonusInt { get; set; }
        public int StatBonusWis { get; set; }
        public int StatBonusCha { get; set; }
        public int HungerModifier { get; set; }
        public int ThirstModifier { get; set; }

        //Add Weakness / Resistances


    }
}