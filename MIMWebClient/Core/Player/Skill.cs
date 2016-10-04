using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.Player.Skills;

    public class Skill
    {
        public string Name { get; set; }

        public double Proficiency { get; set; }

        public bool Passive { get; set; }

        public int Delay { get; set; }

        public int CoolDown { get; set; }

        //Says what status a skill can be used from
        // Fighting means in a fight only
        //Resting means everything
        //standing means all but resting
        //can't use skills while asleep
        public string UsableFromStatus { get; set; }

        public int LevelObtained { get; set; }

        public string HelpText { get; set; }

        public string Syntax { get; set; }

        /* List of skills, use this for skill names and finding skills */
        public static readonly string HandToHand = "Hand to hand";
        public static readonly string Kick = "Kick";

        public static List<Skill> Skills ()
        {
            var skillList =  new List<Skill>();
            skillList.Add(Player.Skills.Kick.KickSkill);
            skillList.Add(Player.Skills.HandToHand.HandToHandSkill);

            return skillList;
        }
    }
}
