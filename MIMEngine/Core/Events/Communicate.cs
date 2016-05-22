using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using MIMEngine.Core.PlayerSetup;
    

    public class Communicate
    {
        public static void Say(string message, Player player, bool recipient = false)
        {
            string playerId = player.HubGuid;
            string recipientName = string.Empty;
            Player recipientPlayer = null;
            if (recipient != false)
            {
                 recipientPlayer = MIMHubServer.MimHubServer._PlayerCache.FirstOrDefault(x => x.Value.Name == recipientName).Value;
            }

            if (recipientPlayer == null)
            {
                HubContext.SendToClient("You say " + message, playerId, null, false, false);
                HubContext.SendToClient(player.Name + " says " + message, playerId, null, true, true);
               
            }
            else
            {
                //self
                HubContext.SendToClient("You say to " + recipientName + " " + message, playerId, null, false, false);
                // to room
                HubContext.SendToClient(player.Name + " says to " + recipientName + message, playerId, null, true, true, true);

                string recipientPlayerId = MIMHubServer.MimHubServer._PlayerCache.FirstOrDefault(x => x.Value.Name == recipientName).Value.HubGuid;
                // to recipient
                HubContext.SendToClient(player.Name + " says to you " + message, playerId, recipientName, true, true);

               
            }
        }
    }
}
