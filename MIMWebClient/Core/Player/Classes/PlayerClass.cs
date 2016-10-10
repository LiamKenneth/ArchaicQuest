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
        public int MinHpGain { get; set; }
        public int MaxHpGain { get; set; }
        public int MinManaGain { get; set; }
        public int MaxManaGain { get; set; }
        public Help HelpText { get; set; }

        //add THAC0
    }
}