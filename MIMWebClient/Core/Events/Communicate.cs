using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Events
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;



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

                //check npc response
                

            }
            else
            {
                HubContext.SendToClient("No one here by that name", playerId, null, false, false);
            }
        }


        public static void NewbieChannel(string message, Player player)
        {          
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("Newbie: " + player.Name + " says " +  message, pc.HubGuid);
            }

        }

        public static void GossipChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("Gossip: " + player.Name + " says " + message, pc.HubGuid);
            }

        }

        public static void OocChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("OOC: " + player.Name + " says " + message, pc.HubGuid);
            }

        }

    }
}
