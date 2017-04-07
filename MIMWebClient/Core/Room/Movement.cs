using System;
using System.Linq;
using MIMWebClient.Core.World.Tutorial;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using System.Collections.Generic;

    public static class Movement
    {
        public static void EnterRoom(Player player, Room room, string direction = "")
        {
            string name = Helpers.ReturnName(player, string.Empty);
            string movement = "walks in "; // runs, hovers, crawls. Steps out of a portal, appears?
            direction = oppositeDirection(direction);
            string enterText = name + " " + movement + direction;

            for (int i = 0; i < room.players.Count; i++)
            {

                if (player.Name != room.players[i].Name)
                {
                    HubContext.getHubContext.Clients.Client(room.players[i].HubGuid).addNewMessageToPage(enterText);
                }
                else
                {
                    if (player.Status == Player.PlayerStatus.Standing)
                    {
                        enterText = "You walk in " + direction;
                        HubContext.getHubContext.Clients.Client(room.players[i].HubGuid).addNewMessageToPage(enterText);
                    }

                }

                var roomdata = LoadRoom.DisplayRoom(room, room.players[i].Name);
                Score.UpdateUiRoom(room.players[i], roomdata);
            }

          

            if (room.EventOnEnter != null)
            {
                Event.ParseCommand(room.EventOnEnter, player, null, room);
            }

            //NPC Enter event here

            foreach (var mob in room.mobs)
            {              

                if (mob.EventOnEnter != null)
                {
                    Event.ParseCommand(mob.EventOnEnter, player, mob, room);
                }

            }


        }

        public static void ExitRoom(Player player, Room room, string direction)
        {
            string name = Helpers.ReturnName(player, string.Empty);
            string movement = "walks "; // runs, hovers, crawls. Steps out of a portal, appears?
            string exitDir = direction;                             // string prevDirection = "South";

            string exitText = name + " " + movement + exitDir;

            for (int i = 0; i < room.players.Count; i++)
            {
                if (player.Name != room.players[i].Name)
                {
                    HubContext.getHubContext.Clients.Client(room.players[i].HubGuid).addNewMessageToPage(exitText);

                }
                else
                {
                    exitText = "You walk " + direction;
                    HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(exitText);
                }

                var roomdata = LoadRoom.DisplayRoom(room, room.players[i].Name);
                Score.UpdateUiRoom(room.players[i], roomdata);
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
                        return "" + "" + "from Upstairs";
                    }
                default:
                    {
                        return string.Empty;

                    }

            }

        }

        //Create transport method

        public static void Teleport(Player player, Room room, Exit exit)
        {

            Room roomData = room;


            //Find Exit


            //remove player from old room
            PlayerManager.RemovePlayerFromRoom(roomData, player);

            //exit message
            ExitRoom(player, roomData, null);

            //change player Location
            player.Area = exit.area;
            player.AreaId = exit.areaId;
            player.Region = exit.region;

            //Get new room  
            try
            {
                //Room getNewRoom =  await HubProxy.MimHubServer.Invoke<Room>("getRoom", player.HubGuid);
                Room getNewRoom = MIMWebClient.Hubs.MIMHub.getRoom(player.HubGuid);

                if (getNewRoom != null)
                {
                    //add player to new room
                    PlayerManager.AddPlayerToRoom(getNewRoom, player);

                    //enter message
                    EnterRoom(player, getNewRoom, null);

                    var roomDescription = LoadRoom.DisplayRoom(getNewRoom, player.Name);

                  if (player.Status != Player.PlayerStatus.Sleeping)
                    {
                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(roomDescription);

                    }


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

                        if (mob.DialogueTree != null && mob.DialogueTree.Count >= 1)
                        {
                            var speak = mob.DialogueTree[0];

                            HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(mob.Name + " says to you " + speak.Message);
                            var i = 1;
                            foreach (var respond in speak.PossibleResponse)
                            {
                                var textChoice = "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " + respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " + respond.Response + "</a>";
                                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(textChoice);
                                i++;

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //log error
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="room"></param>
        /// <param name="direction"></param>
        public static void Move(Player player, Room room, string direction)
        {

            Room roomData = room;

            if (roomData.exits == null)
            {
                room.exits = new List<Exit>();
            }

            //Find Exit
            if (roomData.exits != null)
            {
                var exit = roomData.exits.Find(x => x.name == direction);


                if (exit != null)
                {
                    if (exit.open == false)
                    {
                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("The " + exit.doorName + " is close");
                        return;
                    }

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
                        Room getNewRoom = MIMWebClient.Hubs.MIMHub.getRoom(player.HubGuid);

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
                                    // Event.ParseCommand("greet", player, mob, getNewRoom);
                                }
                                else
                                {
                                    //mob might be aggro
                                }

                                if (mob.DialogueTree != null)
                                {
                                    var speak = mob.DialogueTree[0];

                                    HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(mob.Name + " says to you " + speak.Message);
                                    var i = 1;
                                    foreach (var respond in speak.PossibleResponse)
                                    {
                                        var textChoice = "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " + respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " + respond.Response + "</a>";
                                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(textChoice);
                                        i++;

                                    }
                                }

                                if (mob.EventOnEnter != null)
                                {
                                    Event.ParseCommand(mob.EventOnEnter, player, mob, room);
                                }

                                foreach (var quest in player.QuestLog.Where(x => x.Completed == false))
                                {
                                    if (quest.QuestHint != null && mob.Name == quest.QuestFindMob)
                                    {
                                        HubContext.SendToClient(quest.QuestHint, player.HubGuid);
                                    }
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

}
