using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Room
{
    using System.Security.Cryptography.X509Certificates;

    using Microsoft.AspNet.SignalR.Client;

    using MIMEngine.Core.PlayerSetup;

    public class Movement
    {
        public static void EnterRoom(Player player)
        {
            string name = player.Name;
            string movement = "walks in"; // runs, hovers, crawls. Steps out of a portal, appears?
           // string prevDirection = "South";

            string enterText = name + " " + movement;

            HubProxy.MimHubServer.Invoke("SendToClient", enterText);
        }

        public static void ExitRoom(Player player, string direction)
        {
            string name = player.Name;
            string movement = "walks "; // runs, hovers, crawls. Steps out of a portal, appears?
            string exitDir = direction;                             // string prevDirection = "South";

            string exitText = name + " " + movement + exitDir;

            HubProxy.MimHubServer.Invoke("SendToClient", exitText);
        }

        public static void Move(Player player, Room room, string direction)
        {
            //Find Exit
            var exit = room.exits.Find(x => x.name == direction);

            if (exit != null)
            {

                //remove player from old room
                PlayerManager.RemovePlayerFromRoom(room, player);

                //exit message
                ExitRoom(player, direction);

                //change player Location
                player.Area = exit.area;
                player.AreaId = exit.areaId;
                player.Region = exit.region;

                //GEt new room

                Room getRoom = null;

                if (getRoom != null)
                {
                     
                    //add player to new room
                    PlayerManager.AddPlayerToRoom(getRoom, player);

                    //enter message
                    EnterRoom(player);
                }
            }
            else
            {
                HubProxy.MimHubServer.Invoke("SendToClient", "There is no exit here");
            }

             
        }

        //private static async Task WrapSomeMethod(int exitID)
        //{
        //    //adding .ConfigureAwait(false) may NOT be what you want but google it.
        //    return await Task.Run(() => HubProxy.MimHubServer.Invoke("ReturnRoom", exitID).ConfigureAwait(true));
        //}

       
    }
}
