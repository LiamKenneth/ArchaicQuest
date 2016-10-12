using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    public class Greeting
    {
        public static void greet(PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room)
        {
            if (player.Type == PlayerSetup.Player.PlayerTypes.Player)
            {
                string greetMessageToRoom = string.Format(mob.GreetMessage, player.Name);
                HubContext.broadcastToRoom(mob.Name + " Says " + greetMessageToRoom, room.players, player.HubGuid);
            }
            else
            {
                //Greet other mob
            }
        }
    }
}