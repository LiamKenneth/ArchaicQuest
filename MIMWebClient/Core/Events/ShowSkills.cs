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
                var getClassSkills =
                    PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.Equals(player.SelectedClass));

                HubContext.SendToClient("You currently know these skills:", player.HubGuid);

                var skills =
                    getClassSkills.Value.Skills.Where(x => x.LevelObtained <= player.Level)
                        .OrderBy(o => o.LevelObtained)
                        .ToList();

                foreach (var skill in skills)
                {

                    HubContext.SendToClient(
                        "Level " + skill.LevelObtained + ": " + skill.Name + " " + skill.Proficiency + "%",
                        player.HubGuid);
                }
            }
        }

        public static void ShowPlayerAllSkills(Player player)
        {
            var getClassSkills = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.Equals(player.SelectedClass));

            HubContext.SendToClient("You will eventually learn these skills:", player.HubGuid);

            var skills = getClassSkills.Value.Skills.OrderBy(o => o.LevelObtained).ToList();

            foreach (var skill in skills)
            {

                HubContext.SendToClient("Level " + skill.LevelObtained + ": " + skill.Name + " " + skill.Proficiency + "%", player.HubGuid);
            }
        }
    }
}