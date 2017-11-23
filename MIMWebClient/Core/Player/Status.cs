using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player
{
    public class Status
    {
        public static void WakePlayer(IHubContext context, PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                context.SendToClient("You wake and stand up", player.HubGuid);

                foreach (var character in room?.players)
                {
                    if (player != character)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wakes and stands up.";

                        context.SendToClient(roomMessage, character.HubGuid);
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
                context.SendToClient("You are already awake", player.HubGuid);
            }
        }

        public static void SleepPlayer(IHubContext context, PlayerSetup.Player player, Room.Room room)
        {

            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                context.SendToClient("You are sleeping.", player.HubGuid);
                return;
            }

            if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                context.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }

            if (room.terrain == Room.Room.Terrain.Water)
            {
                context.SendToClient("You can't sleep here.", player.HubGuid);
                return;
            }

            player.Status = PlayerSetup.Player.PlayerStatus.Sleeping;

            context.SendToClient("You laydown and go to sleep", player.HubGuid);

            foreach (var character in room?.players)
            {
                if (player != character)
                {

                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} lays down and goes to sleep.";

                    context.SendToClient(roomMessage, character.HubGuid);
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

        public static void RestPlayer(IHubContext context, PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                context.SendToClient("You are sleeping.", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                context.SendToClient("You are already resting.", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                context.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }

            player.Status = PlayerSetup.Player.PlayerStatus.Resting;

            context.SendToClient("You sit down and rest.", player.HubGuid);

            foreach (var character in room.players)
            {
                if (player != character)
                {
                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} sits down and rests.";

                    context.SendToClient(roomMessage, character.HubGuid);
                }
            }
        }

        public static void StandPlayer(IHubContext context, PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                context.SendToClient("You wake and stand up.", player.HubGuid);
                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                foreach (var character in room?.players)
                {
                    if (player != character)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wakes and stands up.";

                        context.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                context.SendToClient("You stop resting and stand up.", player.HubGuid);

                player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                foreach (var character in room?.players)
                {
                    if (player != character)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} stops resting and stands up.";

                        context.SendToClient(roomMessage, character.HubGuid);
                    }
                }


                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                context.SendToClient("You are in a middle of a fight!", player.HubGuid);
                return;
            }
            else if (player.Status == PlayerSetup.Player.PlayerStatus.Standing)
            {
                context.SendToClient("You are standing already.", player.HubGuid);
                return;
            }
        }
    }
}