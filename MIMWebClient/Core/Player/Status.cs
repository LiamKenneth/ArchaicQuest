using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player
{
    public class Status
    {
        public static void WakePlayer(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                HubContext.SendToClient("You wake and stand up", player.HubGuid);

                if (!string.IsNullOrEmpty(room.EventWake))
                {
                    Event.ParseCommand(room.EventWake, player, null, room, "wake");
                }

 
                foreach (var mob in room.mobs)
                {

                    if (!string.IsNullOrEmpty(mob.EventWake))
                    {
                        Event.ParseCommand(mob.EventWake, player, mob, room, "wake");
                    }
 
                }

                Command.ParseCommand("look", player, room);
            }

            else
            {
                HubContext.SendToClient("You are already awake", player.HubGuid);
            }
        }

        public static void SleepPlayer(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status != PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                player.Status = PlayerSetup.Player.PlayerStatus.Sleeping;

                HubContext.SendToClient("You laydown and go to sleep", player.HubGuid);

                if (!string.IsNullOrEmpty(room.EventWake))
                {
                    Event.ParseCommand(room.EventWake, player, null, room, "sleep");
                }


                foreach (var mob in room.mobs)
                {

                    if (!string.IsNullOrEmpty(mob.EventWake))
                    {
                        Event.ParseCommand(mob.EventWake, player, mob, room, "sleep");
                    }

                }
            }

            else
            {
                HubContext.SendToClient("You are already asleep", player.HubGuid);
            }
        }
    }
}