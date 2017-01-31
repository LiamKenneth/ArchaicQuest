using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.Player.Skills;

    using MongoDB.Bson.Serialization.Attributes;

    public class Skill
    {
        public string Name { get; set; }

        public int Proficiency { get; set; }

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

        public int ManaCost { get; set; }

        public int MaxProficiency { get; set; }
        [BsonIgnore]
        public Help HelpText { get; set; }
        [BsonIgnore]
        public string Syntax { get; set; }


        public static bool CheckPlayerHasSkill(PlayerSetup.Player player, string skillName)
        {

            //Check if player has spell
            var hasSpell =
               player.Skills.FirstOrDefault(
                   x =>
                       x.Name.Equals(skillName) &&
                       x.LevelObtained <= player.Level);

            if (hasSpell != null)
            {
                return true;
            }


            return false;
        }


        public static string GetSkillTarget(string target)
        {
            if (target == "")
            {
                return String.Empty;
            }

            // check to see if user typed "C magic target" or just "c magic"
            // if an actual target there wil lbe a space
            var targetLength = target.Count(x => x.Equals(' '));
            if (targetLength >= 1)
            {
                string theTarget;
                var hasQuotes = target.Contains("'\"");

                if (hasQuotes)
                {
                    theTarget = target.Substring(target.LastIndexOf('"') + 1);

                    if (string.IsNullOrEmpty(theTarget))
                    {
                        theTarget = target.Substring(target.LastIndexOf('\'') + 1);
                    }
                }
                else
                {
                    theTarget = target.Substring(target.LastIndexOf(' ') + 1);
                }


                return theTarget;

            }

            return String.Empty;
        }

    }
}
