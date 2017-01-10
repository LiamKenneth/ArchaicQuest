using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    public class Greeting
    {
        public static void greet(PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room, string message = "")
        {
            if (player.Type == PlayerSetup.Player.PlayerTypes.Player && message == string.Empty)
            {
                if (mob.GreetMessage != null)
                {
                    string greetMessageToRoom = mob.GreetMessage + " " + player.Name;
                    HubContext.broadcastToRoom(mob.Name + " says " + greetMessageToRoom, room.players, player.HubGuid);
                }
               
            }
            else
            {
               
            }
        }
    }
}