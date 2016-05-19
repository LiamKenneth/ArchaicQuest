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

        public static void Move(Player player, ConcurrentDictionary<int, Core.Room.Room> room, string direction)
        {

            Room roomData = null;

            if (room.TryGetValue(player.AreaId, out roomData))
            {
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

                    Room getNewRoom = null;

                    if (room.TryGetValue(player.AreaId, out getNewRoom))
                    {

                        //add player to new room
                        PlayerManager.AddPlayerToRoom(getNewRoom, player);

                        //enter message
                        EnterRoom(player);

                        //Display Room
                        LoadRoom.DisplayRoom(getNewRoom);
                    }
                    else
                    {
                        //get room for DB
                        LoadRoom getRoomFromDB = new LoadRoom();

                        getRoomFromDB.id = player.AreaId;
                        getRoomFromDB.Area = player.Area;
                        getRoomFromDB.Region = player.Region;

                        Room newRoom = getRoomFromDB.LoadRoomFile();

                        //add player to new room
                        PlayerManager.AddPlayerToRoom(newRoom, player);

                        //enter message
                        EnterRoom(player);

                        //Display Room
                       string newRoomData = LoadRoom.DisplayRoom(newRoom);

                        HubProxy.MimHubServer.Invoke("SendToClient", newRoomData);

                        //save new room to cache
                        
                             HubProxy.MimHubServer.Invoke("SaveRoom", newRoomData);
                    }
                }
                else
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", "There is no exit here");
                }
            }

        }

        //private static async Task GetRoom(int exitID)
        //{
        //    //adding .ConfigureAwait(false) may NOT be what you want but google it.
        //    return await Task.Run(() => HubProxy.MimHubServer.Invoke("ReturnRoom", exitID).ConfigureAwait(true));
        //}

       
    }
}
