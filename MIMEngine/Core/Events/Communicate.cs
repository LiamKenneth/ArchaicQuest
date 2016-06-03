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
        public static void Say(string message, Player player, Room room)
        {
            string playerId = player.HubGuid;

                HubContext.SendToClient("You say " + message, playerId, null, false, false);
                HubContext.broadcastToRoom(player.Name + " says " + message, room.players, playerId, true);
               
        }

        public static void SayTo(string message, Room room, Player player)
        {
            string playerName = message;
            string actualMessage = string.Empty;
            int indexOfSpaceInUserInput = message.IndexOf(" ", StringComparison.Ordinal);
  
            if (indexOfSpaceInUserInput > 0 )
            {
                playerName = message.Substring(0, indexOfSpaceInUserInput);

                if (indexOfSpaceInUserInput != -1)
                {
                    actualMessage = message.Substring(indexOfSpaceInUserInput, message.Length - indexOfSpaceInUserInput).TrimStart();
                        // message is everythign after the 1st space
                }
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
