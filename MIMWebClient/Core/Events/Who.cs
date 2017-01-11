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

            var whoList = new StringBuilder();
            whoList.Append(Cache.ReturnPlayers().Count).Append(" Players currently playing <br />");
            foreach (var player in Cache.ReturnPlayers())
            {
                
                    whoList.Append("<p>[")
                        .Append(player.Level)
                        .Append(" ")
                        .Append(player.Race.Truncate(5))
                        .Append(" ")
                        .Append(player.SelectedClass.Truncate(3))
                        .Append("]")
                        .Append(" ")
                        .Append(player.Name).Append("</p><br />");
                
                 
            }

            HubContext.SendToClient(whoList.ToString(), playerData.HubGuid);
        }
    }
}