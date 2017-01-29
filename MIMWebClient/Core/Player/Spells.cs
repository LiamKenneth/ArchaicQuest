using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Room;

namespace MIMWebClient.Core.Player
{
    public class Spells : Skill
    {
        public static void DoSpell(PlayerSetup.Player playerData, Room.Room room, string commandOptions)
        {
            var spell =
                playerData.Skills.Where(
                    x =>
                        x.Name.StartsWith(commandOptions, StringComparison.CurrentCultureIgnoreCase) &&
                        x.LevelObtained <= playerData.Level);

            if (spell != null)
            {
               // spell.FirstOrDefault()
            }
        }
    }
}