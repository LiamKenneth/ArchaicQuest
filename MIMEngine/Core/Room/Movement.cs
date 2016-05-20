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
    using MIMHubServer;
    public static class Movement
    {
        public static void EnterRoom(Player player, Room room, string direction = "")
        {
            string name = player.Name;
            string movement = "walks in"; // runs, hovers, crawls. Steps out of a portal, appears?
            direction = oppositeDirection(direction);
            string enterText = name + " " + movement + direction;

            foreach (var players in room.players)
            {
                if (player.Name != players.Name)
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", enterText, players.HubGuid);
                }
                else
                {
                    enterText = "You walk in " + direction;
                    HubProxy.MimHubServer.Invoke("SendToClient", enterText, player.HubGuid);
                }
            }

        }

        public static void ExitRoom(Player player, Room room, string direction)
        {
            string name = player.Name;
            string movement = "walks "; // runs, hovers, crawls. Steps out of a portal, appears?
            string exitDir = direction;                             // string prevDirection = "South";

            string exitText = name + " " + movement + exitDir;

            foreach (var players in room.players)
            {
                if (player.Name != players.Name)
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", exitText, players.HubGuid);
                }
                else
                {
                    exitText = "You walk " + direction;
                    HubProxy.MimHubServer.Invoke("SendToClient", exitText, player.HubGuid);
                }
            }
        }

        public static string oppositeDirection(string direction)
        {


            switch (direction)
            {
                case "North":
                    {
                        return " from the South";
                    }
                case "East":
                    {
                        return " from the West";
                    }
                case "West":
                    {
                        return " from the East";
                    }
                case "South":
                    {
                        return " from the North";
                    }
                case "Up":
                    {
                        return " from down the stairs";
                    }
                case "Down":
                    {
                        return " from Upstairs";
                    }
                default:
                    {
                        return string.Empty;

                    }

            }

        }

        public async static void Move(Player player, Room room, string direction)
        {

            Room roomData = room;

            //Find Exit
            var exit = roomData.exits.Find(x => x.name == direction);

            if (exit != null)
            {

                //remove player from old room
                PlayerManager.RemovePlayerFromRoom(roomData, player);

                //exit message
                ExitRoom(player, roomData, direction);

                //change player Location
                player.Area = exit.area;
                player.AreaId = exit.areaId;
                player.Region = exit.region;

                //Get new room  
                try
                {
                    //Room getNewRoom =  await HubProxy.MimHubServer.Invoke<Room>("getRoom", player.HubGuid);
                    Room getNewRoom = MimHubServer.getRoom(player.HubGuid);

                    if (getNewRoom != null)
                    {
                        //add player to new room
                        PlayerManager.AddPlayerToRoom(getNewRoom, player);

                        //enter message
                        EnterRoom(player, getNewRoom, direction);

                        var roomDescription = LoadRoom.DisplayRoom(getNewRoom);

                        HubProxy.MimHubServer.Invoke("SendToClient", roomDescription, player.HubGuid);
                    }
                }
                catch (Exception e)
                {
                    //log error
                }
            }
            else
            {
                HubProxy.MimHubServer.Invoke("SendToClient", "There is no exit here");
            }
        }

    }

}
