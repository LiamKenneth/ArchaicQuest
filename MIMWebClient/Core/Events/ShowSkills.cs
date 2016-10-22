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
        public static void ShowPlayerSkills(Player player)
        {
            var getClassSkills = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.Equals(player.SelectedClass));
            
            HubContext.SendToClient("You currently know these skills:", player.HubGuid);           

            foreach (var skill in getClassSkills.Value.Skills)
            {
                HubContext.SendToClient("Level: " + skill.LevelObtained + " ".PadRight(3) + skill.Name.PadRight(3) + " " + skill.Proficiency +"%", player.HubGuid);
            }
        }
    }
}