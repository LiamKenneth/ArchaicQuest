using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    public class Welcome
    {
        public static void NewUser(PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room)
        {
            if (player.Type != PlayerSetup.Player.PlayerTypes.Player) return;

            var msg =
                "Welcome to Anker! I am here to guide you and I will keep it brief. To move type the direction shown in the exit list. North for example or n for short. You can also get items or drop them. Wield weapons, wear armour and kill. Kill the cat if you like.";

            HubContext.broadcastToRoom(mob.Name + " says " + msg, room.players, player.HubGuid);
        }
    }
}