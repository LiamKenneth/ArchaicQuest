using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class BroadcastPlayerAction
    {
        public static void BroadcastPlayerActions (string playerHub, string playerName, List<PlayerSetup.Player> playersInRoom, string messageForPlayer, string messageForRoom)
        {
            HubContext.SendToClient(messageForPlayer, playerHub);
            HubContext.broadcastToRoom(playerName + " " + messageForRoom, playersInRoom, playerHub, true);
        }
       
    }
}