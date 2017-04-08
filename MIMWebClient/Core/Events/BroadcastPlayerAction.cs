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

            var getPlayer = playersInRoom.FirstOrDefault(x => x.HubGuid.Equals(playerHub));
           
            foreach (var character in playersInRoom)
            {
                if (getPlayer != character)
                {
                  
                    var roomMessage = $"{ Helpers.ReturnName(getPlayer, character, string.Empty)} says {messageForRoom}.";

                    HubContext.SendToClient(roomMessage, character.HubGuid);
                }
            }
        }
       
    }
}