using System;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using System.Collections.Generic;

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
                    HubContext.getHubContext.Clients.Client(players.HubGuid).addNewMessageToPage(enterText);
                }
                else
                {
                    enterText = "You walk in " + direction;
                    HubContext.getHubContext.Clients.Client(players.HubGuid).addNewMessageToPage(enterText);
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
                    HubContext.getHubContext.Clients.Client(players.HubGuid).addNewMessageToPage(exitText);
                    
                }
                else
                {
                    exitText = "You walk " + direction;
                    HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(exitText);
                }
            }
        }

        public static string oppositeDirection(string direction)
        {


            switch (direction)
            {
                case "North":
                    {
                        return "from the South";
                    }
                case "East":
                    {
                        return "from the West";
                    }
                case "West":
                    {
                        return "from the East";
                    }
                case "South":
                    {
                        return "from the North";
                    }
                case "Up":
                    {
                        return "from down the stairs";
                    }
                case "Down":
                    {
                        return "" + "" + "rom Upstairs";
                    }
                default:
                    {
                        return string.Empty;

                    }

            }

        }

        public  static void Move(Player player, Room room, string direction)
        {

            Room roomData = room;

            if (roomData.exits == null)
            {
                room.exits = new List<Exit>();
            }

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
                    Room getNewRoom =  MIMWebClient.Hubs.MIMHub.getRoom(player.HubGuid);

                    if (getNewRoom != null)
                    {
                        //add player to new room
                        PlayerManager.AddPlayerToRoom(getNewRoom, player);

                        //enter message
                        EnterRoom(player, getNewRoom, direction);

                        var roomDescription = LoadRoom.DisplayRoom(getNewRoom, player.Name);

                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(roomDescription);

                        //NPC Enter event here
                        foreach (var mob in getNewRoom.mobs)
                        {

                            if (mob.Greet)
                            {
                                Event.ParseCommand("greet", player, mob, getNewRoom);
                            }
                            else
                            {
                                //mob might be aggro
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    //log error
                }
            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("There is no exit here");
              
            }
        }

    }

}
