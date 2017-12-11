using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Tutorial;
using MIMWebClient.Hubs;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using System.Collections.Generic;

    public static class Movement
    {
        public static void EnterRoom(Player player, Room room, string direction = "", bool teleported = false)
        {
            var directionOrigin = oppositeDirection(direction, true);

            for (int i = 0; i < room.players.Count; i++)
            {

                if (room.players[i].Status == Player.PlayerStatus.Sleeping)
                {
                    continue;
                }

                if (teleported == false)
                {
                    string name = Helpers.ReturnName(player, room.players[i], string.Empty);
                    string movement = "walks in "; // runs, hovers, crawls. Steps out of a portal, appears?


                    if (player.Effects?.FirstOrDefault(
                            x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        movement = "floats in ";
                    }

                    if ((player.Effects?.FirstOrDefault(
                             x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) == null && room.needsBoat))
                    {
                        movement = "swims in";
                    }

                    direction = oppositeDirection(direction, false);
                    string enterText = name + " " + movement + direction;

                    if (player.HubGuid != null)
                    {

                        if (player.Name != room.players[i].Name)
                        {
                            HubContext.Instance.AddNewMessageToPage(room.players[i].HubGuid, enterText);
                        }
                        else
                        {
                            if (player.Status == Player.PlayerStatus.Standing)
                            {

                                if (player.Effects?.FirstOrDefault(
                                        x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) != null)
                                {
                                    enterText = "You float in " + direction;
                                }
                                else if ((player.Effects?.FirstOrDefault(
                                              x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) ==
                                          null && room.needsBoat))
                                {
                                    enterText = "You swim in " + direction;

                                    Swim.SwimSuccess(player,
                                        player.Skills.FirstOrDefault(x => x.Name.Equals("Swim")).Proficiency);
                                }
                                else
                                {
                                    enterText = "You walk in " + direction;
                                }

                                HubContext.Instance.AddNewMessageToPage(room.players[i].HubGuid, enterText);
                            }

                        }

                        var roomdata = LoadRoom.DisplayRoom(room, room.players[i].Name);
                        Score.UpdateUiRoom(room.players[i], roomdata);

             
                    }
                    else
                    {
                        if (room.players[i].HubGuid != null)
                        {
                            //don't show if sneaking
                            if (!Effect.HasEffect(room.players[i], "Sneak"))
                            {
                                HubContext.Instance.SendToClient(enterText, room.players[i].HubGuid);
                            }
 
                          
                        }

                    }
                }
            }


            Score.UpdateUiMap(player, room.areaId, room.area.ToLower(), room.region.ToLower(), room.coords.Z = 0);
        }

        public static void ExitRoom(Player player, Room room, string direction, bool teleported = false)
        {
            var hasFly = player.Effects?.FirstOrDefault(
                             x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) != null;


            for (int i = 0; i < room.players.Count; i++)
            {
                if (teleported == false)
                {

                    string name = Helpers.ReturnName(player, room.players[i], string.Empty);
                    string movement = "walks "; // runs, hovers, crawls. Steps out of a portal, appears?

                    if (hasFly)
                    {
                        movement = "floats ";
                    }

                    string exitDir = direction; // string prevDirection = "South";
                    string exitText = name + " " + movement + exitDir;

                    if (player.Name != room.players[i].Name)
                    {
                      
                        //don't show if sneaking
                        if (!Effect.HasEffect(room.players[i], "Sneak"))
                        {
                            HubContext.Instance.AddNewMessageToPage(room.players[i].HubGuid, exitText);
                        }

                    }
                    else
                    {
                        exitText = "You walk " + direction;

                        if (hasFly)
                        {
                            exitText = "You float " + direction;
                        }

                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, exitText);
                    }

                }

                var roomdata = LoadRoom.DisplayRoom(room, room.players[i].Name);
                Score.UpdateUiRoom(room.players[i], roomdata);

            }

            if (teleported == false)
            {


                if (player.Followers != null && player.Followers.Count > 0)
                {

                    foreach (var follower in player.Followers.ToList())
                    {

                        if (follower.AreaId == player.AreaId && follower.Area == player.Area && follower.Region == player.Region)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(follower, player, string.Empty) + " follows you " + direction,
                                player.HubGuid);
                            HubContext.Instance.SendToClient(
                                "You follow " + Helpers.ReturnName(player, follower, string.Empty) + " " + direction,
                                follower.HubGuid);

                            if (follower.HubGuid == null)
                            {
                                Movement.MobMove(follower, player, room, direction);
                            }
                            else
                            {
                                Command.ParseCommand(direction, follower, room);
                            }

                        }

                    }

                }
            }


            if (!hasFly || player.Mount == null)
            {
                player.MovePoints -= 1;
            }


            Score.UpdateUiPrompt(player);

        }

        public static string oppositeDirection(string direction, bool forMobMovement)
        {
            if (forMobMovement)
            {
                switch (direction)
                {
                    case "North":
                        {
                            return "South";
                        }
                    case "East":
                        {
                            return "West";
                        }
                    case "West":
                        {
                            return "East";
                        }
                    case "South":
                        {
                            return "North";
                        }
                    case "Up":
                        {
                            return "Down";
                        }
                    case "Down":
                        {
                            return "Up";
                        }
                    default:
                        {
                            return string.Empty;

                        }

                }
            }

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

        public static void Teleport(Player player, Room room, Exit exit = null)
        {

            Room roomData = room;


            //remove player from old room
           PlayerManager.RemovePlayerFromRoom(roomData, player);

            if (exit != null)
            {
                //exit message
                ExitRoom(player, roomData, null, true);
            }
           
            //change player Location
            if (exit != null)
            {
                //change player Location
                player.Area = exit.area;
                player.AreaId = exit.areaId;
                player.Region = exit.region;
            }
            else
            {

                player.Area = room.area;
                player.AreaId = room.areaId;
                player.Region = room.region;
            }

            //Get new room  
            try
            {
                //Room getNewRoom =  await HubProxy.MimHubServer.Invoke<Room>("getRoom", player.HubGuid);
                Room getNewRoom = MIMWebClient.Hubs.MIMHub.getRoom(player);

                if (getNewRoom != null)
                {
                    //add player to new room
                    PlayerManager.AddPlayerToRoom(getNewRoom, player);

                    //enter message
                    EnterRoom(player, getNewRoom, null, true);

                    var roomDescription = LoadRoom.DisplayRoom(getNewRoom, player.Name);

                    if (player.Status != Player.PlayerStatus.Sleeping)
                    {
                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, roomDescription);

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

                        if (mob.MobTalkOnEnter && mob.DialogueTree != null && mob.DialogueTree.Count >= 1)
                        {
                            var speak = mob.DialogueTree[0];

                            HubContext.Instance.AddNewMessageToPage(player.HubGuid, "<span class='sayColor'>" + mob.Name + " says to you \"" + speak.Message + "\"</span>");
                            var i = 1;
                            foreach (var respond in speak.PossibleResponse)
                            {
                                var textChoice =
                                    "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                                    respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " + respond.Response +
                                    "</a>";
                                HubContext.Instance.AddNewMessageToPage(player.HubGuid, textChoice);
                                i++;

                            }
                        }
                    }

                    ShowUIExits(getNewRoom, player.HubGuid);
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
            if (player.Status == Player.PlayerStatus.Fighting)
            {
                HubContext.Instance.SendToClient("You are fighting! Try fleeing.", player.HubGuid);
                return;
            }

            if (player.Status == Player.PlayerStatus.Resting || player.Status == Player.PlayerStatus.Sleeping)
            {
                HubContext.Instance.SendToClient("You need to get up before you move.", player.HubGuid);
                return;
            }


            var isBlind = player.Effects?.FirstOrDefault(
                          x => x.Name.Equals("Blindness", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (player.MovePoints <= 0)
            {
                HubContext.Instance.SendToClient("You are exhausted an unable to move", player.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                        HubContext.Instance.SendToClient(
                            $"{Helpers.ReturnName(player, character, string.Empty)} tries to move but is too exhausted.",
                            player.HubGuid);
                    }
                }
                return;
            }

            if (player.ActiveSkill != null)
            {
                HubContext.Instance.SendToClient("You are too busy to move.", player.HubGuid);
                return;
            }


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
                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, "The " + exit.doorName + " is close");
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
                        Room getNewRoom = MIMWebClient.Hubs.MIMHub.getRoom(player);

                        if (getNewRoom != null)
                        {

                            //add player to new room
                            PlayerManager.AddPlayerToRoom(getNewRoom, player);

                      
                            //enter message
                            EnterRoom(player, getNewRoom, direction);

                            var roomDescription = LoadRoom.DisplayRoom(getNewRoom, player.Name);

                            if (!isBlind)
                            {
                                HubContext.Instance.AddNewMessageToPage(player.HubGuid, roomDescription);
                            }
                            else
                            {
                                HubContext.Instance.SendToClient("You can't see a thing.", player.HubGuid);
                            }

                            //Show exits UI
                            ShowUIExits(getNewRoom, player.HubGuid);

                            //NPC Enter event here


                 

                            foreach (var mob in getNewRoom.mobs)
                            {

                                if (mob.Greet)
                                {
                                    // Event.ParseCommand("greet", player, mob, getNewRoom);
                                }
                              

                                if (mob.MobTalkOnEnter && mob.DialogueTree != null && mob.DialogueTree.Count > 0)
                                {
                                    var speak = mob.DialogueTree[0];

                                    HubContext.Instance.AddNewMessageToPage(player.HubGuid, "<span class='sayColor'>" + mob.Name + " says to you \"" + speak.Message + "\"</span>");
                                    var i = 1;
                                    foreach (var respond in speak.PossibleResponse)
                                    {
                                        var textChoice =
                                            "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                                            respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " +
                                            respond.Response + "</a>";
                                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, textChoice);
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
                                        HubContext.Instance.SendToClient(quest.QuestHint, player.HubGuid);
                                    }
                                }

                            }

                            foreach (var mob in getNewRoom.mobs.Where(x => x.Aggro).ToList())
                            {
                                HubContext.Instance.SendToClient($"{mob.Name} screams and attacks you", player.HubGuid);
                                Player.MobAttack(player, mob, getNewRoom);
                            }



                            if (!string.IsNullOrEmpty(getNewRoom.EventOnEnter))
                            {
                                Event.ParseCommand(getNewRoom.EventOnEnter, player, null, room);
                            }


                            Score.ReturnScoreUI(player);
                        }
                    }
                    catch (Exception e)
                    {
                        //log error
                    }
                }
                else
                {
                    HubContext.Instance.AddNewMessageToPage(player.HubGuid, "There is no exit here");

                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="room"></param>
        /// <param name="direction"></param>
        public static async Task MobMove(Player mob, Player ThingYourFollowing, Room room, string direction)
        {

 
            Room roomData = room;

            if (roomData.exits == null)
            {
                room.exits = new List<Exit>();
            }


            //Find Exit
            if (roomData.exits != null)
            {
                var exit = roomData.exits.Find(x => x.name.StartsWith(direction, StringComparison.CurrentCultureIgnoreCase));


                if (exit != null)
                {
                    if (exit.open == false)
                    {
                        return;
                    }

                    if (roomData.players.Count > 0)
                    {
                        foreach (var player in roomData.players)
                        {
                            HubContext.Instance.SendToClient(mob.Name + " leaves " + exit.name, player.HubGuid);
                        }
                    }

                    //remove player from old room
                    PlayerManager.RemoveMobFromRoom(roomData, mob);

                    //change player Location
                    mob.Area = exit.area;
                    mob.AreaId = exit.areaId;
                    mob.Region = exit.region;

                    //Get new room  
                    try
                    {
                        Room getNewRoom = MIMWebClient.Hubs.MIMHub.getRoom(mob);

                        if (getNewRoom != null)
                        {

                            //enter message
                            EnterRoom(mob, getNewRoom, direction);
                            //add player to new room
                            PlayerManager.AddMobToRoom(getNewRoom, mob);


                            //NPC Enter event here
                            foreach (var mobb in getNewRoom.mobs.Where(x => x.EventOnEnter != null).ToList())
                            {

                                if (!string.IsNullOrEmpty(mobb.EventOnEnter))
                                {
                                    Event.ParseCommand(mob.EventOnEnter, ThingYourFollowing, mobb, room);
                                }
                            }

                            if (!string.IsNullOrEmpty(room.EventOnEnter))
                            {
                                Event.ParseCommand(room.EventOnEnter, mob, null, room);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        //log error
                    }
                }

            }
        }


        public static async Task MobWalk(Player mob)
        {
            if (mob.Status == Player.PlayerStatus.Fighting || mob.Status == Player.PlayerStatus.Dead || mob.HitPoints <= 0)
            {
                return;
            }

            if (mob.PathList == null)
            {
                return;
            }

            var room = MIMHub.getRoom(mob);
            string pathDir;

            if (mob.PathCount == 0 || mob.PathCount >= mob.PathList.Count)
            {
               mob.PathCount = 0;
               pathDir = mob.PathList[0];
            }
            else
            {
                pathDir = mob.PathList[mob.PathCount];
            }

          await  MobMove(mob, null, room, pathDir);

            mob.PathCount++;

        }


        public static void ShowUIExits(Room room, string id)
        {
            if (room == null || string.IsNullOrEmpty(id))
            {
                return;
            }

            var exits = new List<string>();
            foreach (var exit in room.exits)
            {
                exits.Add(exit.name);
            }

            HubContext.Instance.UpdateExits(id, exits);
        }

    }

}
