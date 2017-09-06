using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

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

                foreach (var character in room.players)
                {
                    if (player != character)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wakes and stands up.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

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

                CheckEvent.FindEvent(CheckEvent.EventType.Wake, player, "awakening awake");
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

                foreach (var character in room.players)
                {
                    if (player != character)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} lays down and goes to sleep.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

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