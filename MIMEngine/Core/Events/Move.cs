using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using MIMEngine.Core.PlayerSetup;

    using Newtonsoft.Json.Linq;

    class Move
    {
        public static void  MoveCharacter(PlayerSetup character, JObject room, string direction)
        {
            // char location
             string regionName = character.Area;
              string areaName = character.Area;
              int roomId = character.AreaId;


            // check direction exists for the room the player in
            var roomExitObj = room.Property("exits").Children();
            string newRegionName = string.Empty;
            string newAreaName = string.Empty;
            int newRoomId = 0;

            foreach (var exit in roomExitObj)
            {
                if (exit["North"] != null)
                {
                   newRegionName = (string)exit["North"]["region"];
                    newAreaName = (string)exit["North"]["area"];
                    newRoomId = (int)exit["North"]["areaId"];

                }
            }

            //updates player location
            PlayerSetup updateChar = character;
            updateChar.Region = newRegionName;
            updateChar.Area = newAreaName;
            updateChar.AreaId = newRoomId;
            //update player cache
            HubProxy.MimHubServer.Invoke("updatePlayer", character.HubGuid, updateChar);


            HubProxy.MimHubServer.Invoke("addToRoom", newRoomId, room, updateChar, "player");


            // change char location to new room
            var players = room["players"];

            foreach (var player in players)
            {
                if ((string)player["name"] == character.Name)
                {
                    player.Remove();
                }
            }

            // remove char from current room

            // add char to new room

            // send enter message to other players

        }

        private static JToken findExit(JObject room, string direction)
        {
            var roomExitObj = room.Property("exits").Children();


            string exitList = null;
            foreach (var exit in roomExitObj)
            {
                if (exit["North"] != null)
                {
                    return exit["north"];
                }

                if (exit["East"] != null)
                {
                    return exit["east"];
                }

               

            }

            return "You cannot go that way";
        }
    }
}
