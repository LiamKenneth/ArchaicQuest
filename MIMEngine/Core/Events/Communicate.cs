using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;



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

        public static void SayTo(string message, Room room, Player player)
        {
            string playerName = message;
            string actualMessage = string.Empty;
            int indexOfSpaceInUserInput = message.IndexOf(" ", StringComparison.Ordinal);
  
            if (indexOfSpaceInUserInput > 0 )
            {
                playerName = message.Substring(0, indexOfSpaceInUserInput);
            }
         
            string playerId = player.HubGuid;
           
             Player recipientPlayer = (Player)ManipulateObject.FindObject(room, player, "", playerName, "all");

            if (recipientPlayer != null)
            {
                string recipientName = recipientPlayer.Name;
                HubContext.SendToClient("You say to " + recipientName + " " + actualMessage, playerId, null, false, false);
                HubContext.SendToClient(player.Name + " says to you " + actualMessage, playerId, recipientName, true, true);

            }
            else
            {
                HubContext.SendToClient("No one here by that name", playerId, null, false, false);
            }
        }

    }
}
