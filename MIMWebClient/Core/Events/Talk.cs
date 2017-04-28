using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class Talk
    {
        public static void TalkTo(string message, Room.Room room, PlayerSetup.Player player)
        {
            string playerName = message;
            string actualMessage = string.Empty;
            int indexOfSpaceInUserInput = message.IndexOf(" ", StringComparison.Ordinal);

            if (indexOfSpaceInUserInput > 0)
            {
                playerName = message.Substring(0, indexOfSpaceInUserInput);

                if (indexOfSpaceInUserInput != -1)
                {
                    actualMessage =
                        message.Substring(indexOfSpaceInUserInput, message.Length - indexOfSpaceInUserInput).TrimStart();
                    // message is everythign after the 1st space
                }
            }

            string playerId = player.HubGuid;

            PlayerSetup.Player recipientPlayer =
                (PlayerSetup.Player) ManipulateObject.FindObject(room, player, "", playerName, "all");

            if (recipientPlayer != null)
            {
                string recipientName = recipientPlayer.Name;
                HubContext.SendToClient("You say to \"" + recipientName + "\" " + actualMessage, playerId, null, false,
                    false);
                HubContext.SendToClient(player.Name + " says to you \"" + actualMessage + "\"", playerId, recipientName, true,
                    true);



            }
            else
            {
                HubContext.SendToClient("No one here by that name", playerId, null, false, false);
            }
        }
    }
}