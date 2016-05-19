using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Room
{
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    using Microsoft.AspNet.SignalR.Client;

    using MIMEngine.Core.Events;
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

            Room roomData = room;

            //Find Exit
            var exit = roomData.exits.Find(x => x.name == direction);

            if (exit != null)
            {

                //remove player from old room
                PlayerManager.RemovePlayerFromRoom(roomData, player);

                //exit message
                ExitRoom(player, direction);

                //change player Location
                player.Area = exit.area;
                player.AreaId = exit.areaId;
                player.Region = exit.region;

                //GEt new room                  
                Room getNewRoom = HubProxy.MimHubServer.Invoke<Room>("getRoom", player.HubGuid).Result; //returns string

                if (getNewRoom != null)
                {
                    //add player to new room
                    PlayerManager.AddPlayerToRoom(getNewRoom, player);

                    //enter message
                    EnterRoom(player);

                    var roomDescription = LoadRoom.DisplayRoom(getNewRoom);

                    HubProxy.MimHubServer.Invoke("SendToClient", roomDescription);
                }
            }
            else
            {
                HubProxy.MimHubServer.Invoke("SendToClient", "There is no exit here");
            }
        }

    }

}
