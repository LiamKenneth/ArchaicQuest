using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Classes
{
    public class PlayerClass
    {
        public string Name { get; set; }
        public List<PlayerClass> ReclassOptions { get; set; } 
        public List<Skill> Skills { get; set; } 
        public int ExperienceModifier { get; set; }
        public string HelpText { get; set; }

        //add THAC0
    }
}