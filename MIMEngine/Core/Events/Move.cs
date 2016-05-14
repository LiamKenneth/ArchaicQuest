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

            JToken nextRoomInfo =  findExit(room, direction);

            
            // change char location to new room

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
