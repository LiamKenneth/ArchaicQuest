using Microsoft.AspNet.SignalR;
using MIMHubServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMEngine.Core.Player;

namespace MIMEngine.Core
{
    using MIMEngine.Core.Player;
    public static class HubContext
    {
        private static IHubContext _getHubContext;
        /// <summary>
        /// gets the SignalR Hub connection
        /// </summary>
        public static IHubContext getHubContext
        {
            get
            {
                if (_getHubContext == null)
                {
                    _getHubContext = GlobalHost.ConnectionManager.GetHubContext<MimHubServer>();
                }

                return _getHubContext;
            }
        }

        /// <summary>
        /// Send a message to connected clients
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="player">The active player</param>
        /// <param name="sendToAll">toggle to send to all or to caller</param>
        /// <param name="excludeCaller">toggle to send to all but exclude caller</param>
        public static void SendToClient(string message, string playerId, string recipientId = null, bool sendToAll = false, bool excludeCaller = false, bool excludeRecipient = false)
        {
            if (message != null)
            {

                //Send a message to all users
                if (sendToAll && excludeCaller == false)
                {
                    HubContext.getHubContext.Clients.All.addNewMessageToPage(message);
                }

                if (playerId != null)
                {
                    //send a message to all but caller
                    if (excludeCaller)
                    {
                        HubContext.getHubContext.Clients.AllExcept(playerId).addNewMessageToPage(message);
                    }

                    //send a message to all but recipient
                    if (excludeRecipient && recipientId != null)
                    {
                        HubContext.getHubContext.Clients.Client(recipientId).addNewMessageToPage(message);
                    }

                    //send only to caller
                    if (sendToAll == false && excludeCaller == false)
                    {
                        HubContext.getHubContext.Clients.Client(playerId).addNewMessageToPage(message);
                    }


                }
            }
        }

        public static void broadcastToRoom(string message, List<PlayerSetup.Player> players, string playerId, bool excludeCaller = false)
        {
            int playerCount = players.Count;

            if (excludeCaller)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    if (playerId != players[i].HubGuid)
                    {
                        HubContext.getHubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                    }                  
                }
            }
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    HubContext.getHubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                }
            }

           
        }
    }
}
