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

                HubContext.Instance.SendToClient("You wake and stand up", player.HubGuid);

                foreach (var character in room.players)
                {
                    if (player != character)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wakes and stands up.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
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
                HubContext.Instance.SendToClient("You are already awake", player.HubGuid);
            }
        }

        public static void SleepPlayer(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status != PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                player.Status = PlayerSetup.Player.PlayerStatus.Sleeping;

                HubContext.Instance.SendToClient("You laydown and go to sleep", player.HubGuid);

                foreach (var character in room.players)
                {
                    if (player != character)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} lays down and goes to sleep.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
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
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                HubContext.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }
            else
            {
                HubContext.Instance.SendToClient("You are already asleep", player.HubGuid);
            }
        }

        public static void RestPlayer(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                HubContext.SendToClient("You are sleeping.", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                HubContext.SendToClient("You are already resting.", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                HubContext.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }

                player.Status = PlayerSetup.Player.PlayerStatus.Resting;

            HubContext.SendToClient("You sit down and rest.", player.HubGuid);

            foreach (var character in room.players)
            {
                if (player != character)
                {
                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} sits down and rests.";

                    HubContext.SendToClient(roomMessage, character.HubGuid);
                }
            }
        }

        public static void StandPlayer(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                HubContext.SendToClient("You wake and stand up.", player.HubGuid);
                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                foreach (var character in room.players)
                {
                    if (player != character)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wakes and stands up.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                HubContext.SendToClient("You stop resting and stand up.", player.HubGuid);

                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                foreach (var character in room.players)
                {
                    if (player != character)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} stops resting and stands up.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }


                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                HubContext.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Standing)
            {
                HubContext.SendToClient("You are standing already.", player.HubGuid);
                return;
            }
        }
    }
}