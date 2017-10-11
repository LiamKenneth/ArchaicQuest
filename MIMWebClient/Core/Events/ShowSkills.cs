using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;

    using PlayerClass = MIMWebClient.Core.Player.Classes.PlayerClass;

    public class ShowSkills : Skill
    {
        public static void ShowPlayerSkills(Player player, string commandOption)
        {
            if (commandOption.StartsWith("all"))
            {
                ShowPlayerAllSkills(player);
            }
            else
            {
              

                HubContext.Instance.SendToClient("You currently know these skills:", player.HubGuid);

                foreach (var skill in player.Skills)
                {

                    HubContext.Instance.SendToClient(
                        "Level " + skill.LevelObtained + ": " + skill.Name + " " + skill.Proficiency + "%",
                        player.HubGuid);
                }
            }
        }

        public static void ShowPlayerAllSkills(Player player)
        {
            var getClassSkills = GetSkills(player);

            HubContext.Instance.SendToClient("You will eventually learn these skills:", player.HubGuid);

            var skills = getClassSkills.OrderBy(o => o.LevelObtained).ToList();

            foreach (var skill in skills)
            {

                HubContext.Instance.SendToClient("Level " + skill.LevelObtained + ": " + skill.Name + " " + skill.Proficiency + "%", player.HubGuid);
            }

        }

        public static List<Skill> GetSkills(Player player)
        {
           return PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.Equals(player.SelectedClass)).Value.Skills;
          
        }
    }
}