using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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

                var skillTable = new StringBuilder();

                skillTable.Append("<table><tr><td>Skill Name</td><td>Skill Level</td></tr>");

                foreach (var skill in player.Skills.OrderBy(x => x.LevelObtained))
                {
                    if (skill.SkillType == Type.Crafting)
                    {
                        skillTable.Append($"<tr><td>{skill.Name}</td><td>Rank { CraftRank(skill.Points)}</td></tr>");
                    }
                    else { skillTable.Append($"<tr><td>{skill.Name}</td><td>{ SkillRank(skill.Proficiency)}</td></tr>"); }
                   
                   
                }

                skillTable.Append("</table>");

                HubContext.Instance.SendToClient(skillTable.ToString(), player.HubGuid);
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

        public static string SkillRank(int percentage)
        {

            if (percentage == 0)
            {
                return "untrained 0%";
            }
            else if (percentage <= 25)
            {
                return $"Inept {percentage}%";
            }
            else if (percentage <= 50)
            {
                return $"Competent {percentage}%";
            }
            else if (percentage <= 75)
            {
                return $"Proficient {percentage}%";
            }
            else if (percentage <= 95)
            {
                return $"Expert {percentage}%";
            }

            return $"Expert {percentage}%";
        }

        public static string CraftRank(int percentage)
        {

            if (percentage == 0 || percentage <= 99)
            {
                return $"Helper ({percentage}/99)";
            }
            else if (percentage == 100 || percentage <= 199)
            {
                return $"Junior Apprentice ({percentage}/199)";
            }
            else if (percentage == 200 || percentage <= 299)
            {
                return $"Apprentice ({percentage}/299)";
            }
            else if (percentage == 300 || percentage <= 399)
            {
                return $"Neophyte ({percentage}/399)";
            }
            else if (percentage == 400 || percentage <= 499)
            {
                return $"Assistant ({percentage}/499)";
            }
            else if (percentage == 500 || percentage <= 599)
            {
                return $"Junior ({percentage}/599)";
            }
            else if (percentage == 600 || percentage <= 699)
            {
                return $"Journeyman ({percentage}/699)";
            }
            else if (percentage == 700 || percentage <= 799)
            {
                return $"Senior ({percentage}/799)";
            }
            else if (percentage == 800 || percentage <= 899)
            {
                return $"Master ({percentage}/899)";
            }
            else if (percentage == 900 || percentage <= 999)
            {
                return $"Grand Master ({percentage}/999)";
            }
            else if (percentage >= 1000)
            {
                return $"Legendary Grand Master";
            }

            return $"error no rank";
        }

        public static string checkRank(int percentage, Player player)
        {

            if (string.IsNullOrEmpty(player.ChosenCraft) && percentage == 99)
            {
                return "To increase your craft rank you need to commit to a craft.";
            }
            

            return String.Empty;
        }


        public static List<Skill> GetSkills(Player player)
        {
           return PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.Equals(player.SelectedClass)).Value.Skills;
          
        }
    }
}