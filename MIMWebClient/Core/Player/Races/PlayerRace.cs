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
        public int MaxStr { get; set; }
        public int MaxDex { get; set; }
        public int MaxCon { get; set; }
        public int MaxInt { get; set; }
        public int MaxWis { get; set; }
        public int MaxCha { get; set; }
        public int HungerModifier { get; set; }
        public int ThirstModifier { get; set; }
        public string Alignment { get; set; }
        public string[] Vulnerabilities { get; set; }
        public string[] Resistances { get; set; }
        /// <summary>
        /// Size of the race
        /// </summary>
        public int Size { get; set; }
        public List<string> SuggestedClass { get; set; }
        /// <summary>
        /// Humans and Half humans receive a bonus point to their class
        /// </summary>
        public bool PrimeStat { get; set; }

        //Add Weakness / Resistances


    }
}