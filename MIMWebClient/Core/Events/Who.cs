using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MIMWebClient.Core.Util;

namespace MIMWebClient.Core.Events
{
    public class Who
    {
        public static void Connected(PlayerSetup.Player playerData)
        {
            var playerCount = Cache.ReturnPlayers().Count + 4;
            var whoList = new StringBuilder();
 
            whoList.Append("<p>" + playerCount).Append(" players currently playing:</p>");
            whoList.Append("<p>[Imm][")
                        .Append("51")
                        .Append(" ")
                        .Append("Elf".Truncate(5))
                        .Append(" ")
                        .Append("Mage".Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append("Malleus").Append("</p> ");

                        whoList.Append("<p>[Imm][")
                        .Append("51")
                        .Append(" ")
                        .Append("Human".Truncate(5))
                        .Append(" ")
                        .Append("Fighter".Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append("Gamia").Append("</p> ");

                                    whoList.Append("<p>[")
                        .Append("9")
                        .Append(" ")
                        .Append("Mau".Truncate(5))
                        .Append(" ")
                        .Append("Cleric".Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append("Kencori").Append("</p> ");

                        whoList.Append("<p>[")
                        .Append("5")
                        .Append(" ")
                        .Append("Human".Truncate(5))
                        .Append(" ")
                        .Append("Fighter".Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append("Makkan").Append("</p> ");

            foreach (var player in Cache.ReturnPlayers())
            {
                if (player.Name == "Malleus" || player.Name == "Gamia" || player.Name == "Kencori" || player.Name == "Makkan")
                {
                    continue;
                }
                    whoList.Append("<p>[")
                        .Append(player.Level)
                        .Append(" ")
                        .Append(player.Race.Truncate(5))
                        .Append(" ")
                        .Append(player.SelectedClass.Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append(player.Name).Append("</p>");
                          
            }

            whoList.Append("<br /><p>This is a new game so player numbers wont be high yet, but we are active and improving the game everyday,<a href='https://discord.gg/nuf7FVq'> join the discord channel and say hi! </a></p>");
           
            HubContext.Instance.SendToClient(whoList.ToString(), playerData.HubGuid);
        }
    }
}