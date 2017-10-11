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
            var isMobHere = room.mobs.FirstOrDefault(x => x.Name.Equals(mob.Name));

            if (isMobHere == null)
            {
                return;
            }

            if (player.Type == PlayerSetup.Player.PlayerTypes.Player && message == string.Empty)
            {
                if (!string.IsNullOrEmpty(mob.GreetMessage))
                {
                    string greetMessageToRoom = mob.GreetMessage + " " + player.Name;
                     
                    foreach (var character in room.players)
                    {
                          
                            var roomMessage = $"{ Helpers.ReturnName(mob, character, string.Empty)} says \"{greetMessageToRoom}\"";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                         
                    }
                }
               
            }
            else
            {
               
            }
        }
    }
}